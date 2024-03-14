import { Component } from '@angular/core';

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

    ngOnInit(): void {
        for (let i = 0; i < 0; i++) {
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
    }
  
  public chooseGroup(group: any): void {
    console.log("Escolher este grupo: " + group);
  }
}
