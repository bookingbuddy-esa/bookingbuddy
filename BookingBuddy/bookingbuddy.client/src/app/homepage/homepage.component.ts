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
  editPassword: boolean = false;
  changePWForm!: FormGroup;
  submitting: boolean = false;
  errors: string[] = [];
  user: UserInfo | undefined;

  constructor(private authService: AuthorizeService, private router: Router) {
    this.errors = [];
    this.changePWForm = new FormGroup({
      password: new FormControl<string>('', {
        validators: [Validators.required, Validators.pattern(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*]).{6,}$/)],
      }),
      confirmPassword: new FormControl<string>('', {
        validators: [Validators.required]
      }),
      oldPassword: new FormControl<string>('', {
        validators: [Validators.required]
      }),
    });
  }

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

  get passwordFormField() {
    return this.changePWForm.get('password');
  }

  public changePassword(_: any) {
    this.submitting = true;
    if (!this.changePWForm.valid) {
      this.submitting = false;
      return;
    }
    this.errors = [];
    const password = this.changePWForm.get('password')?.value;
    const confirmPassword = this.changePWForm.get('confirmPassword')?.value;
    const oldPassword = this.changePWForm.get('oldPassword')?.value;
    if (password !== confirmPassword) {
      this.errors.push('As palavras-passe não são iguais.');
      this.submitting = false;
      return;
    }
    this.authService.changePassword(password, oldPassword).forEach(
      response => {
        if (response) {
          this.submitting = false;
          this.editPassword = false;
          this.changePWForm.reset();
        }
      }).catch(
        error => {
          if (error.error) {
            const errorObj = JSON.parse(error.error);
            Object.entries(errorObj).forEach((entry) => {
              const [key, value] = entry;
              var code = (value as any)["code"]
              var description = (value as any)["description"];
              if (code === "InvalidToken" || code === "UserNotFound") {
                //em caso de o token ou o user serem inválidos
                //this.validUrl = false;
              }
              this.errors.push(description);
            });
          }
          this.submitting = false;
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
