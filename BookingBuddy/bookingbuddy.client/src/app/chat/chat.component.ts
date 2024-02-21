import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  onlineUsers: string[] = [];
  userName: string = '';
  groupName: string = '';
  messageToSend: string = '';
  joined: boolean = false;
  conversation: NewMessage[] = [{
    message: 'Bem-vindo ao chat!',
    userName: 'Sistema'
  }];

  private connection: HubConnection;

  constructor() {
    this.connection = new HubConnectionBuilder()
      .withUrl('https://localhost:7213/hubs/chat') // TODO: mudar isto
      .build();

    this.connection.on("NewUser", message => this.newUser(message));
    this.connection.on("NewMessage", message => this.newMessage(message));
    this.connection.on("LeftUser", message => this.leftUser(message));
    this.connection.on("UserList", users => {
      this.chatUsers = users;
    });
  }

  ngOnInit(): void {
    this.connection.start()
      .then(_ => {
        console.log('Connection Started');
      }).catch(error => {
        return console.error(error);
      });
  }

  public join() {
    this.connection.invoke('JoinGroup', this.groupName, this.userName)
      .then(_ => {
        this.joined = true;
      });
  }

  public sendMessage() {
    const newMessage: NewMessage = {
      message: this.messageToSend,
      userName: this.userName,
      groupName: this.groupName
    };

    this.connection.invoke('SendMessage', newMessage)
      .then(_ => this.messageToSend = '');
  }

  public leave() {
    this.connection.invoke('LeaveGroup', this.groupName, this.userName)
      .then(_ => this.joined = false);

    this.conversation = [];
  }

  private newUser(message: string) {
    console.log(message);
<<<<<<< Updated upstream
=======
    const userName = this.extractUserName(message);

>>>>>>> Stashed changes
    this.conversation.push({
      userName: 'Sistema',
      message: message
    });
  }

  private newMessage(message: NewMessage) {
    this.conversation.push(message);
  }

  private leftUser(message: string) {
<<<<<<< Updated upstream
    console.log(message);
=======
    const userName = this.extractUserName(message);
    //this.chatUsers = this.chatUsers.filter(user => user !== userName);

>>>>>>> Stashed changes
    this.conversation.push({
      userName: 'Sistema',
      message: message
    });
  }

}

interface NewMessage {
  userName: string;
  message: string;
  groupName?: string;
}
