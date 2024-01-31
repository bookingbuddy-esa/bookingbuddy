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


  }
}
