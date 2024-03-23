import {Component} from '@angular/core';
import {FooterService} from "./auxiliary/footer.service";
import {FeedbackService} from "./auxiliary/feedback.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'Booking Buddy';
  showFooter: boolean = true;

  constructor(private footerService: FooterService) {
    this.footerService.isFooterVisible().forEach(isVisible => {
      this.showFooter = isVisible;
    });
  }
}

