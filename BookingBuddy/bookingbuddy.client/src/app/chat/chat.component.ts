import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';


@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {

  public userName = '';
  public groupName = '';
  public messageToSend = '';
  public joined = false;
  public conversation: NewMessage[] = [{
    message: 'Bem-vindo ao chat!',
    userName: 'Sistema'
  }];

  private connection: HubConnection;

  public chatUsers: string[] = [];

  constructor() {
    this.connection = new HubConnectionBuilder()
      .withUrl('https://localhost:7213/hubs/chat')
      .build();

    this.connection.on("NewUser", message => this.newUser(message));
    this.connection.on("NewMessage", message => this.newMessage(message));
    this.connection.on("LeftUser", message => this.leftUser(message));
  }

  ngOnInit(): void {
    this.connection.start()
      .then(_ => {
        console.log('Connection Started');

        if (this.connection.state === "Connected") {
          this.connection.invoke('GetUsers', this.groupName)
            .then(users => {
              console.log('Received users:', users);
              this.chatUsers = users;
            })
            .catch(error => console.error(error));
        } else {
          console.warn('Connection is not in "Connected" state.');
        }
      })
      .catch(error => console.error(error));
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

    const userName = this.extractUserName(message);
    this.chatUsers.push(userName);

    this.conversation.push({
      userName: 'Sistema',
      message: message
    });
  }

  private newMessage(message: NewMessage) {
    console.log(message);
    this.conversation.push(message);
  }

  private leftUser(message: string) {
    console.log(message);

    const userName = this.extractUserName(message);
    this.chatUsers = this.chatUsers.filter(user => user !== userName);

    this.conversation.push({
      userName: 'Sistema',
      message: message
    });
  }

  private extractUserName(message: string): string {
    const parts = message.split(' ');
    return parts[0];
  }
}

interface NewMessage {
  userName: string;
  message: string;
  groupName?: string;
}
