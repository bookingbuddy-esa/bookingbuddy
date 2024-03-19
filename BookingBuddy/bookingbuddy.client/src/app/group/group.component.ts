import { Component, ElementRef, QueryList, ViewChildren } from '@angular/core';
import {environment} from "../../environments/environment";
import { ActivatedRoute, Router } from '@angular/router';
import { GroupService } from './group.service';
import { timeout } from 'rxjs';
import { AuthorizeService } from '../auth/authorize.service';
import { UserInfo } from '../auth/authorize.dto';
import { Group } from '../models/group';
import { Property } from '../models/property';

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

  constructor(private authService: AuthorizeService, private route: ActivatedRoute, private router: Router, private groupService: GroupService) {
  }

  ngOnInit(): void {
      this.authService.user().forEach(async user => {
        this.user = user;
        this.setupGroupById().then(() => {
          this.loadUserGroups();
        });
      });
  }

  public copyGroupLink(): void {
    let url = window.location.href;
    navigator.clipboard.writeText(url);
  }

  public deleteGroup(): void {
    this.groupService.deleteGroup(this.currentGroup!.groupId).forEach(response => {
      if (response) {
        this.success_alerts.push("Grupo removido com sucesso!");
        this.loadUserGroups();
        this.currentGroup = undefined;
        this.router.navigateByUrl('/group');
      }
    }).catch(error => {
      this.errors.push(error.error);
    });
  }

  public testSendWS(): void {
    if(this.ws){
      //this.currentGroup!.name = "Teste Novo";
      //this.currentGroup!.membersId.push("5f3e3e3e3e3e3e3e3e3e3e3e");
      this.ws.send(JSON.stringify(this.currentGroup));
      console.log("Sending to ws: " + JSON.stringify(this.currentGroup));
    }
  }

  public sendMessage(): void {
    let message = {
      userName: this.user!.name,
      message: this.newMessage.trim()
    };

    // TODO: enviar a mensagem para a base de dados com o service
    this.groupService.sendGroupMessage(this.currentGroup!.groupId, message.message).forEach(response => { 
      if (response) {
      console.log(response);
    }
  })

    this.currentGroup!.messages.push(message);
    this.newMessage = '';
    this.testSendWS();
    console.log("mensagem enviada");

  }
  
  public chooseGroup(group: Group): void {
    //console.log("Escolher este grupo: " + JSON.stringify(group));
    this.errors = [];
    this.success_alerts = [];
    this.router.navigate([], { queryParams: { groupId: group.groupId }});
    this.currentGroup = group;
    this.isGroupOwner = this.currentGroup.groupOwnerId == this.user?.userId;

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

  private loadUserGroups() {
    this.groupService.getGroupsByUserId(this.user!.userId).pipe(timeout(10000)).forEach(groups => {
      console.log("Grupos Recebidos deste User: " + JSON.stringify(groups));
      this.group_list = groups;
      this.submitting = false;
    }).catch(error => {
      this.errors.push(error.error);
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
              //console.log("Grupo: " + JSON.stringify(getGroupResponse));
              const group: Group = getGroupResponse as Group;
              let isMemberOfGroup = group.membersId.includes(this.user!.userId);
              if (!isMemberOfGroup) {
                this.groupService.addMemberToGroup(params['groupId']).forEach(addMemberResponse => {
                  if (addMemberResponse) {
                    //console.log("Membro adicionado ao grupo: " + JSON.stringify(response));
                    //console.log("Grupo: " + JSON.stringify(getGroupResponse));
                    console.log("AddMemberResponse: " + JSON.stringify(addMemberResponse));
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
}
