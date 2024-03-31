import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {Chat, Message} from "../models/chat";

@Injectable({
  providedIn: 'root'
})
export class ChatNewService {
  private http: HttpClient = inject(HttpClient);

  public getChat(chatId: string) {
    return this.http.get<Chat>(`${environment.apiUrl}/api/chat/${chatId}`);
  }

  public getMessages(chatId: string, offset: number, limit: number) {
    return this.http.get<Message[]>(`${environment.apiUrl}/api/chat/messages/${chatId}?offset=${offset}&limit=${limit}`);
  }
}
