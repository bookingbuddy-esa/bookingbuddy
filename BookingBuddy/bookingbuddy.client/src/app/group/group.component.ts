import {ChangeDetectorRef, Component, ElementRef, EventEmitter, OnInit, QueryList, ViewChildren} from '@angular/core';
import {environment} from "../../environments/environment";
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import {GroupService} from './group.service';
import {AuthorizeService} from '../auth/authorize.service';
import {UserInfo} from '../auth/authorize.dto';
import {Group, GroupAction, GroupMember, GroupProperty} from '../models/group';
import {Property} from '../models/property';
import {Discount} from '../models/discount';
import {MatCalendarCellClassFunction, MatDatepickerInputEvent} from '@angular/material/datepicker';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {PropertyAdService} from '../property-ad/property-ad.service';
import {OrderService} from '../payment/order.service';
import {WebsocketMessage} from "../models/websocket-message";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {AuxiliaryModule} from "../auxiliary/auxiliary.module";
import {NgIf} from "@angular/common";

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrl: './group.component.css'
})
export class GroupComponent implements OnInit {
  @ViewChildren('successAlerts, errorAlerts') alertContainers!: QueryList<ElementRef>;
  success_alerts: string[] = [];
  errors: string[] = [];
  protected readonly navigator = navigator;

  // Carregamento de dados

  global_loading: boolean = false;
  submitting: boolean = false;
  submitting_invite: boolean = false;

  // Utilizador, grupos e reservas

  user: UserInfo | undefined;
  group_list: Group[] = [];
  currentGroup: Group | undefined;
  isGroupOwner: boolean = false;
  bookingData: any;
  ws: WebSocket | undefined;

  // Convidar para grupo

