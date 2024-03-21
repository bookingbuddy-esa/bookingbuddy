import { Component, ElementRef, QueryList, ViewChildren } from '@angular/core';
import {environment} from "../../environments/environment";
import { ActivatedRoute, Router } from '@angular/router';
import { GroupService } from './group.service';
import { timeout } from 'rxjs';
import { AuthorizeService } from '../auth/authorize.service';
import { UserInfo } from '../auth/authorize.dto';
import { Group, GroupAction } from '../models/group';
import { Property } from '../models/property';
import { Discount } from '../models/discount';
import { MatCalendarCellClassFunction, MatDatepickerInputEvent } from '@angular/material/datepicker';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PropertyAdService } from '../property-ad/property-ad.service';
import { PaymentService } from '../payment/payment.service';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrl: './group.component.css'
})
export class GroupComponent {
  @ViewChildren('successAlerts, errorAlerts') alertContainers!: QueryList<ElementRef>;
  success_alerts: string[] = [];
  errors: string[] = [];
  submitting: boolean = false;
  user: UserInfo | undefined;
  group_list: Group[] = [];
  currentGroup: Group | undefined;
  ws: WebSocket | undefined;
 
  newMessage: string = '';
  modalOpen: boolean = false;
  isGroupOwner: boolean = false;

  // Datas de Reservas
  calendarMaxDate: Date = new Date(8640000000000000);
  maxDate: Date = this.calendarMaxDate;
  blockedDates: Date[] = [];
  discounts: Discount[] = [];
  discountDates: Date[] = [];
  checkInDate: Date | undefined;
  checkOutDate: Date | undefined;
  reservarPropriedadeForm!: FormGroup;
  reservarPropriedadeFailed: boolean;
  pricesMap: Map<number, number> = new Map<number, number>();

  constructor(private authService: AuthorizeService,
              private formBuilder: FormBuilder, 
              private route: ActivatedRoute, 
              private router: Router, 
              private groupService: GroupService, 
              private propertyService: PropertyAdService,
              private paymentService: PaymentService) {
    this.reservarPropriedadeFailed = false;

    this.reservarPropriedadeForm = this.formBuilder.group({
      checkIn: ['', Validators.required],
      checkOut: ['', Validators.required]
    });
  }

  ngOnInit(): void {
      this.authService.user().forEach(async user => {
        this.user = user;
        this.setupGroupById().then(() => {
          this.loadUserGroups();
        });
      });
  }

  get groupAction(): string {
    if (this.currentGroup) {
      switch (this.currentGroup.groupAction) {
        case GroupAction.none:
          return 'None';
        case GroupAction.voting:
          return 'Voting';
        case GroupAction.paying:
          return 'Paying';
      }
    }

    return 'None';
  }

  public copyGroupLink(): void {
    let url = window.location.href;
    navigator.clipboard.writeText(url);
  }

  public setGroupAction(action: string): void {
    // update group action to voting (in the db) and send to WS

    switch (action) {
      case 'voting':
        this.currentGroup!.groupAction = GroupAction.voting;
        break;
      case 'paying':
        this.currentGroup!.groupAction = GroupAction.paying;
        break;
    }

    this.sendMessageWS();
  }

  public deleteGroup(): void {
    this.groupService.deleteGroup(this.currentGroup!.groupId).forEach(response => {
      if (response) {
        this.success_alerts.push("Grupo removido com sucesso!");
        this.loadUserGroups();
        this.currentGroup = undefined;
        this.router.navigateByUrl('/groups');
      }
    }).catch(error => {
      this.errors.push(error.error);
    });
  }

  public sendMessageWS(): void {
    if(this.ws){
      this.ws.send(JSON.stringify(this.currentGroup));
      console.log("Sending to WS: " + JSON.stringify(this.currentGroup));
    }
  }

  public sendMessage(): void {
    if (this.newMessage.trim() !== '') {
      let message = {
        userName: this.user!.name,
        message: this.newMessage.trim()
      };

      this.groupService.sendGroupMessage(this.currentGroup!.groupId, message.message).forEach(response => {
        if (response) {
          console.log(response);
          this.currentGroup!.messages.push(message);
          this.sendMessageWS();
        }
      })

      this.newMessage = '';
    }
  }

  public removeProperty(property: Property) {
    this.groupService.removePropertyFromGroup(this.currentGroup?.groupId!, property.propertyId).forEach(response => {
      if (response) {
        //console.log(response);
      }
    });
    const idIndex = this.currentGroup?.propertiesId.indexOf(property.propertyId);
    const propertyIndex = this.currentGroup?.properties.indexOf(property);
    this.currentGroup?.propertiesId.splice(idIndex!, 1);
    this.currentGroup?.properties.splice(propertyIndex!, 1);
  }
  
