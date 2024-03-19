import { Component } from '@angular/core';
import { AppComponent } from '../app.component';
import { UserInfo } from '../auth/authorize.dto';
import { AuthorizeService } from '../auth/authorize.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  user: UserInfo | undefined;
  signedIn: boolean = false;
  constructor(private appComponent: AppComponent, private authService: AuthorizeService){
    this.appComponent.showChat = false;
  }

  ngOnInit(): void {
    this.authService.isSignedIn().forEach(
      isSignedIn => {
        this.signedIn = isSignedIn;
        if (isSignedIn) {
          this.authService.user().forEach(user => this.user = user);
        }
      });
  }


}
