import {Component, OnInit} from '@angular/core';
import {AuthorizeService} from "../auth/authorize.service";
import {Router} from "@angular/router";
import {UserInfo} from "../auth/authorize.dto";

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.css'
})
export class MenuComponent implements OnInit {
  protected signedIn: boolean = false;
  protected user: UserInfo | undefined;
  protected isLandlord: boolean = false;

  resize: ResizeObserver = new ResizeObserver(() => {
    if(window.innerWidth > 768 && this.isExpanded) {
      this.isExpanded = false;
    }
  });

  constructor(private authService: AuthorizeService, private router: Router) {
    this.resize.observe(document.body);
  }

  ngOnInit(): void {
    this.authService.isSignedIn().forEach(
      isSignedIn => {
        this.signedIn = isSignedIn;
        if (isSignedIn) {
          this.authService.user().forEach(user => {
            this.user = user
            if (user.roles.includes('landlord') || user.roles.includes('admin')) {
              this.isLandlord = true;
            }
          });
        }
      });
    this.authService.onStateChanged().forEach(isSignedIn => {
      this.signedIn = isSignedIn;
      if (isSignedIn) {
        this.authService.user().forEach(user => {
          this.user = user
          if (user.roles.includes('landlord') || user.roles.includes('admin')) {
            this.isLandlord = true;
          }
        });
      }
    });
  }

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  signOut() {
    this.router.navigateByUrl('/logout').then(
      () => {
        this.isLandlord = false;
        this.signedIn = false;
        this.user = undefined;
      }
    )
  }
}
