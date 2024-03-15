import { Component } from '@angular/core';
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
  submitting: boolean = false;
  user: UserInfo | undefined;
  group_list: Group[] = [];
  currentGroup: Group | null = null;
  ws: WebSocket | undefined;

  constructor(private authService: AuthorizeService, private route: ActivatedRoute, private router: Router, private groupService: GroupService) {
  }

  ngOnInit(): void {
      this.authService.user().forEach(async user => {
        this.user = user;
        this.loadUserGroups();
      });

      this.route.queryParams.forEach(params => {
        // Aqui faz o load do grupo
        //console.log("Query params: " + params['groupId']);

        if (params['groupId']) {
          this.groupService.getGroup(params['groupId']).forEach(response => {
            if (response) {
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
            console.log("Erro ao receber grupo: " + error);
          });
        }
      });
  }

  public vote(): void {
    this.currentGroup?.propertiesId.push(this.currentGroup?.propertiesId[0]);
    if (this.ws) {
      this.ws.send(JSON.stringify(this.currentGroup));
    }
  }
  
  public chooseGroup(group: Group): void {
    console.log("Escolher este grupo: " + JSON.stringify(group));
    this.currentGroup = group;
    this.router.navigate([], { 
      queryParams: {
        groupId: group.groupId
      }
    });
  }

  private loadUserGroups() {
    if(this.user){
      this.submitting = true;
      this.groupService.getGroupByUserId(this.user?.userId).pipe(timeout(10000)).forEach(groups => {
        console.log(groups);
        this.group_list = groups;
        this.chooseGroup(this.group_list[0]);
        this.submitting = false;
      })
    }
  }
}
