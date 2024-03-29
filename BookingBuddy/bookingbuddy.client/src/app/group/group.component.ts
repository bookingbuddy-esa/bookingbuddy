import {AfterViewInit, Component, HostListener, inject, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {environment} from "../../environments/environment";
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import {GroupService} from './group.service';
import {AuthorizeService} from '../auth/authorize.service';
import {UserInfo} from '../auth/authorize.dto';
import {Group, GroupAction, GroupActionHelper, GroupMember, GroupProperty} from '../models/group';
import {WebsocketMessage} from "../models/websocket-message";
import {NgbActiveModal, NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {AuxiliaryModule} from "../auxiliary/auxiliary.module";
import {KeyValue, NgIf} from "@angular/common";
import {v4 as uuidv4} from 'uuid';
import {group} from "@angular/animations";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {
  DateFilterFn,
  MatCalendarCellClassFunction,
  MatDatepickerInputEvent,
  MatDateRangeInput
} from "@angular/material/datepicker";
import {PropertyAdService, ReturnedDiscount} from "../property-ad/property-ad.service";

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrl: './group.component.css'
})
export class GroupComponent implements OnInit, OnDestroy {
  // Serviços

  private authService: AuthorizeService = inject(AuthorizeService);
  private route: ActivatedRoute = inject(ActivatedRoute);
  protected router: Router = inject(Router);
  private groupService: GroupService = inject(GroupService);
  private modalService: NgbModal = inject(NgbModal);
  private formBuilder: FormBuilder = inject(FormBuilder);
  private propertyService: PropertyAdService = inject(PropertyAdService);

