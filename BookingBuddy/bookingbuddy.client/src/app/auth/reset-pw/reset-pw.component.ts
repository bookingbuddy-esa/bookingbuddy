import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl } from "@angular/forms";
import { AuthorizeService } from "../authorize.service";
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-reset-pw',
  templateUrl: './reset-pw.component.html',
  styleUrl: './reset-pw.component.css'
})
export class ResetPwComponent {
  errors: string[] = [];
  resetPWForm!: FormGroup;
  emailSent: boolean = false;
  submitting: boolean = false;
  token: string = "";
  uid: string = "";
  validUrl: boolean = false;

  constructor(private authService: AuthorizeService, private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.errors = [];
    this.resetPWForm = new FormGroup({
      password: new FormControl<string>('', {
        validators: [Validators.required, Validators.pattern(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*]).{6,}$/)],
      }),
      confirmPassword: new FormControl<string>('', {
        validators: [Validators.required]
      })
    });
    var urlUid = this.route.snapshot.queryParamMap.get("uid");
    var urlToken = this.route.snapshot.queryParamMap.get("token");
    if (urlUid !== null && urlToken !== null) {
      this.uid = urlUid;
      this.token = urlToken;
      this.validUrl = true
    }
  }

  get passwordFormField() {
    return this.resetPWForm.get('password');
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
          this.submitting = false;
          this.router.navigateByUrl("/signin")
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
}
