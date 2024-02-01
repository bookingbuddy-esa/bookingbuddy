import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AuthorizeService } from "../authorize.service";
import { ActivatedRoute } from '@angular/router';
import { query } from '@angular/animations';

@Component({
  selector: 'app-reset-pw',
  templateUrl: './reset-pw.component.html',
  styleUrl: './reset-pw.component.css'
})
export class ResetPwComponent {
  errors: string[] = [];
  resetPWForm!: FormGroup;
  emailSent: boolean = false;
  token: string = "";
  uid: string = "";
  validUrl: boolean = false;

  constructor(private authService: AuthorizeService, private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.errors = [];
    this.resetPWForm = this.formBuilder.group(
      {
        password: ['', [Validators.required]],
        confirmPassword: ['', [Validators.required]]
      });

    // implementação sem o Router
    var url = location.href;
    var query = url.split('?')[1];
    if (query !== undefined) {
      query.split('&').forEach(param => {
        var elem = param.split('=');
        if (elem[0] === "uid") {
          this.uid = decodeURIComponent(elem[1]);
        }
        if (elem[0] === "token") {
          this.token = decodeURIComponent(elem[1]);
        }
      });
    }
    if (this.uid !== "" && this.token !== "") {
      this.validUrl = true
    }
  }

  public reset(_: any) {
    if (!this.resetPWForm.valid) {
      return;
    }
    this.errors = [];
    const password = this.resetPWForm.get('password')?.value;
    const confirmPassword = this.resetPWForm.get('confirmPassword')?.value;
    if (password !== confirmPassword) {
      this.errors.push('Passwords do not match.');
      return;
    }
    this.authService.resetPassword(this.uid, this.token, password).forEach(
      response => {
        if (response) {
          location.href = "/signin"
        }
      }).catch(
        error => {
          if (error.error) {
            const errorObj = JSON.parse(error.error);
            if (errorObj && errorObj.errors) {
              // problem details { "field1": [ "error1", "error2" ], "field2": [ "error1", "error2" ]}
              const errorList = errorObj.errors;
              for (let field in errorList) {
                if (Object.hasOwn(errorList, field)) {
                  let list: string[] = errorList[field];
                  for (let idx = 0; idx < list.length; idx += 1) {
                    this.errors.push(`${field}: ${list[idx]}`);
                  }
                }
              }
            }
          }
        });
  }
}
