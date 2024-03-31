import {
  AfterViewChecked, AfterViewInit,
  Component,
  ElementRef, HostListener,
  inject,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
  ViewChild
} from '@angular/core';
import {Chat, Message} from "../models/chat";
import {ChatNewService} from "./chat-new.service";
import {environment} from "../../environments/environment";
import {v4 as uuidv4} from "uuid";
import {WebSocketMessage} from "../models/web-socket-message";
import {UserInfo} from "../auth/authorize.dto";
import {FormControl, Validators} from "@angular/forms";
import {DatePipe} from "@angular/common";

@Component({
  selector: 'app-chat-new',
  templateUrl: './chat-new.component.html',
  styleUrl: './chat-new.component.css'
})
export class ChatNewComponent implements OnInit, AfterViewChecked, OnChanges {
  private chatService: ChatNewService = inject(ChatNewService);
  private datePipe: DatePipe = inject(DatePipe);
  @Input() chatId: string | undefined;
  @Input() user: UserInfo | undefined;
  @Input() chatTitle: string = 'Chat';
  @ViewChild('messages') messages: ElementRef | undefined;
  protected chat: Chat | undefined;
  protected loading: boolean = true;
  private scrollToBottom: boolean = false;
  protected newMessages: boolean = false;
  protected messageControl: FormControl<string | null> = new FormControl<string>('', [Validators.required]);
  protected ws_loading: boolean = false;
  protected message_loading: boolean = false;
  private noMoreMessages: boolean = false;
  private ws: WebSocket | undefined;

  ngOnInit(): void {
    if (this.chatId && this.user) {
      this.updateChat(this.chatId);
    }
  }

  private initWebSocket() {
    this.ws_loading = true;
    let url = environment.apiUrl;
    url = url.replace('https', 'wss');
    let uuid = uuidv4();
    let fullUrl = `${url}/api/chat/ws?chatId=${this.chatId}`
    this.ws = new WebSocket(fullUrl);
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
      let message = JSON.parse(event.data) as WebSocketMessage;
      switch (message.code) {
        case 'UserMessage': {
          const msg = message.content as Message;
          this.chat?.lastMessages.push(msg);
          if (msg.user.id === this.user?.userId || this.isScrollAtBottom) {
            this.scrollToBottom = true;
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
            if (this.chatId) {
              this.updateChat(this.chatId);
            }
          });
        }, 1000);
      }
    }
  }

  ngAfterViewChecked() {
    if (this.messages) {
      if (this.scrollToBottom) {
        this.messages.nativeElement.scroll(0, this.messages.nativeElement.scrollHeight)
        this.scrollToBottom = false;
      }
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    for (const propName in changes) {
      switch (propName) {
        case'chatId' : {
          this.loading = true;
          this.chat = undefined;
          this.updateChat(changes['chatId'].currentValue);
          this.initWebSocket();
          break;
        }
      }
    }
  }

  protected updateChat(chatId: string) {
    this.chatId = chatId;
    if (this.chatId) {
      this.chatService.getChat(this.chatId).forEach(chat => {
        this.chat = chat;
        chat.lastMessages.reverse();
        this.loading = false;
        this.scrollToBottom = true;
      }).catch(error => {
        console.error(error);
      });
    }
  }

  protected sendMessage() {
    if (this.messageControl.value && this.ws) {
      this.ws.send(JSON.stringify({code: 'UserMessage', content: this.messageControl.value} as WebSocketMessage));
      this.messageControl.reset();
    }
  }

  protected get isScrollAtBottom(): boolean {
    if (this.messages) {
      console.log(this.messages.nativeElement.scrollHeight, this.messages.nativeElement.clientHeight, this.messages.nativeElement.scrollTop);
      return Math.abs(this.messages.nativeElement.scrollHeight - this.messages.nativeElement.clientHeight - this.messages.nativeElement.scrollTop) <= 1;
    }
    return false;
  }

  protected formatMessageDate(sentAt: Date) {
    sentAt = new Date(sentAt);
    const today = new Date();
    const yesterday = new Date();
    yesterday.setDate(today.getDate() - 1);
    const compareDates = (date1: Date, date2: Date) => {
      return date1.getDate() === date2.getDate() && date1.getMonth() === date2.getMonth() && date1.getFullYear() === date2.getFullYear();
    }
    if (compareDates(sentAt, today)) {
      return `Hoje às ${this.datePipe.transform(sentAt, 'HH:mm')}`;
    } else if (compareDates(sentAt, yesterday)) {
      return `Ontem às ${this.datePipe.transform(sentAt, 'HH:mm')}`;
    } else {
      return this.datePipe.transform(sentAt, 'dd/MM/yyyy HH:mm');
    }
  }

  protected async loadMoreMessages() {
    if (this.noMoreMessages || this.message_loading || !this.chatId) {
      return;
    } else {
      this.message_loading = true;
      let offset = this.chat?.lastMessages.length || 0;
      let limit = 10;
      return this.chatService.getMessages(this.chatId, offset, limit).forEach(messages => {
        messages.reverse();
        console.log(messages);
        if (messages.length === 0) {
          this.noMoreMessages = true;
        } else {
          this.chat?.lastMessages.unshift(...messages);
        }
        this.message_loading = false;
      }).catch(error => {
        console.error("Erro ao carregar mensagens: ", error);
        this.message_loading = false;
      });
    }
  }

  protected onScroll(event: Event) {
    const element = event.target as HTMLElement;
    const scrollTop = element.scrollTop;
    const distanceFromBottom = element.scrollHeight - scrollTop;
    if (scrollTop === 0) {
      this.loadMoreMessages().then(() => {
        element.scroll(0, element.scrollHeight - distanceFromBottom)
        console.log(element.scrollHeight - scrollTop, element.clientHeight, element.scrollHeight);
      });
    }
  }
}
