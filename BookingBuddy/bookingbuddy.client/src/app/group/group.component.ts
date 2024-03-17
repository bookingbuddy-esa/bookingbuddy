import { Component, ElementRef, QueryList, ViewChildren } from '@angular/core';
import {environment} from "../../environments/environment";
import { ActivatedRoute, Router } from '@angular/router';
import { GroupService } from './group.service';
import { timeout } from 'rxjs';
import { AuthorizeService } from '../auth/authorize.service';
import { UserInfo } from '../auth/authorize.dto';
import { Group } from '../models/group';

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

  constructor(private authService: AuthorizeService, private route: ActivatedRoute, private router: Router, private groupService: GroupService) {
  }

  ngOnInit(): void {
      this.authService.user().forEach(async user => {
        this.user = user;
        this.loadUserGroups();
      });
  }

  public vote(): void {
    this.currentGroup?.propertiesId.push(this.currentGroup?.propertiesId[0]);
    if (this.ws) {
      this.ws.send(JSON.stringify(this.currentGroup));
    }
  }
  
  public chooseGroup(group: Group): void {
    //console.log("Escolher este grupo: " + JSON.stringify(group));
    this.errors = [];
    this.success_alerts = [];
    this.router.navigate([], { queryParams: { groupId: group.groupId }});
    this.currentGroup = group;
  }

  private loadUserGroups() {
    if(this.user){
      this.submitting = true;
      this.groupService.getGroupsByUserId(this.user?.userId).pipe(timeout(10000)).forEach(groups => {
        this.group_list = groups;
        this.submitting = false;
      }).then(() => {
        this.route.queryParams.forEach(params => {
          if (params['groupId']) {
            this.groupService.getGroup(params['groupId']).forEach(response => {
              if (response) {
                let isMemberOfGroup = this.group_list.some(group => group.groupId === params['groupId']);
                if (!isMemberOfGroup) {
                  this.groupService.addMemberToGroup(params['groupId']).forEach(response => {
                    if (response) {
                      //console.log("Membro adicionado ao grupo: " + JSON.stringify(response));
                      this.success_alerts.push(response);
                      this.loadUserGroups();
                    }
                  }).catch(error => {
                    this.errors.push(error.error);
                    //console.log("Erro ao adicionar membro ao grupo: " + JSON.stringify(error));
                  });
                }
                
                let url = environment.apiUrl;
                url = url.replace('https', 'wss');

                if (this.ws) {
                  this.ws.close();
                }

                this.ws = new WebSocket(`${url}/api/groups/ws?groupId=${params['groupId']}`);
                this.ws.onmessage = (event) => {
                  console.log("Mensagem recebida: " + event.data);
                  let message = JSON.parse(event.data);
                  this.currentGroup = message;
                };
              }
            }).catch(error => {
              this.errors.push(error.error);
              //console.log("Erro ao receber grupo: " + JSON.stringify(error));
            });
          }
        });
      /*}).then(() => {
        setTimeout(() => {
          console.log("Esperando 5 segundos para carregar grupos");
          this.alertContainers.forEach(alertContainer => {
            alertContainer.nativeElement.classList.remove('show');
          });
        }, 5000);

        this.errors = [];
        this.success_alerts = [];*/
      }).catch(error => {
        console.log("Erro ao carregar grupos: " + JSON.stringify(error));
      });
    }
  }
}
