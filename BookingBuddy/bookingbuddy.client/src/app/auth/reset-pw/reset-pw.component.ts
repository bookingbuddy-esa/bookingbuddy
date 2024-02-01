import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AuthorizeService } from "../authorize.service";

@Component({
  selector: 'app-reset-pw',
  templateUrl: './reset-pw.component.html',
  styleUrl: './reset-pw.component.css'
})
export class ResetPwComponent {
  errors: string[] = [];
  resetPWForm!: FormGroup;
  emailSent: boolean = false;

  constructor(private authService: AuthorizeService, private formBuilder: FormBuilder) {

  }

  ngOnInit(): void {
    this.errors = [];
    this.resetPWForm = this.formBuilder.group(
      {
        email: ['', [Validators.required, Validators.email]],

      });
  }
  public reset(_: any) {
    
  }
}