  invite_link: boolean = false;
  failed_invite: boolean = false;
  pendingGroup: Group | undefined;
  //acceptInviteModal: Modal | undefined;

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
              protected router: Router,
              private cdref: ChangeDetectorRef,
              private groupService: GroupService,
              private propertyService: PropertyAdService,
              private modalService: NgbModal,
              private orderService: OrderService) {
    this.reservarPropriedadeFailed = false;

    this.reservarPropriedadeForm = this.formBuilder.group({
      checkIn: ['', Validators.required],
      checkOut: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.global_loading = true;
    this.authService.user().forEach(async user => {
      this.user = user;
    }).then(() => {
      this.loadGroups().then(() => {
        this.global_loading = false;
      });
    }).catch(error => {
      console.error('Erro ao carregar utilizador:', error);
      this.global_loading = false;
    });
  }

  async loadGroups() {
    if (this.user) {
      return this.groupService.getGroupsByUserId(this.user.userId).forEach(groups => {
        this.group_list = groups;
      }).then(async () => {
        let queryGroupId = this.route.snapshot.queryParams['groupId'];
        if (queryGroupId) {
          let group = this.group_list.find(g => g.groupId == queryGroupId);
          if (group) {
            this.chooseGroup(group);
            await this.initWebSocket(group);
          } else {
            this.invite_link = true;
            this.groupService.getGroup(queryGroupId).forEach(group => {
              this.pendingGroup = group;
            }).then(() => {
              let modalRef = this.modalService.open(AcceptInviteModal,
                {
                  backdrop: 'static',
                  keyboard: false,
                  animation: true,
                  centered: true,
                });
              modalRef.componentInstance.pendingGroup = this.pendingGroup;
              modalRef.componentInstance.failed_invite = this.failed_invite;
              modalRef.componentInstance.submitting_invite = this.submitting_invite;
              modalRef.componentInstance.onAcceptInvite.forEach(() => {
                  this.acceptInvite()
                  modalRef.close();
                }
              );
              modalRef.componentInstance.onClose.forEach(() => {
                  modalRef.close();
                  this.router.navigate(['/groups']);
                }
              )
            }).catch(error => {
              console.error('Erro ao carregar grupo:', error);
              this.router.navigate(['/groups']);
            });
          }
        }
      }).catch(error => {
        console.error('Erro ao carregar grupos:', error);
      });
    }
  }

  async initWebSocket(group: Group) {
    let url = environment.apiUrl;
    url = url.replace('https', 'wss');
    if (this.ws) this.ws.close();
    this.ws = new WebSocket(`${url}/api/groups/ws?groupId=${group.groupId}`);
    this.ws.onmessage = (event) => {
      let message = JSON.parse(event.data) as WebsocketMessage;
      switch (message.code) {
        case 'PropertyRemoved':
          let content = JSON.parse(message.content) as { propertyId: string }
          this.currentGroup!.properties = this.currentGroup!.properties.filter(p => p.propertyId != content?.propertyId);
          break;
        case 'PropertyAdded':
          let property = JSON.parse(message.content) as GroupProperty;
          if (!this.currentGroup?.properties.find(p => p.propertyId == property.propertyId)) {
            this.currentGroup!.properties.push(property);
          }
          break;
        case 'MemberAdded':
          let member = JSON.parse(message.content) as GroupMember;
          if (!this.currentGroup?.members.find(m => m.id == member.id)) {
            this.currentGroup!.members.push(member);
          }
          break;
        case 'GroupDeleted':
          let groupId = JSON.parse(message.content) as { groupId: string };
          if (this.group_list.find(g => g.groupId == groupId.groupId)) {
            if (this.currentGroup?.groupId == groupId.groupId) {
              this.currentGroup = undefined;
              this.router.navigate(['/groups']);
            }
            this.group_list = this.group_list.filter(g => g.groupId != groupId.groupId);
          }
      }
    };
  }

  acceptInvite() {
    this.submitting_invite = true;
    this.groupService.addMemberToGroup(this.pendingGroup!.groupId).forEach(response => {
      if (response) {
        this.submitting_invite = false;
      }
    }).then(() => {
      this.global_loading = true;
      this.loadGroups()
        .then(() => {
          this.global_loading = false;
        });
    }).catch(error => {
      console.error('Erro ao aceitar convite:', error);
      this.failed_invite = true;
    });
  }

  setBookingData(): any {
    return this.bookingData = {
      groupBookingId: this.currentGroup?.groupBookingId,
      orderType: 'group-booking'
    };
  }

  public copyGroupLink(): void {
    let url = window.location.href;
    navigator.clipboard.writeText(url);
  }

  public setGroupAction(action: string): void {
    if (!this.currentGroup || action != 'voting' && action != 'booking' && action != 'paying') {
      return;
    }

    this.groupService.setGroupAction(this.currentGroup!.groupId, action).forEach(response => {
      if (response) {
        //this.success_alerts.push("Ação do grupo atualizada com sucesso!");
        // switch (action) {
        //   case 'voting':
        //     this.currentGroup!.groupAction = GroupAction.voting;
        //     // TODO: enviar alert do tipo "info" (fazer um alerta genérico em vez de success e error)
        //     break;
        //   case 'booking':
        //     this.currentGroup!.groupAction = GroupAction.booking;
        //     break;
        //   case 'paying':
        //     this.currentGroup!.groupAction = GroupAction.paying;
        //     break;
        // }

        this.sendMessageWS();
      }
    }).catch(error => {
      //this.errors.push(error.error);
      console.log("ERROR: " + JSON.stringify(error));
    });
  }

  public deleteGroup(): void {
    this.groupService.deleteGroup(this.currentGroup!.groupId).forEach(async response => {
      if (response) {
        this.currentGroup = undefined;
        this.group_list = this.group_list.filter(g => g.groupId != this.currentGroup?.groupId);
        await this.router.navigate(['/groups']);
      }
    }).catch(error => {
      this.errors.push(error.error);
    });
  }

  public sendMessageWS(): void {
    if (this.ws) {
      this.ws.send(JSON.stringify(this.currentGroup));
      console.log("Sending to WS: " + JSON.stringify(this.currentGroup));
    }
  }


  public setChoosenProperty(property: Property) {
    /*this.groupService.setChoosenProperty(this.currentGroup!.groupId, property.propertyId).forEach(response => {
      if (response) {
        console.log(response);
        this.currentGroup!.choosenProperty = property.propertyId;
        this.sendMessageWS();
      }
    });*/
    if (property) {
      this.currentGroup!.chosenProperty = property.propertyId;
      this.sendMessageWS();
    }
  }

  public setChoosenPropertyDEV(property: Property) {
    this.groupService.setChoosenProperty(this.currentGroup!.groupId, property.propertyId).forEach(response => {
      if (response) {
        console.log(response);
        this.currentGroup!.chosenProperty = property.propertyId;
        this.sendMessageWS();
      }
    }).catch(error => {
      console.log("Erro ao setChoosenProperty: " + JSON.stringify(error));
      //this.errors.push(error.error);
    });
  }

  public removeProperty(propertyId: string) {
    this.groupService.removePropertyFromGroup(this.currentGroup?.groupId!, propertyId).forEach(response => {
      if (response) {
        this.currentGroup!.properties = this.currentGroup!.properties.filter(p => p.propertyId != propertyId);
      }
    }).catch(error => {
        console.log("Erro ao remover propriedade: " + JSON.stringify(error));
      }
    );
  }

  public chooseGroup(group: Group): void {
    this.errors = [];
    this.success_alerts = [];
    this.router.navigate([], {queryParams: {groupId: group.groupId}});

    this.currentGroup = group;
    this.isGroupOwner = group.groupOwner.id === this.user?.userId;

    this.loadDiscounts();
    this.loadBlockedDates();
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
    } else if (event.value) {
      this.checkOutDate = event.value;
    }
    this.cdref.detectChanges();
  }

  clearDates() {
    this.reservarPropriedadeForm.patchValue({
      checkIn: null,
      checkOut: null
    });

    this.checkInDate = this.checkOutDate = undefined;
    this.maxDate = this.calendarMaxDate;
    this.cdref.detectChanges();
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
        this.maxDate = this.calendarMaxDate;
      }
    }
  }

  public reservar(_: any) {
    if (this.currentGroup) {
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

      this.orderService.createBookingOrder(this.currentGroup?.groupId!, checkInDate, checkOutDate).forEach(response => {
        if (response) {
          //console.log(response);
          /*this.bookingData = {
            groupBookingId: response.orderId,
            orderType: 'group-booking'
          };*/

          this.currentGroup!.groupBookingId = response.orderId;
          this.setGroupAction('paying');
          //this.sendMessageWS();
        }
      }).catch(error => {
        this.reservarPropriedadeFailed = true;
      });
    }
  }

  loadBlockedDates() {
    if (this.currentGroup?.chosenProperty) {
      this.propertyService.getPropertyBlockedDates(this.currentGroup?.chosenProperty).forEach((dateRanges: any[]) => {
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
    if (this.currentGroup?.chosenProperty) {
      this.propertyService.getPropertyDiscounts(this.currentGroup?.chosenProperty).forEach((dateRanges: any[]) => {
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
      formattedStrings.push(`${price}€ x ${count} noites - ${Math.round(((price * count) + Number.EPSILON) * 100) / 100}€`);
    });

    return formattedStrings;
  }

  calcularTotalDesconto() {
    let pricePerNight = this.currentGroup?.properties.find(p => p.propertyId === this.currentGroup?.chosenProperty)?.pricePerNight;
    const selectedDates: Date[] = [];
    this.pricesMap = new Map();
    if (this.currentGroup?.chosenProperty && this.checkInDate && this.checkOutDate) {
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
    let pricePerNight = this.currentGroup?.properties.find(p => p.propertyId === this.currentGroup?.chosenProperty)?.pricePerNight;
    if (this.currentGroup?.chosenProperty && pricePerNight) {
      const discountMultiplier = 1 - discountAmount / 100;

      return Math.round(((pricePerNight * discountMultiplier) + Number.EPSILON) * 100) / 100
    }
    return 0;
  }

  redirectToProperty(propertyId: string) {
    this.router.navigate(['/property', propertyId]);
  }
}

@Component({
  standalone: true,
  template: `
    <div class="modal-header">
      <h5 class="modal-title" id="acceptInviteLabel">Convite para grupo de reserva</h5>
      <button *ngIf="failed_invite && !submitting_invite" type="button" class="btn-close" aria-label="Close"
              (click)="this.onClose.emit()"></button>
    </div>
    <div class="modal-body">
      <span *ngIf="!failed_invite && !submitting_invite">
        Aceitar o convite para o grupo de reserva <strong>{{ pendingGroup?.name }}</strong>?
      </span>
      <span *ngIf="failed_invite && !submitting_invite"
            class="text-center">Ocorreu um erro ao aceitar o convite!</span>
      <app-loader class="m-3" *ngIf="submitting_invite"></app-loader>
    </div>
    <div *ngIf="!submitting_invite" class="modal-footer">
      <button *ngIf="!failed_invite" type="button" class="btn btn-danger" data-bs-dismiss="modal"
              (click)="this.onClose.emit()">
        Rejeitar
      </button>
      <button *ngIf="!failed_invite" type="button" class="btn btn-success"
              (click)="this.onAcceptInvite.emit();">
        Aceitar
      </button>
      <button *ngIf="failed_invite" type="button" class="btn btn-danger" data-bs-dismiss="modal"
              (click)="this.onClose.emit()">
        Fechar
      </button>
    </div>
  `,

  imports: [
    RouterLink,
    AuxiliaryModule,
    NgIf
  ]
})
export class AcceptInviteModal {
  failed_invite: boolean = false;
  submitting_invite: boolean = false;
  pendingGroup: Group | undefined;
  onAcceptInvite: EventEmitter<void> = new EventEmitter<void>();
  onClose: EventEmitter<void> = new EventEmitter<void>();
}
