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
  numberOfPages: number = 1;
  startIndex: number = 0;
  itemsPerPage: number = 100;
  numberOfProperties: number = 0;

  constructor(
    private authService: AuthorizeService,
    private propertyService: PropertyAdService,
    private router: Router,
    private FeedbackService: FeedbackService) {
  }

  ngOnInit(): void {
    this.authService.isSignedIn().forEach(isSignedIn => {
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

    this.submitting = true;
    this.countProperties().then(() => this.loadProperties());
  }

  countProperties() {
    return new Promise<void>((resolve, reject) => {
      this.propertyService.getPropertiesCount().forEach(response => {
        if (response) {
          this.numberOfProperties = response as number;
          this.numberOfPages = Math.ceil(this.numberOfProperties / this.itemsPerPage);
          resolve();
        } else {
          reject("Error fetching properties count");
        }
      });
    });
  }

  loadProperties() {
    console.log("A carregar: " + this.itemsPerPage + " propriedades a partir do índice " + this.startIndex)
    this.propertyService.getProperties(this.itemsPerPage, this.startIndex).subscribe(response => {
      if (response) {
        this.property_list = response as Property[];
        this.submitting = false;
      }
    });
  }

  updateItemsPerPage(value: number) {
    this.itemsPerPage = value;
    this.countProperties().then(() => this.loadProperties());
  }

  setPage(page: number) {
    if(page < 1 || page > this.numberOfPages || this.startIndex === page - 1) {
      return;
    }

    this.startIndex = page - 1;
    this.loadProperties();
  }

  previousPage() {
    if (this.startIndex > 0) {
      console.log("Previous page -> " + this.startIndex)
      this.startIndex -= 1;
      console.log("Depois: " + this.startIndex)
      this.loadProperties();
    }
  }

  nextPage() {
    if (this.startIndex < this.numberOfPages - 1) {
      console.log("Next page -> " + this.startIndex)
      this.startIndex += 1;
      console.log("Depois: " + this.startIndex)
      this.loadProperties();
    }
  }

  getPages(): number[] {
    return Array.from({ length: this.numberOfPages }, (_, i) => i + 1);
  }
}
