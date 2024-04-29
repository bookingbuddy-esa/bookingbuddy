import {Component, OnInit} from '@angular/core';
import {FooterService} from "./auxiliary/footer.service";
import {FeedbackService} from "./auxiliary/feedback.service";
import {Feedback, FeedbackType} from "./models/feedback";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'Booking Buddy';
  protected readonly FeedbackType = FeedbackType;
  showFooter: boolean = true;
  currentFeedback: Feedback | undefined;

  constructor(private footerService: FooterService, protected feedbackService: FeedbackService) {
    this.footerService.isFooterVisible().forEach(isVisible => {
      this.showFooter = isVisible;
    });
    this.feedbackService.getFeedback().forEach(feedback => {
      this.currentFeedback = feedback;
    });
  }
}

