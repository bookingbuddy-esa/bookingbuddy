import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AuthorizeService } from "../authorize.service";

@Component(
  {
    selector: 'app-signin',
    templateUrl: './signin.component.html',
    styleUrl: './signin.component.css'
  }
)
export class SignInComponent implements OnInit {
  errors: string[] = [];
  signinForm!: FormGroup;


  constructor(private authService: AuthorizeService, private formBuilder: FormBuilder) {

  }

  ngOnInit(): void {
    this.errors = [];
    this.signinForm = this.formBuilder.group(
      {
        email: ['', [Validators.required, Validators.email]],

      });
  }
  public signin(_: any) {


  }
}

