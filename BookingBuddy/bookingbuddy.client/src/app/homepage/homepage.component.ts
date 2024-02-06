import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../auth/authorize.service';
import { UserInfo } from '../auth/authorize.dto';
import { Router } from '@angular/router';
import { Property } from '../models/property';

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

  constructor(private authService: AuthorizeService, private router: Router) { }

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
    for (let i = 0; i < 25; i++) {
      this.property_list.push({ name: "propriedade " + i.toString(), location: "Setúbal", pricePerNight: 100 + i });
    }
  }

  public logout() {
    this.authService.signOut().forEach(response => {
      if (response) {
        this.router.navigateByUrl('');
      }
    });
  }
}
