import {Component, OnInit, HostListener, inject} from '@angular/core';
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
  protected isExpanded = false;
  private authService: AuthorizeService = inject(AuthorizeService);
  private router: Router = inject(Router);

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.isExpanded = event.target.innerWidth <= 576 && this.isExpanded;
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

  public async updateUserInfo() {
    return this.authService.user().forEach(user => {
      this.user = user
      if (user.roles.includes('landlord') || user.roles.includes('admin')) {
        this.isLandlord = true;
      }
    });
  }

  collapse() {
    this.isExpanded = false;
  }

  search(event: any) {

    if (event.key === 'Enter') {
      const search = event.target.value.trim();
      
      if (search) {
        this.router.navigate(['/'], { queryParams: { search: search } });
      }
    }

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