  public chooseGroup(group: Group): void {
    console.log("Escolher este grupo: " + JSON.stringify(group));
    this.errors = [];
    this.success_alerts = [];
    this.router.navigate([], { queryParams: { groupId: group.groupId }});

    this.currentGroup = group;
    this.isGroupOwner = this.currentGroup.groupOwnerId == this.user?.userId;

    this.loadDiscounts();
    this.loadBlockedDates();

    let url = environment.apiUrl;
    url = url.replace('https', 'wss');

    if (this.ws) {
      this.ws.close();
    }

    this.ws = new WebSocket(`${url}/api/groups/ws?groupId=${group.groupId}`);
    this.ws.onmessage = (event) => {
      console.log("Mensagem recebida: " + event.data);
      let newGroupState = JSON.parse(event.data);

      let index = this.group_list.findIndex(g => g.groupId == newGroupState.groupId);
      if(index >= 0){
        this.group_list[index] = newGroupState;
      }

      if(this.currentGroup?.groupId == newGroupState.groupId){
        this.currentGroup = newGroupState;
      }
    };
  }

  public getPropertyImage(property: Property) {
    if (property && property.imagesUrl && property.imagesUrl.length > 0) {
      return property.imagesUrl[0];
    }

    return 'N/A'; // TODO: foto default caso nao tenha?
  }

  public getGroupMembers(): number {
    if(this.currentGroup && this.currentGroup?.members.length > 0){
      return this.currentGroup.members.length;
    }

    return 0;
  }

  private loadUserGroups() {
    this.groupService.getGroupsByUserId(this.user!.userId).pipe(timeout(10000)).forEach(groups => {
      //console.log("Grupos Recebidos deste User: " + JSON.stringify(groups));
      /*for (let i = 0; i < 10; i++){
        let p: Group = {
          groupId: '123',
          groupOwnerId: '123',
          name: 'grupo ' + i,
          membersId: [],
          members: [],
          propertiesId: [],
          properties: [],
          choosenProperty: 'ya',
          messages: []
        }

        this.group_list.push(p);
      }*/
      
      this.group_list = groups;
      this.submitting = false;
    }).catch(error => {
      console.log(error.error);
      //this.errors.push(error.error);
      this.submitting = false;
      //console.log("Erro ao receber grupos: " + JSON.stringify(error));
    });
  }

  private async setupGroupById() {
    if(this.user){
      this.submitting = true;
      this.route.queryParams.forEach(params => {
        if (params['groupId']) {
          this.groupService.getGroup(params['groupId']).forEach(getGroupResponse => {
            if (getGroupResponse) {
              const group: Group = getGroupResponse as Group;
              let isMemberOfGroup = group.membersId.includes(this.user!.userId);
              if (!isMemberOfGroup) {
                this.groupService.addMemberToGroup(params['groupId']).forEach(addMemberResponse => {
                  if (addMemberResponse) {
                    this.loadUserGroups();
                    this.success_alerts.push("Membro adicionado ao grupo com sucesso!");
                  }
                }).catch(error => {
                  this.errors.push("Erro ao adicionar membro ao grupo!");
                  //console.log("Erro ao adicionar membro ao grupo: " + JSON.stringify(error));
                });
              }
            }

            return Promise.resolve();
          }).catch(error => {
            this.errors.push(error.error);
            //console.log("Erro ao receber grupo: " + JSON.stringify(error));
          });
        }
      });
    }
  }

  dateFilter = (date: Date | null): boolean => {
    if (!date) {
      return false;
    }

    const currentDate = new Date();

    return date >= currentDate && !this.blockedDates.some(blockedDate => this.isSameDay(date, blockedDate));
  };

  onDateChange(event: MatDatepickerInputEvent<Date>, type: 'start' | 'end'): void {
    if (type === 'start' && event.value) {
      this.checkInDate = event.value;
      this.updateMaxDate();
    } else if (event.value){        
      this.checkOutDate = event.value;
    }
  }

  clearDates() {
    this.reservarPropriedadeForm.patchValue({
      checkIn: null,
      checkOut: null
    });

    this.checkInDate = this.checkOutDate = undefined;
    this.maxDate = this.calendarMaxDate;
  }

  discountClass: MatCalendarCellClassFunction<Date> = (cellDate, view) => {
    if (this.discountDates.some(discountDate => this.isSameDay(cellDate, discountDate))) {
      return 'discount-date-class';
    }

    return '';
  };

  private isSameDay(date1: Date, date2: Date): boolean {
    return date1.getFullYear() === date2.getFullYear() &&
      date1.getMonth() === date2.getMonth() &&
      date1.getDate() === date2.getDate();
  }

  updateMaxDate(): void {
    if (this.checkInDate) {
      const nextBlockedDate = this.blockedDates.find(date => date > this.checkInDate!);

      if (nextBlockedDate) {
        this.maxDate = nextBlockedDate;
      } else {  
        this.maxDate =this.calendarMaxDate;
      }
    }
  }

