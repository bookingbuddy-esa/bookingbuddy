import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AuthorizeService } from "../authorize.service";

@Component(
  {
    selector: 'app-recover-pw',
    templateUrl: './recover-pw.component.html',
    styleUrl: './recover-pw.component.css'
  }
)
export class RecoverPwComponent implements OnInit {
  errors: string[] = [];
  recoverPWForm!: FormGroup;
  emailSent: boolean = false;

  constructor(private authService: AuthorizeService, private formBuilder: FormBuilder) {

  }

  ngOnInit(): void {
    this.errors = [];
    this.recoverPWForm = this.formBuilder.group(
      {
        email: ['', [Validators.required, Validators.email]],

      });
  }
  public recover(_: any) {
    if (!this.recoverPWForm.valid) {
      return;
    }

    this.errors = [];

    const email = this.recoverPWForm.get('email')?.value;

    this.authService.recoverPassword(email).forEach(
      response => {
        if (response) {
          this.emailSent = true;
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
        }
      );
  }
}
