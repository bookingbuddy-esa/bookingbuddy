import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl} from "@angular/forms";
import {AuthorizeService} from "../authorize.service";
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'app-reset-pw',
  templateUrl: './reset-pw.component.html',
  styleUrl: './reset-pw.component.css'
})
export class ResetPwComponent implements OnInit {
  errors: string[] = [];
  resetPWForm!: FormGroup;
  submitting: boolean = false;
  token: string = "";
  uid: string = "";

  constructor(private authService: AuthorizeService, private route: ActivatedRoute, private router: Router) {
  }

  ngOnInit(): void {
    this.submitting = true;
    this.errors = [];
    this.resetPWForm = new FormGroup({
      password: new FormControl<string>('', {
        validators: [Validators.required, Validators.pattern(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*]).{6,}$/)],
      }),
      confirmPassword: new FormControl<string>('', {
        validators: [Validators.required]
      })
    });
    const urlUid = this.route.snapshot.queryParamMap.get("uid");
    const urlToken = this.route.snapshot.queryParamMap.get("token");
    if (urlUid !== null && urlToken !== null) {
      this.uid = urlUid;
      this.token = urlToken;
      this.authService.checkResetToken(this.uid, this.token).forEach(
        response => {
          if (response) {
            this.submitting = false;
          }
        }).catch(
        error => {
          console.log(error);
          this.router.navigate(["bad-request"]).then(r => {
            this.submitting = false;
          });
        });
    } else {
      this.submitting = false;
      this.router.navigate(["bad-request"]).then(r => {
        this.submitting = false;
      });
    }
  }

  get passwordFormField() {
    return this.resetPWForm.get('password');
  }

  get confirmPasswordFormField() {
    return this.resetPWForm.get('confirmPassword');
  }

  public reset(_: any) {
    this.submitting = true;
    if (!this.resetPWForm.valid) {
      this.submitting = false;
      return;
    }
    this.errors = [];
    const password = this.resetPWForm.get('password')?.value;
    const confirmPassword = this.resetPWForm.get('confirmPassword')?.value;
    if (password !== confirmPassword) {
      this.errors.push('As palavras-passe não são iguais.');
      this.submitting = false;
      return;
    }
    this.authService.resetPassword(this.uid, this.token, password).forEach(
      response => {
        if (response) {
          this.router.navigate(["signin"]).then(r => {
            this.submitting = false;
          });
        }
      }).catch(
      error => {
        if (error.error) {
          const errorObj = JSON.parse(error.error);
          if (errorObj && errorObj.errors) {
            const errorList = errorObj.errors;
            for (let field in errorList) {
              if (Object.prototype.hasOwnProperty.call(errorList, field)) {
                let list: string[] = errorList[field];
                for (let idx = 0; idx < list.length; idx += 1) {
                  this.errors.push(`${field}: ${list[idx]}`);
                }
              }
            }
          }
        }
        this.submitting = false;
      });
  }
}