  public reservar(_: any) {
    if(this.currentGroup){
      this.reservarPropriedadeFailed = false;
      const checkInDate: Date = new Date(this.reservarPropriedadeForm.get('checkIn')?.value);
      const checkOutDate: Date = new Date(this.reservarPropriedadeForm.get('checkOut')?.value);

      // TODO: verificar se datas sao validas antes de fazer a order
      /*this.router.navigate(['/transaction-handler'], { 
          queryParams: {
              groupId: this.currentGroup?.groupId,
              startDate: checkInDate.toISOString().split('T')[0],
              endDate: checkOutDate.toISOString().split('T')[0],
              orderType: 'group-booking'
          }
      });*/

      this.paymentService.createBookingOrder(this.currentGroup?.groupId!, checkInDate, checkOutDate).forEach(response => {
        if (response) {
          //this.currentGroup!.groupAction = "Payment";
          
          this.sendMessageWS();
        }
      }).catch(error => {
        this.reservarPropriedadeFailed = true;
      });
    }
  }

  loadBlockedDates() {
    if (this.currentGroup?.choosenProperty) {
      this.propertyService.getPropertyBlockedDates(this.currentGroup?.choosenProperty).forEach((dateRanges: any[]) => {
        this.blockedDates = [];

        dateRanges.forEach(dateRange => {
          const startDate = new Date(dateRange.start);
          const endDate = new Date(dateRange.end);
          const currentDate = new Date(startDate);
          while (currentDate <= endDate) {
            this.blockedDates.push(new Date(currentDate));
            currentDate.setDate(currentDate.getDate() + 1);
          }
        });

      }).catch(error => {
        console.error('Erro ao carregar intervalos de datas bloqueadas:', error);
      });
    }
  }

  loadDiscounts() {
    if (this.currentGroup?.choosenProperty) {
      this.propertyService.getPropertyDiscounts(this.currentGroup?.choosenProperty).forEach((dateRanges: any[]) => {
        this.discounts = [];

        dateRanges.forEach(dateRange => {
          const discount: Discount = {
            discountId: dateRange.discountId,
            startDate: dateRange.startDate,
            endDate: dateRange.endDate,
            discountAmount: dateRange.discountAmount,
            dates: []
          };

          const startDate = new Date(dateRange.startDate);
          const endDate = new Date(dateRange.endDate);
          const currentDate = new Date(startDate);

          while (currentDate <= endDate) {
            discount.dates.push(new Date(currentDate));
            this.discountDates.push(new Date(currentDate));
            currentDate.setDate(currentDate.getDate() + 1);
          }

          this.discounts.push(discount);
        });
      }).catch(error => {
        console.error('Erro ao carregar intervalos de datas bloqueadas:', error);
      }
      );
    }
  }

  formatPricesMap(): string[] {
    const formattedStrings: string[] = [];

    this.pricesMap.forEach((count, price) => {
      formattedStrings.push(`${price}€ x ${count} noites - ${Math.round(((price * count) + Number.EPSILON) * 100)/ 100}€`);
    });

    return formattedStrings;
  }

  calcularTotalDesconto() {
    let pricePerNight = this.currentGroup?.properties.find(p => p.propertyId === this.currentGroup?.choosenProperty)?.pricePerNight;
    const selectedDates: Date[] = [];
    this.pricesMap = new Map();
    if (this.currentGroup?.choosenProperty && this.checkInDate && this.checkOutDate) {
      const currentDate = new Date(this.checkInDate);
      while (currentDate < this.checkOutDate) {
        selectedDates.push(new Date(currentDate));
        currentDate.setDate(currentDate.getDate() + 1);
      }

      selectedDates.forEach(selectedDate => {
        const matchingDiscounts = this.discounts.filter(discount =>
          discount.dates.some(date => this.isSameDay(date, selectedDate))
        );

        if (matchingDiscounts.length > 0) {
          matchingDiscounts.forEach(discount => {
            const newPrice = this.priceWithDiscount(discount.discountAmount);
            const currentCount = this.pricesMap.get(newPrice) || 0;
            this.pricesMap.set(newPrice, currentCount + 1);
          });
        } else {
          const currentCount = this.pricesMap.get(pricePerNight as number) || 0;
          this.pricesMap.set(pricePerNight as number, currentCount + 1);
        }
      });

      let totalPrice = 0;
      this.pricesMap.forEach((count, price) => {
        const aux = count * price;
        totalPrice += aux;
      });
      return Math.round(((totalPrice /*+ 25 + 20*/) + Number.EPSILON) * 100) / 100;
    }
    return 0;
  }

  priceWithDiscount(discountAmount: number): number {
    let pricePerNight = this.currentGroup?.properties.find(p => p.propertyId === this.currentGroup?.choosenProperty)?.pricePerNight;
    if (this.currentGroup?.choosenProperty && pricePerNight) {
      const discountMultiplier = 1 - discountAmount / 100;

      return Math.round(((pricePerNight * discountMultiplier) + Number.EPSILON) * 100) / 100
    }
    return 0;
  }
}
