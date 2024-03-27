import {Component, EventEmitter, inject, Input, OnInit, Output} from '@angular/core';
import {environment} from "../../environments/environment";
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import {GroupService} from './group.service';
import {AuthorizeService} from '../auth/authorize.service';
import {UserInfo} from '../auth/authorize.dto';
import {Group, GroupAction, GroupActionHelper, GroupMember, GroupProperty} from '../models/group';
import {WebsocketMessage} from "../models/websocket-message";
import {NgbActiveModal, NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {AuxiliaryModule} from "../auxiliary/auxiliary.module";
import {NgIf} from "@angular/common";

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrl: './group.component.css'
})
export class GroupComponent implements OnInit {
  // Serviços

  private authService: AuthorizeService = inject(AuthorizeService);
  private route: ActivatedRoute = inject(ActivatedRoute);
  protected router: Router = inject(Router);
  private groupService: GroupService = inject(GroupService);
  private modalService: NgbModal = inject(NgbModal);
  protected readonly navigator = navigator;
  protected readonly GroupAction = GroupAction;

  // Carregamento de dados

  protected global_loading: boolean = false;
  protected submitting: boolean = false;
  protected submitting_invite: boolean = false;

  // Utilizador, grupos e reservas

  protected user: UserInfo | undefined;
  protected group_list: Group[] = [];
  protected currentGroup: Group | undefined;
  protected isGroupOwner: boolean = false;

  // WebSocket

  protected ws: WebSocket | undefined;
  protected wsLoading: boolean = false;

  // Convidar para grupo

  protected failed_invite: boolean = false;
  protected pendingGroup: Group | undefined;

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

  private async loadGroups() {
    if (this.user) {
      return this.groupService.getGroupsByUserId(this.user.userId).forEach(groups => {
        this.group_list = groups;
      }).then(async () => {
        let queryGroupId = this.route.snapshot.queryParams['groupId'];
        if (queryGroupId) {
          let group = this.group_list.find(g => g.groupId == queryGroupId);
          if (group) {
            await this.chooseGroup(group);
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
              modalRef.componentInstance.submitting_invite = this.submitting_invite;
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

  private async initWebSocket(group: Group) {
    this.wsLoading = true;
    let url = environment.apiUrl;
    url = url.replace('https', 'wss');
    let fullUrl = `${url}/api/groups/ws?groupId=${group.groupId}`

    if (this.ws?.readyState === WebSocket.OPEN) {
      this.ws.close(3000, 'Changing group');
    }
    this.ws = new WebSocket(fullUrl);

    this.ws.onopen = () => {
      this.wsLoading = false;
    }
    this.ws.onmessage = (event) => {
      let message = JSON.parse(event.data) as WebsocketMessage;
      switch (message.code) {
        case 'PropertyRemoved': {
          let content = JSON.parse(message.content) as { groupId: string, propertyId: string }
          if (this.currentGroup?.groupId == content?.groupId) {
            this.currentGroup!.properties = this.currentGroup!.properties.filter(p => p.propertyId != content?.propertyId);
          } else {
            let group = this.group_list.find(g => g.groupId == content?.groupId);
            if (group) {
              group.properties = group.properties.filter(p => p.propertyId != content?.propertyId);
            }
          }
          break;
        }
        case 'PropertyAdded': {
          let content = JSON.parse(message.content) as { groupId: string, property: GroupProperty };
          if (this.currentGroup?.groupId == content.groupId) {
            if (!this.currentGroup?.properties.find(p => p.propertyId == content.property.propertyId)) {
              this.currentGroup!.properties.push(content.property);
            }
          } else {
            let group = this.group_list.find(g => g.groupId == content.groupId);
            if (group) {
              if (!group.properties.find(p => p.propertyId == content.property.propertyId)) {
                group.properties.push(content.property);
              }
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
          } else {
            let group = this.group_list.find(g => g.groupId == content.groupId);
            if (group) {
              if (!group.members.find(m => m.id == content.member.id)) {
                group.members.push(content.member);
              }
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
          } else {
            let group = this.group_list.find(g => g.groupId == content.groupId);
            if (group) {
              group.groupAction = content.groupAction;
            }
          }
          break;
        }
        case 'ChosenPropertyUpdated': {
          let content = JSON.parse(message.content) as { groupId: string, property: GroupProperty };
          if (this.currentGroup?.groupId == content.groupId) {
            this.currentGroup!.chosenProperty = content.property;
          } else {
            let group = this.group_list.find(g => g.groupId == content.groupId);
            if (group) {
              group.chosenProperty = content.property;
            }
          }
          break;
        }
      }
    };
    this.ws.onclose = (event) => {
    }
  }

  private async acceptInvite() {
    this.submitting_invite = true;
    return this.groupService.addMemberToGroup(this.pendingGroup!.groupId).forEach(response => {
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

  protected async copyGroupLink() {
    let url = window.location.href;
    await navigator.clipboard.writeText(url);
  }

  protected updateGroupAction(action: GroupAction): void {
    if (!this.currentGroup) {
      return;
    }

    let actionString = GroupActionHelper.AsString(action);
    this.groupService.updateGroupAction(this.currentGroup.groupId, actionString).forEach(response => {
      if (response && !(this.ws?.readyState === WebSocket.OPEN)) {
        this.currentGroup!.groupAction = actionString;
      }
    }).catch(error => {
      console.error('Erro ao atualizar ação do grupo:', error);
    });
  }

  protected deleteGroup() {
    this.groupService.deleteGroup(this.currentGroup!.groupId).forEach(async response => {
      if (response && !(this.ws?.readyState === WebSocket.OPEN)) {
        this.currentGroup = undefined;
        this.group_list = this.group_list.filter(g => g.groupId != this.currentGroup?.groupId);
        await this.router.navigate(['/groups']);
      }
    }).catch(error => {
      console.error('Erro ao apagar grupo:', error);
    });
  }

  protected removeProperty(propertyId: string) {
    this.groupService.removePropertyFromGroup(this.currentGroup?.groupId!, propertyId).forEach(response => {
      if (response && !(this.ws?.readyState === WebSocket.OPEN)) {
        this.currentGroup!.properties = this.currentGroup!.properties.filter(p => p.propertyId != propertyId);
      }
    }).catch(error => {
        console.log("Erro ao remover propriedade: " + JSON.stringify(error));
      }
    );
  }

  protected async chooseGroup(group: Group) {
    await this.router.navigate([], {queryParams: {groupId: group.groupId}});
    this.currentGroup = group;
    this.isGroupOwner = group.groupOwner.id === this.user?.userId;
    await this.initWebSocket(group);
  }

  protected async chooseProperty(property: GroupProperty) {
    if (!this.currentGroup) {
      return;
    }
    await this.groupService.updateChosenProperty(this.currentGroup.groupId!, property.propertyId).forEach(response => {
      if (response && !(this.ws?.readyState === WebSocket.OPEN)) {
        this.currentGroup!.chosenProperty = property;
      }
    }).catch(error => {
      console.error('Erro ao escolher propriedade:', error);
    });
  }

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
    modalRef.componentInstance.group = this.currentGroup;
    modalRef.componentInstance.onAccept = async () => {
      await this.deleteGroup();
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
