import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FeedbackService {

  private _feedback: Map<string, string[]> = new Map<string, string[]>();

  constructor() {
    this._feedback.set("homepage", ["Welcome to the homepage!"]);
  }

  consumeFeedback(key: string): string[] {
    return this._feedback.get(key) ?? [];
  }

  addFeedback(key: string, feedback: string): void {
    if (this._feedback.has(key)) {
      this._feedback.get(key)?.push(feedback);
    } else {
      this._feedback.set(key, [feedback]);
    }
  }

  setFeedback(key: string, feedback: string[]): void {
    this._feedback.set(key, feedback);
  }

  getFeedback(key: string): string[] {
    return this._feedback.get(key) ?? [];
  }
}
