import {Component, OnInit} from '@angular/core';
import {AuthorizeService} from '../auth/authorize.service';
import {UserInfo} from '../auth/authorize.dto';
import {Router} from '@angular/router';
import {Property} from '../models/property';
import {PropertyAdService} from '../property-ad/property-ad.service';
import {FeedbackService} from "../auxiliary/feedback.service";

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrl: './homepage.component.css'
})
export class HomepageComponent implements OnInit {
  signedIn: boolean = false;
  submitting: boolean = false;
  user: UserInfo | undefined;
  property_list: Property[] = [];

  constructor(
    private authService: AuthorizeService,
    private propertyService: PropertyAdService,
    private router: Router,
    private FeedbackService: FeedbackService) {}

  ngOnInit(): void {
    this.authService.isSignedIn().forEach(
      isSignedIn => {
        this.signedIn = isSignedIn;
        if (isSignedIn) {
          this.authService.user().forEach(user => this.user = user);
        }
      });
    this.authService.onStateChanged().forEach(isSignedIn => {
      this.signedIn = isSignedIn;
      if (isSignedIn) {
        this.authService.user().forEach(user => this.user = user);
      }
    });

    this.propertyService.getProperties().forEach(
      response => {
        if (response) {
          this.property_list = response as Property[];
        }
      }).catch(
      error => {
        //this.errors.push("TODO");
      }
    );
  }
}
