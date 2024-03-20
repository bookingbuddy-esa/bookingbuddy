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
  isEditing: boolean = false;
  errors: string[] = [];
  success: string[] = [];

  constructor(private authService: AuthorizeService){
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

  get name(): string {
    return this.user?.name || '';
  }

  get role(): string {
    switch(this.user?.roles[0]){
      case "admin":
        return "Administrador";
      case "landlord":
        return "Proprietário";
      case "user":
        return "Utilizador";
      default:
        return "N/A";
    }
  }

  get email(): string {
    return this.user?.email || '';
  }

  saveProfile(): void {
    this.isEditing = false;
    this.success = ["Perfil atualizado com sucesso!"];
  }

  editProfile(): void {
    this.isEditing = true;
  }

  uploadPhoto(): void {
    console.log("upload photo");
  }
}
