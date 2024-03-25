import {Component, Input, OnInit} from '@angular/core';
import {Chat} from "../models/chat";
import {ChatNewService} from "./chat-new.service";

@Component({
  selector: 'app-chat-new',
  templateUrl: './chat-new.component.html',
  styleUrl: './chat-new.component.css'
})
export class ChatNewComponent implements OnInit{

  constructor(private chatService: ChatNewService) {
  }

  ngOnInit(): void {
  }
  @Input() chatId: string | undefined;

}
