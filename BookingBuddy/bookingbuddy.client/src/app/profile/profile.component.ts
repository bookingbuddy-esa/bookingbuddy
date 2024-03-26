import { Component } from '@angular/core';
import { AppComponent } from '../app.component';
import { UserInfo } from '../auth/authorize.dto';
import { AuthorizeService } from '../auth/authorize.service';
import { FooterService } from '../auxiliary/footer.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ProfileService } from './profile.service';
import { ProfileInfo } from '../models/profile';
import { HostingService } from '../hosting/hosting.service';
import { Property } from '../models/property';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  user: UserInfo | undefined;
  profileInfo: ProfileInfo | undefined;
  userProperties: Property[] = [];
  signedIn: boolean = false;
  isViewingOwnProfile: boolean = false;
  isEditing: boolean = false;
  errors: string[] = [];
  success: string[] = [];

  constructor(private authService: AuthorizeService, private profileService: ProfileService, private hostingService: HostingService, private footerService: FooterService, private route: ActivatedRoute, private router: Router){
    this.footerService.hideFooter();
  }

  ngOnInit(): void {
    this.authService.isSignedIn().subscribe(isSignedIn => {
      this.signedIn = isSignedIn;
      if (isSignedIn) {
        this.authService.user().subscribe(user => {
          this.user = user;
          this.route.params.subscribe(params => {
            if (!params['id']) {
              this.getProfile(this.user!.userId);
              this.hostingService.getPropertiesByUserId(this.user!.userId).forEach(properties => {
                this.userProperties = properties;
              })
              this.isViewingOwnProfile = true;
            } else if(params['id'] === this.user!.userId) {
              this.router.navigate(['/profile']);
            } else {
              this.getProfile(params['id']);
              this.hostingService.getPropertiesByUserId(this.user!.userId).forEach(properties => {
                this.userProperties = properties;
              })
            }
          });
        });
      }
    });
  }

  getProfile(userId: string): void {
    this.profileService.getProfile(userId).forEach(profile => {
      this.profileInfo = profile;
    }).catch(error => {
      this.router.navigate(['/error']);
    });
  }

  get name(): string {
    return this.profileInfo?.name || '';
  }

  get role(): string {
    switch(this.profileInfo?.roles[0]){
      case "admin":
        return "Administrador";
      case "landlord":
        return "Proprietário";
      case "user":
        return "Utilizador";
      default:
        return "";
    }
  }

  get email(): string {
    return this.profileInfo?.email || '';
  }

  get description(): string {
    return this.profileInfo?.description || 'Este utilizador não tem descrição...';
  }

  public getPropertyImage(property: Property) {
    if (property && property.imagesUrl && property.imagesUrl.length > 0) {
      return property.imagesUrl[0];
    }

    return 'N/A'; // TODO: foto default caso nao tenha?
  }

  saveProfile(): void {
    this.isEditing = false;
    this.success = ["Perfil atualizado com sucesso!"];
  }

  uploadPhoto(): void {
    console.log("upload photo");
  }
}
