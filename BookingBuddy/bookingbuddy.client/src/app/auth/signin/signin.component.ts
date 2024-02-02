import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AuthorizeService } from "../authorize.service";
import { Router } from '@angular/router';

@Component(
  {
    selector: 'app-signin-component',
    templateUrl: './signin.component.html',
    styleUrl: './signin.component.css'
  }
)
export class SignInComponent implements OnInit {
  errors: string[] = [];
  signinForm!: FormGroup;
  signinFailed: boolean = false;
  signinSucceeded: boolean = false;
  signedIn: boolean = false;

  constructor(private authService: AuthorizeService, private formBuilder: FormBuilder, private router: Router) {
    this.authService.isSignedIn().forEach(
      isSignedIn => {
        this.signedIn = isSignedIn;
      });
  }

  ngOnInit(): void {
    this.errors = [];
    this.signinFailed = false;
    this.signinSucceeded = false;
    this.signinForm = this.formBuilder.group(
      {
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required]]
      });
  }

  public signin(_: any) {
    if (!this.signinForm.valid) {
      return;
    }

    this.signinFailed = false;
    this.errors = [];

    const userName = this.signinForm.get('email')?.value;
    const password = this.signinForm.get('password')?.value;

    this.authService.signIn(userName, password).forEach(
      response => {
        if (response) {
          this.signinSucceeded = true;
          this.signedIn = true;
          this.router.navigateByUrl('');
        }
      }).catch(
      error => {
          this.signinFailed = true;
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
      }
    );
  }
}

