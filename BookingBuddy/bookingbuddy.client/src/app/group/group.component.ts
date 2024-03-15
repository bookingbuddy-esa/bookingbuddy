import { Component } from '@angular/core';
import {environment} from "../../environments/environment";
import { ActivatedRoute, Router } from '@angular/router';
import { GroupService } from './group.service';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrl: './group.component.css'
})
export class GroupComponent {
  //group_list: [] = [];

  /*{ id: "sIc92wa4", members: ["eduardo", "pedro"], lastMessage: { member: "eduardo", message: "olá, espero que esteja tudo bem com vocês"} },
    { id: "M5zklaw0", members: ["joao", "andre", "diogo"], lastMessage: { member: "diogo", message: "vamos fazer a reserva então?"} },*/
    
    group_list: { id: string; members: string[]; lastMessage: { member: string; message: string; }; }[] = [];
    ws: WebSocket | undefined;

    constructor(private route: ActivatedRoute, private router: Router, private groupService : GroupService) {
    }

    ngOnInit(): void {
        for (let i = 0; i < 10; i++) {
            let members = Array.from({length: Math.floor(Math.random() * 5) + 1}, () => Math.random().toString(36).substring(2, 9));
            let group = { 
                id: Math.random().toString(36).substring(2, 9), 
                members: members, 
                lastMessage: { 
                    member: "user1", 
                    message: "Mensagem de placeholder não liguem a isto..."
                } 
            };
            this.group_list.push(group);
        }

        this.route.queryParams.forEach(params => {
          // Aqui faz o load do grupo
          console.log("Query params: " + params['groupId']);
          
          this.groupService.getGroup(params['groupId']).forEach(response => {
            if (response) {
              console.log("existe um grupo na db com este id");
            }
          }).catch(error => {
            console.log("Erro ao receber grupo: " + error);
          });


          /*let url = environment.apiUrl;
          url = url.replace('https', 'wss');

          if (this.ws) {
            this.ws.close();
          }

          let ws = new WebSocket(`${url}/api/groups/ws?groupId=${this.payment.paymentId}`);
            ws.onmessage = (event) => {
          }*/
        });
    }
  
  public chooseGroup(group: any): void {
    console.log("Escolher este grupo: " + group);
    this.router.navigate([], { 
      queryParams: {
        groupId: group.id
      }
    });
  }
}