  // Responsividade

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.currentWidth = event.target.innerWidth;
  }
  protected currentWidth: number = window.innerWidth;

  // Carregamento de dados

  protected global_loading: boolean = false;
  protected submitting: boolean = false;

  // Utilizador, grupos e reservas

  protected user: UserInfo | undefined;
  protected group_list: Group[] = [];
  protected currentGroup: Group | undefined;

  // WebSocket

  protected ws_uuid: string | undefined;
  protected ws: WebSocket | undefined;
  protected ws_loading: boolean = false;

  // Convidar para grupo

  protected failed_invite: boolean = false;
  protected pendingGroup: Group | undefined;
  protected invite_loading: boolean = false;

  // Reservar propriedade
  protected dates_loading: boolean = false;
  private readonly MAX_FUTURE_YEARS = 1;
  protected readonly defaultMaxDate: Date = (() => {
    let date = new Date();
    date.setFullYear(date.getFullYear() + this.MAX_FUTURE_YEARS);
    return date;
  })();
  protected maxDate: Date = this.defaultMaxDate;
  protected checkIn: Date | undefined;
  protected checkOut: Date | undefined;
  protected blockedDates: Date[] = [];
  protected discounts: ReturnedDiscount[] = [];
  protected bookPropertyForm: FormGroup;
  protected pricesMap: Map<number, number> = new Map();

  constructor() {
    this.bookPropertyForm = this.formBuilder.group({
      checkIn: [Validators.required],
      checkOut: [Validators.required]
    });
  }

  ngOnInit(): void {
    this.global_loading = true;
    this.authService.user().forEach(async user => {
      this.user = user;
    }).then(() => {
      this.loadGroups().then(() => {
        this.global_loading = false;
        this.initWebSocket();
      });
    }).catch(error => {
      console.error('Erro ao carregar utilizador:', error);
      this.global_loading = false;
    });
  }

  ngOnDestroy() {
    this.ws?.close(3000, "Página fechada");
  }

  private async loadGroups() {
    if (this.user) {
      return this.groupService.getGroupsByUserId(this.user.userId).forEach(groups => {
        this.group_list = groups;
      }).then(async () => {
        let queryGroupId = this.route.snapshot.queryParams['groupId'];
        if (queryGroupId) {
          let group = this.group_list.find(g => g.groupId == queryGroupId);
          if (group) {
            await this.chooseGroup(group.groupId);
          } else {
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
              modalRef.componentInstance.submitting_invite = this.invite_loading;
              modalRef.componentInstance.onAccept = async () => {
                await this.acceptInvite();
                modalRef.close();
              }
              modalRef.componentInstance.onClose = async () => {
                modalRef.close();
                await this.router.navigate(['/groups']);
              }
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

  private initWebSocket() {
    this.ws_loading = true;
    let url = environment.apiUrl;
    url = url.replace('https', 'wss');
    let uuid = uuidv4();
    let fullUrl = `${url}/api/groups/ws?socketId=${uuid}&userId=${this.user?.userId}`
    this.ws = new WebSocket(fullUrl);
    this.ws_uuid = uuid;
    console.log(`[WebSocket] A estabelecer conexão...`);
    this.ws.onopen = () => {
      this.ws_loading = false;
      console.log(`[WebSocket] Conexão estabelecida.`);
    }
    this.ws.onerror = (ev) => {
      this.ws_loading = false;
      console.error(`[WebSocket] Erro de conexão: ${ev}`);
    }
    this.ws.onmessage = (event) => {
      let message = JSON.parse(event.data) as WebsocketMessage;
      switch (message.code) {
        case 'PropertyRemoved': {
          let content = JSON.parse(message.content) as { groupId: string, propertyId: string }
          if (this.currentGroup?.groupId == content?.groupId) {
            this.currentGroup!.properties = this.currentGroup!.properties.filter(p => p.propertyId != content?.propertyId);
          }
          break;
        }
        case 'PropertyAdded': {
          let content = JSON.parse(message.content) as { groupId: string, property: GroupProperty };
          if (this.currentGroup?.groupId == content.groupId) {
            if (!this.currentGroup?.properties.find(p => p.propertyId == content.property.propertyId)) {
              this.currentGroup!.properties.push(content.property);
            }
          }
          break;
        }
        case 'MemberAdded': {
          let content = JSON.parse(message.content) as { groupId: string, member: GroupMember };
          if (this.currentGroup?.groupId == content.groupId) {
            if (!this.currentGroup?.members.find(m => m.id == content.member.id)) {
              this.currentGroup!.members.push(content.member);
            }
          }
          break;
        }
        case 'GroupDeleted': {
          let content = JSON.parse(message.content) as { groupId: string };
          if (this.group_list.find(g => g.groupId == content.groupId)) {
            if (this.currentGroup?.groupId == content.groupId) {
              this.currentGroup = undefined;
              this.router.navigate(['/groups']);
            }
            this.group_list = this.group_list.filter(g => g.groupId != content.groupId);
          }
          break;
        }
        case 'GroupActionUpdated': {
          let content = JSON.parse(message.content) as { groupId: string, groupAction: string };
          if (this.currentGroup?.groupId == content.groupId) {
            this.currentGroup!.groupAction = content.groupAction;
          }
          break;
        }
        case 'ChosenPropertyUpdated': {
          let content = JSON.parse(message.content) as { groupId: string, property: GroupProperty };
          if (this.currentGroup?.groupId == content.groupId) {
            this.currentGroup!.chosenProperty = content.property;
          }
          break;
        }
        case 'UserVoteAdded': {
          let content = JSON.parse(message.content) as {
            groupId: string,
            vote: { userId: string, propertyId: string }
          };
          if (this.currentGroup?.groupId == content.groupId) {
            if (!this.currentGroup?.votes.find(v => v.userId == content.vote.userId)) {
              this.currentGroup!.votes.push(content.vote);
            }
          }
          break;
        }
        case 'UserVoteUpdated' : {
          let content = JSON.parse(message.content) as {
            groupId: string,
            vote: { userId: string, propertyId: string }
          };
          if (this.currentGroup?.groupId == content.groupId) {
            this.currentGroup!.votes = this.currentGroup!.votes.map(v => {
              if (v.userId == content.vote.userId) {
                return {userId: content.vote.userId, propertyId: content.vote.propertyId}
              }
              return v;
            });
          }
          break;
        }
        case 'UserVoteRemoved': {
          let content = JSON.parse(message.content) as {
            groupId: string,
            vote: { userId: string, propertyId: string }
          };
          if (this.currentGroup?.groupId == content.groupId) {
            this.currentGroup!.votes = this.currentGroup!.votes.filter(v => v.userId != content.vote.userId);
          }
          break;
        }
      }
    };
    this.ws.onclose = (event) => {
      if (event.code === 3000) {
        console.log(`[WebSocket] Conexão terminada.`);
      } else {
        this.ws_loading = true;
        console.log(`[WebSocket] Conexão perdida. À espera de reconexão...`);
        setTimeout(async () => {
          this.initWebSocket();
          this.ws?.addEventListener('open', async () => {
            if (this.currentGroup) {
              await this.chooseGroup(this.currentGroup.groupId);
            }
          });
        }, 1000);
      }
    }
  }

  private async acceptInvite() {
    this.invite_loading = true;
    return this.groupService.addMemberToGroup(this.pendingGroup!.groupId).forEach(response => {
      if (response) {
        this.invite_loading = false;
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

  protected async copyGroupLink() {
    let url = window.location.href;
    await navigator.clipboard.writeText(url);
  }

  protected updateGroupAction(action: GroupAction): void {
    if (!this.currentGroup) {
      return;
    }
    let actionString = GroupActionHelper.AsString(action);
    this.groupService.updateGroupAction(this.currentGroup.groupId, actionString, this.ws_uuid).forEach(response => {
      if (response) {
        this.currentGroup!.groupAction = actionString;
      }
    }).catch(error => {
      console.error('Erro ao atualizar ação do grupo:', error);
    });
  }

  protected deleteGroup() {
    this.groupService.deleteGroup(this.currentGroup!.groupId, this.ws_uuid).forEach(async response => {
      if (response) {
        this.group_list = this.group_list.filter(g => g.groupId != this.currentGroup?.groupId);
        this.currentGroup = undefined;
        await this.router.navigate(['/groups']);
      }
    }).catch(error => {
      console.error('Erro ao apagar grupo:', error);
    });
  }

  protected removeProperty(propertyId: string) {
    this.groupService.removePropertyFromGroup(this.currentGroup?.groupId!, propertyId, this.ws_uuid).forEach(response => {
      if (response) {
        this.currentGroup!.properties = this.currentGroup!.properties.filter(p => p.propertyId != propertyId);
      }
    }).catch(error => {
        console.log("Erro ao remover propriedade: " + JSON.stringify(error));
      }
    );
  }

  protected async chooseGroup(groupId: string) {
    await this.router.navigate([], {queryParams: {groupId: groupId}});
    return this.groupService.getGroup(groupId).forEach(group => {
      this.currentGroup = group;
      this.group_list = this.group_list.map(g => {
        if (g.groupId == group.groupId) {
          return group;
        } else {
          return g;
        }
      });
      if (GroupActionHelper.parseGroupAction(group.groupAction) === GroupAction.booking && group.groupOwner.id == this.user?.userId) {
        this.loadBlockedDates();
        this.loadDiscounts();
      }
    }).catch(error => {
      console.error('Erro ao carregar grupo:', error);
    });
  }

  protected async chooseProperty(property: GroupProperty) {
    if (!this.currentGroup) {
      return;
    }
    await this.groupService.updateChosenProperty(this.currentGroup.groupId!, property.propertyId, this.ws_uuid).forEach(response => {
      if (response) {
        this.currentGroup!.chosenProperty = property;
      }
    }).catch(error => {
      console.error('Erro ao escolher propriedade:', error);
    });
  }

  protected hasVotedInProperty(property: GroupProperty) {
    if (!this.currentGroup) {
      return false;
    } else {
      return this.currentGroup.votes.find(v => v.propertyId == property.propertyId && v.userId == this.user?.userId) != undefined;
    }
  }

  protected async voteForProperty(propertyId: string) {
    if (!this.currentGroup) {
      return;
    }
    if (this.currentGroup.votes.find(v => v.userId == this.user?.userId) === undefined) {
      await this.groupService.addPropertyVote(this.currentGroup.groupId!, propertyId, this.ws_uuid).forEach(response => {
        if (response) {
          this.currentGroup!.votes.push({userId: this.user?.userId!, propertyId: propertyId});
        }
      }).catch(error => {
        console.error('Erro ao votar em propriedade:', error);
      });
    } else {
      await this.groupService.updatePropertyVote(this.currentGroup.groupId!, propertyId, this.ws_uuid).forEach(response => {
        if (response) {
          this.currentGroup!.votes = this.currentGroup!.votes.map(v => {
            if (v.userId == this.user?.userId) {
              return {userId: this.user?.userId, propertyId: propertyId}
            }
            return v;
          });
        }
      });
    }
  }

  protected async removeVote(propertyId: string) {
    if (!this.currentGroup || !propertyId) return;
    await this.groupService.removePropertyVote(this.currentGroup.groupId!, propertyId, this.ws_uuid).forEach(response => {
      if (response) {
        this.currentGroup!.votes = this.currentGroup!.votes.filter(v => v.userId != this.user?.userId);
      }
    });
  }

  protected startVoting() {
    if (!this.currentGroup) {
      return;
    }
    let modalRef = this.modalService.open(StartVoteModal, {
      animation: true,
      centered: true
    });
    modalRef.componentInstance.onAccept = () => {
      this.updateGroupAction(GroupAction.voting);
      modalRef.close();
    }
  }

  protected concludeVoting() {
    if (!this.currentGroup) {
      return;
    }

    let propertyVotes =
      this.currentGroup.properties
        .map(p => {
          return {
            property: p,
            votes: this.currentGroup!.votes.filter(v => v.propertyId == p.propertyId).length
          }
        })
        .sort((a, b) => b.votes - a.votes)
        .filter((p, _, arr) => p.votes == arr[0].votes);
    if (propertyVotes[0].votes == 0) {
      let modalRef = this.modalService.open(ConcludeVoteFailedModal, {
        animation: true,
        centered: true
      });
      modalRef.componentInstance.reason = 'Nenhuma propriedade foi votada.';
      modalRef.componentInstance.onClose = () => {
        modalRef.close();
      }
    } else {
      if (propertyVotes.length == 1) {
        let modalRef = this.modalService.open(ConcludeVoteModal, {
          animation: true,
          centered: true
        });
        modalRef.componentInstance.allMemberVoted = this.currentGroup.members.length == this.currentGroup.votes.length;
        modalRef.componentInstance.onAccept = () => {
          // this.updateGroupAction(GroupAction.booking);
          this.chooseProperty(propertyVotes[0].property).then(() => {
            modalRef.close();
          });
        }
      } else {
        let modalRef = this.modalService.open(ConcludeVoteFailedModal, {
          animation: true,
          centered: true
        });
        modalRef.componentInstance.reason = 'Houve um empate na votação.';
        modalRef.componentInstance.onClose = () => {
          modalRef.close();
        }
      }
    }
  }

  protected numberOfPropertyVotes(propertyId: string): number {
    return this.currentGroup?.votes.filter(v => v.propertyId == propertyId).length || 0;
  }

  protected isPropertyVotedByUser(propertyId: string): boolean {
    return this.currentGroup?.votes.find(v => v.userId == this.user?.userId && v.propertyId == propertyId) != undefined;
  }

  protected isPropertyChosen(propertyId: string): boolean {
    return this.currentGroup?.chosenProperty?.propertyId == propertyId;
  }

  protected startBooking() {
    if (!this.currentGroup) {
      return;
    }
    this.updateGroupAction(GroupAction.booking);
  }

  protected filterDates: DateFilterFn<any> = (date: Date | null) => {
    if (!date) return false;
    let today = new Date();
    let dateIntervalValid: boolean = (() => {
      let tomorrow = new Date(date);
      tomorrow.setDate(tomorrow.getDate() + 1);
      let previousDate = new Date(date);
      previousDate.setDate(previousDate.getDate() - 1);
      return !((this.isSameDay(today, previousDate) && this.blockedDates.some(blockedDate => this.isSameDay(tomorrow, blockedDate)))
        || (this.blockedDates.some(blockedDate => this.isSameDay(previousDate, blockedDate)) && this.blockedDates.some(blockedDate => this.isSameDay(tomorrow, blockedDate))));
    })();
    return date >= today && !this.blockedDates.some(blockedDate => this.isSameDay(date, blockedDate)) && dateIntervalValid;
  }

  protected isSameDay = (d1: Date, d2: Date): boolean => {
    return d1.getDate() === d2.getDate() &&
      d1.getMonth() === d2.getMonth() &&
      d1.getFullYear() === d2.getFullYear();
  }

  protected onDateChange($event: MatDatepickerInputEvent<Date>, type: 'checkIn' | 'checkOut') {
    if ($event.value) {
      if (type == 'checkIn') {
        this.checkIn = $event.value;
        this.pricesMap.clear();
        this.maxDate = (() => {
          if (this.checkIn) {
            const nextBlockedDate = this.blockedDates.find(date => date > this.checkIn!);
            if (nextBlockedDate) {
              return this.maxDate = nextBlockedDate;
            } else {
              return this.maxDate = this.defaultMaxDate;
            }
          } else {
            return this.maxDate = this.defaultMaxDate;
          }
        })();
      } else {
        this.checkOut = $event.value;
        console.log(this.checkOut);
      }
    }
  }

  protected clearDates() {
    this.checkIn = undefined;
    this.checkOut = undefined;
    this.bookPropertyForm.reset();
    this.maxDate = this.defaultMaxDate;
    this.pricesMap.clear()
  }

  protected async loadBlockedDates() {
    if (this.currentGroup?.chosenProperty) {
      await this.propertyService.getPropertyBlockedDates(this.currentGroup.chosenProperty.propertyId).forEach((blockedDates) => {
        blockedDates.forEach(bd => {
          const startDate = new Date(bd.start);
          const endDate = new Date(bd.end);
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

  protected async loadDiscounts() {
    if (this.currentGroup?.chosenProperty) {
      await this.propertyService.getPropertyDiscounts(this.currentGroup.chosenProperty.propertyId).forEach((discounts) => {
        discounts.forEach(discount => {
          this.discounts.push({
            discountId: discount.discountId,
            propertyId: discount.propertyId,
            discountAmount: discount.discountAmount,
            startDate: new Date(discount.startDate),
            endDate: new Date(discount.endDate)
          });
        });
      }).catch(error => {
          console.error('Erro ao carregar descontos:', error);
        }
      );
    }
  }

  protected discountClass: MatCalendarCellClassFunction<Date> = (cellDate, view) => {
    return this.discountDates.some(date => this.isSameDay(date, cellDate)) ? 'discount-date-class' : '';
  };

  protected formatPrice(entry: KeyValue<number,number>): string {
    return `${entry.key}€ x ${entry.value} noites - ${Math.round(((entry.key * entry.value) + Number.EPSILON) * 100) / 100}€`
  }

  protected totalDiscount() {
    if (this.currentGroup?.chosenProperty && this.checkIn && this.checkOut) {
      const pricePerNight = this.currentGroup?.chosenProperty?.pricePerNight;
      const selectedDates: Date[] = [];
      this.pricesMap = new Map();
      if (this.currentGroup?.chosenProperty && this.checkIn && this.checkOut) {
        const currentDate = new Date(this.checkIn);
        while (currentDate < this.checkOut) {
          selectedDates.push(new Date(currentDate));
          currentDate.setDate(currentDate.getDate() + 1);
        }

        selectedDates.forEach(selectedDate => {
          const matchingDiscounts = this.discounts.filter(discount =>
            selectedDate >= discount.startDate && selectedDate <= discount.endDate
          );

          if (matchingDiscounts.length > 0) {
            matchingDiscounts.forEach(discount => {
              const newPrice = (() => {
                const discountMultiplier = 1 - discount.discountAmount / 100;
                return Math.round(((pricePerNight * discountMultiplier) + Number.EPSILON) * 100) / 100
              })()
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
    }
    return 0;
  }

  // Propriedades auxiliares

  protected get isActionNone(): boolean {
    return this.currentGroup?.groupAction == GroupActionHelper.AsString(GroupAction.none);
  }

  protected get isActionVoting(): boolean {
    return this.currentGroup?.groupAction == GroupActionHelper.AsString(GroupAction.voting);
  }

  protected get isActionBooking(): boolean {
    return this.currentGroup?.groupAction == GroupActionHelper.AsString(GroupAction.booking);
  }

  protected get isActionPaying(): boolean {
    return this.currentGroup?.groupAction == GroupActionHelper.AsString(GroupAction.paying);
  }

  protected get isOwner(): boolean {
    return this.currentGroup?.groupOwner.id == this.user?.userId;
  }

  protected get hasChosenProperty(): boolean {
    return this.currentGroup?.chosenProperty != undefined;
  }

  protected get chosenProperty(): GroupProperty | undefined {
    return this.currentGroup?.chosenProperty ?? undefined;
  }

  protected get numberOfMembers(): number {
    return this.currentGroup?.members.length ?? 0;
  }

  protected get discountDates(): Date[] {
    let dates: Date[] = [];
    this.discounts.forEach(discount => {
      let startDate = new Date(discount.startDate);
      let endDate = new Date(discount.endDate);
      while (startDate <= endDate) {
        dates.push(new Date(startDate));
        startDate.setDate(startDate.getDate() + 1);
      }
    })
    return dates;
  }

  protected set isActionNone(value: boolean) {
    if (value) {
      this.updateGroupAction(GroupAction.none);
    }
  }

  protected set isActionVoting(value: boolean) {
    if (value) {
      this.updateGroupAction(GroupAction.voting);
    }
  }

  protected set isActionBooking(value: boolean) {
    if (value) {
      this.updateGroupAction(GroupAction.booking);
    }
  }

  protected set isActionPaying(value: boolean) {
    if (value) {
      this.updateGroupAction(GroupAction.paying);
    }
  }

  // Modals

  protected showAddPropertyModal() {
    let modalRef = this.modalService.open(AddPropertyModal,
      {
        animation: true,
        size: 'lg',
        centered: true,
      });
    modalRef.componentInstance.onAccept = async () => {
      modalRef.close();
      await this.router.navigate(['/']);
    }
  }

  protected showDeleteGroupModal() {
    let modalRef = this.modalService.open(DeleteGroupModal,
      {
        animation: true,
        centered: true,
      });
    modalRef.componentInstance.onAccept = async () => {
      this.deleteGroup();
      modalRef.close();
    }
  }
}

// Componentes auxiliares

@Component({
  standalone: true,
  selector: 'accept-invite-modal',
  template: `
    <div class="modal-header">
      <h5 class="modal-title" id="acceptInviteLabel">Convite para grupo de reserva</h5>
      <button *ngIf="failed_invite && !submitting_invite" type="button" class="btn-close" aria-label="Close"
              (click)="onClose()"></button>
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
              (click)="onClose()">
        Rejeitar
      </button>
      <button *ngIf="!failed_invite" type="button" class="btn btn-success"
              (click)="onAccept()">
        Aceitar
      </button>
      <button *ngIf="failed_invite" type="button" class="btn btn-danger" data-bs-dismiss="modal"
              (click)="onClose()">
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
  private activeModal: NgbActiveModal = inject(NgbActiveModal);
  protected failed_invite: boolean = false;
  protected submitting_invite: boolean = false;
  protected pendingGroup: Group | undefined;
  protected onAccept: Function = () => {
    this.activeModal.close();
  }
  protected onClose: Function = () => {
    this.activeModal.dismiss();
  }
}

@Component({
  standalone: true,
  selector: 'add-property-modal',
  template: `
    <div class="modal-header" aria-labelledby="acceptInviteLabel">
      <h5 class="modal-title" id="acceptInviteLabel">Adicionar Propriedade</h5>
      <button type="button" class="btn-close" aria-label="Close" (click)="onClose()"></button>
    </div>
    <div class="modal-body">
      <p class="text-muted">Leia cuidadosamente os seguintes procedimentos.</p>
      <p>Para adicionar uma propriedade ao seu grupo de reserva deve:</p>
      <ol class="list-group list-group-numbered list-group-flush mb-2">
        <li class="list-group-item"><strong>Ir para a Página Inicial:</strong> Clique em "Aceitar" para começar a
          explorar.
        </li>
        <li class="list-group-item"><strong>(Opcional) Pesquisar e Filtrar:</strong> Encontre uma propriedade que se
          adeque às suas necessidades.
        </li>
        <li class="list-group-item"><strong>Adicionar ao Grupo:</strong> Selecione uma propriedade e clique em
          "Adicionar ao Grupo de Reserva".
        </li>
        <li class="list-group-item"><strong>Selecionar Grupo:</strong> Escolha o grupo de reserva onde deseja
          incluir a propriedade.
        </li>
        <li class="list-group-item"><strong>Concluir:</strong> A propriedade será automaticamente adicionada ao grupo
          selecionado.
        </li>
      </ol>
      <p class="mt-2"><strong>NOTA:</strong> Para cada grupo de reserva existe um máximo de 6 propriedades a escolher.
      </p>
    </div>
    <div class="modal-footer">
      <button type="button" class="btn btn-secondary" (click)="onClose()">Cancelar</button>
      <button type="button" class="btn btn-success" (click)="onAccept()">Aceitar</button>
    </div>
  `,

  imports: [
    RouterLink,
    AuxiliaryModule,
    NgIf
  ]
})
export class AddPropertyModal {
  private activeModal: NgbActiveModal = inject(NgbActiveModal);
  protected onClose: Function = () => {
    this.activeModal.dismiss();
  }

  protected onAccept: Function = () => {
    this.activeModal.close();
  }
}

@Component({
  selector: 'delete-group-modal',
  standalone: true,
  template: `
    <div class="modal-header" aria-labelledby="deleteGroupModalLabel">
      <h5 class="modal-title" id="deleteGroupModalLabel">Apagar Grupo</h5>
      <button type="button" class="btn-close" aria-label="Close" (click)="onClose()"></button>
    </div>
    <div class="modal-body">
      <p>Tem certeza de que deseja apagar <strong>{{ group?.name }}</strong>?</p>
      <p>Esta ação é <strong>irreversível</strong> e resultará na exclusão do grupo para todos os membros. Todos os
        dados associados serão perdidos.</p>
    </div>
    <div class="modal-footer">
      <button type="button" class="btn btn-secondary" aria-label="Cancel" (click)="onClose()">Cancelar</button>
      <button type="button" class="btn btn-danger" aria-label="Delete" (click)="onAccept()">Apagar</button>
    </div>
  `,

  imports: [
    RouterLink,
    AuxiliaryModule,
    NgIf
  ]
})
export class DeleteGroupModal {
  private activeModal: NgbActiveModal = inject(NgbActiveModal);
  protected group: Group | undefined;
  protected onClose: Function = () => {
    this.activeModal.dismiss();
  }
  protected onAccept: Function = () => {
    this.activeModal.close();
  }
}

@Component({
  selector: 'start-vote-modal',
  standalone: true,
  template: `
    <div class="modal-header" aria-labelledby="concludeVoteFailedModalLabel">
      <h5 class="modal-title" id="concludeVoteFailedModalLabel">Iniciar votação</h5>
      <button type="button" class="btn-close" aria-label="Close" (click)="onClose()"></button>
    </div>
    <div class="modal-body">
      <p>Tem certeza de que deseja iniciar a votação?</p>
      <span><strong>Após iniciar a votação não será possível adicionar mais membros ou propriedades ao grupo.</strong></span>
    </div>
    <div class="modal-footer">
      <button type="button" class="btn btn-secondary" aria-label="Cancel" (click)="onClose()">Cancelar</button>
      <button type="button" class="btn btn-success" aria-label="Start" (click)="onAccept()">Iniciar</button>
    </div>
  `,

  imports: [
    RouterLink,
    AuxiliaryModule,
    NgIf
  ]
})
export class StartVoteModal {
  private activeModal: NgbActiveModal = inject(NgbActiveModal);
  protected onAccept: Function = () => {
    this.activeModal.close();
  }
  protected onClose: Function = () => {
    this.activeModal.dismiss();
  }
}

@Component({
  selector: 'conclude-vote-vote-modal',
  standalone: true,
  template: `
    <div class="modal-header" aria-labelledby="concludeVoteFailedModalLabel">
      <h5 class="modal-title" id="concludeVoteFailedModalLabel">Concluir votação</h5>
      <button type="button" class="btn-close" aria-label="Close" (click)="onClose()"></button>
    </div>
    <div class="modal-body">
      <p *ngIf="!allMemberVoted"><strong>Ainda existem membros que não votaram.</strong></p>
      <span>Tem a certeza de que deseja concluir a votação?</span>
    </div>
    <div class="modal-footer">
      <button type="button" class="btn btn-secondary" aria-label="Cancel" (click)="onClose()">Cancelar</button>
      <button type="button" class="btn btn-success" aria-label="Conclude" (click)="onAccept()">Concluir</button>
    </div>
  `,

  imports: [
    RouterLink,
    AuxiliaryModule,
    NgIf
  ]
})
export class ConcludeVoteModal {
  private activeModal: NgbActiveModal = inject(NgbActiveModal);
  protected allMemberVoted: boolean = false;
  protected onAccept: Function = () => {
    this.activeModal.close();
  }
  protected onClose: Function = () => {
    this.activeModal.dismiss();
  }
}

@Component({
  selector: 'conclude-vote-failed-modal',
  standalone: true,
  template: `
    <div class="modal-header" aria-labelledby="concludeVoteFailedModalLabel">
      <h5 class="modal-title" id="concludeVoteFailedModalLabel">Não foi possível concluir a votação</h5>
      <button type="button" class="btn-close" aria-label="Close" (click)="onClose()"></button>
    </div>
    <div class="modal-body">
      <span *ngIf="reason">{{ reason }}</span>
    </div>
    <div class="modal-footer">
      <button type="button" class="btn btn-danger" aria-label="Cancel" (click)="onClose()">Fechar</button>
    </div>
  `,

  imports: [
    RouterLink,
    AuxiliaryModule,
    NgIf
  ]
})
export class ConcludeVoteFailedModal {
  private activeModal: NgbActiveModal = inject(NgbActiveModal);
  protected reason: string = '';
  protected onClose: Function = () => {
    this.activeModal.dismiss();
  }
}
