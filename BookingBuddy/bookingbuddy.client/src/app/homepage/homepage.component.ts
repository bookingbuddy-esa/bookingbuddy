import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../auth/authorize.service';
import { UserInfo } from '../auth/authorize.dto';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrl: './homepage.component.css'
})
export class HomepageComponent implements OnInit {
  signedIn: boolean = false;
  submitting: boolean = false;
  user: UserInfo | undefined;

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
  }

  public logout() {
    this.authService.signOut().forEach(response => {
      if (response) {
        this.router.navigateByUrl('');
      }
    });
  }
}
