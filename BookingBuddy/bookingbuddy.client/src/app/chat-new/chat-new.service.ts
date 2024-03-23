import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class ChatNewService {

  constructor(private http: HttpClient) {
  }

  public createChat() {
  }
}
