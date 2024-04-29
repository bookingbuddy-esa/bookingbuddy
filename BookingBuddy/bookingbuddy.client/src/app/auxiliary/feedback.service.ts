import {Injectable} from '@angular/core';
import {BehaviorSubject, Subject} from "rxjs";
import {Feedback, FeedbackType} from "../models/feedback";

@Injectable({
  providedIn: 'root'
})
export class FeedbackService {
  private currentFeedback: Subject<Feedback | undefined> = new Subject<Feedback | undefined>();

  constructor() {
  }

  public getFeedback() {
    return this.currentFeedback.asObservable();
  }

  public setFeedback(feedback: Feedback) {
    this.currentFeedback.next(feedback);
  }

  public clear() {
    this.currentFeedback.next(undefined);
  }
}
