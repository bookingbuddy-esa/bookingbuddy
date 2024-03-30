import { Component, ElementRef, SecurityContext, ViewChild } from '@angular/core';
import { AppComponent } from '../app.component';
import { UserInfo } from '../auth/authorize.dto';
import { AuthorizeService } from '../auth/authorize.service';
import { FooterService } from '../auxiliary/footer.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ProfileService } from './profile.service';
import { ProfileInfo } from '../models/profile';
import { HostingService } from '../hosting/hosting.service';
import { Property } from '../models/property';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  @ViewChild('descriptionElement') descriptionElement: ElementRef | undefined;
  @ViewChild('fileInput') fileInput: ElementRef | undefined;
  submitting: boolean = false;
  user: UserInfo | undefined;
  profileInfo: ProfileInfo | undefined;
  userProperties: Property[] = [];

  signedIn: boolean = false;
  isViewingOwnProfile: boolean = false;
  isEditing: boolean = false;

  protected selectedFiles: File[] = [];
  protected selectedFilesURL: string[] = [];
  
  errors: string[] = [];
  success: string[] = [];

  constructor(private authService: AuthorizeService, private profileService: ProfileService, private hostingService: HostingService, private footerService: FooterService, private route: ActivatedRoute, private router: Router, private sanitizer: DomSanitizer){
    this.footerService.hideFooter();
  }

  ngOnInit(): void {
    this.submitting = true;
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
              this.hostingService.getPropertiesByUserId(params['id']).forEach(properties => {
                this.userProperties = properties;
              })
            }
          });
        });
        this.submitting = false;
      }
    });
  }

  getProfile(userId: string): void {
    this.profileService.getProfile(userId).forEach(profile => {
      this.profileInfo = profile;
      console.log(this.profileInfo)
      this.profileInfo.roles = this.profileInfo.roles.map(role => {
        switch(role){
          case "admin":
            return "Administrador";
          case "landlord":
            return "Proprietário";
          case "user":
            return "Utilizador";
          default:
            return "";
        }
      });
    }).catch(error => {
      this.router.navigate(['/error']);
    });
  }

  get profilePicture(): string {
    return this.profileInfo?.pictureUrl || '../../assets/img/user_logo.png';
  }

  get name(): string {
    return this.profileInfo?.name || '';
  }

  get role(): string {
    return this.profileInfo?.roles.join(", ") || '';
  }

  get email(): string {
    return this.profileInfo?.email || '';
  }

  get description(): string {
    return this.profileInfo?.description || 'Este utilizador não tem descrição...';
  }

  getPropertyImage(property: Property) {
    if (property && property.imagesUrl && property.imagesUrl.length > 0) {
      return property.imagesUrl[0];
    }

    return 'N/A'; // TODO: foto default caso nao tenha?
  }

  saveProfile(): void {
    const innerText = this.descriptionElement?.nativeElement.innerText;
    if (innerText) {
      const currentDescription = innerText.trim();
  
      if (currentDescription !== this.profileInfo?.description) {
        this.profileInfo!.description = currentDescription;
        this.profileService.updateDescription(this.profileInfo!.description).forEach(response => {
          if (response) {
            this.success.push(response);
          }
        }).catch(error => {
          console.log(error);
          this.errors = ["Erro ao atualizar o perfil."];
        });
      }
    }

    this.isEditing = false;
    this.errors = [];
    this.success = [];
  }
  

  openFileUploader(): void {
    this.fileInput?.nativeElement.click();
  }

  onImagesSelect(event: Event): void {
    this.selectedFiles = [];
    this.selectedFilesURL = [];

    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
      const file = target.files[0];
      if (file.type === 'image/jpeg' || file.type === 'image/png') {
        this.selectedFiles.push(target.files[0]);
        const reader = new FileReader();
        reader.onload = (e) => {
          this.selectedFilesURL.push(e.target!.result as string);
        };
        reader.readAsDataURL(target.files[0]);
        this.submitting = true;
      } else {
          this.errors.push(`${file.name} não é um ficheiro de imagem válido.`);
      }
    }

    this.profileService.uploadProfilePicture(this.selectedFiles).forEach(uploadedImageURL => {
      if(uploadedImageURL){
        this.profileService.updateProfilePicture(uploadedImageURL[0]).forEach(response => {
          if (response) {
            this.profileInfo!.pictureUrl = uploadedImageURL[0];
            this.success.push("Imagem de perfil atualizada com sucesso.");
          }
        }).catch(error => {
          this.errors.push("Erro ao atualizar a imagem de perfil.");
        });
        
        this.submitting = false;
      }
    }).catch(error => {
      this.errors.push("Erro ao atualizar a imagem de perfil.");
    });

    this.errors = [];
    this.success = [];
  }

  isPropertyOwner(property: Property): boolean {
    return property.applicationUserId === this.user?.userId;
  }
}
