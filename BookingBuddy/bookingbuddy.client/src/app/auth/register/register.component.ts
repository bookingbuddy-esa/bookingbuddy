import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AuthorizeService } from "../authorize.service";

@Component({
  selector: 'app-register-component',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  errors: string[] = [];
  registerForm!: FormGroup;
  registerFailed: boolean = false;
  registerSucceeded: boolean = false;
  signedIn: boolean = false;

  constructor(private authService: AuthorizeService,
    private formBuilder: FormBuilder) {
    this.authService.isSignedIn().forEach(
      isSignedIn => {
        this.signedIn = isSignedIn;
      });
  }

  ngOnInit(): void {
    this.registerFailed = false;
    this.registerSucceeded = false;
    this.errors = [];
    this.registerForm = this.formBuilder.group(
      {
        name: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required]],
        confirmPassword: ['', [Validators.required]]
      });
  }

  public register(_: any) {
    if (!this.registerForm.valid) {
      return;
    }
    this.registerFailed = false;
    this.errors = [];
    const name = this.registerForm.get('name')?.value;
    const userName = this.registerForm.get('email')?.value;
    const password = this.registerForm.get('password')?.value;
    const confirmPassword = this.registerForm.get('confirmPassword')?.value;
    if (password !== confirmPassword) {
      this.registerFailed = true;
      this.errors.push('As palavras-passe não coincidem.');
      return;
    }
    this.authService.register(name, userName, password).forEach(
      response => {
        if (response) {
          this.registerSucceeded = true;
        }
      }).catch(
        error => {
          this.registerFailed = true;
          if (error.error) {
            console.log(error.error);
            const errorObj = JSON.parse(error.error);
            console.log(errorObj);
            Object.entries(errorObj).forEach((entry) => {
              const [key, value] = entry;

              if (value && typeof value === 'object' && 'description' in value) {
                const description = value.description as string;
                this.errors.push(description);
              }
              /*var code = (value as any)["code"]
              switch (code) {
                case "InvalidToken": {
                  this.validUrl = false;

                  break;
                }
                default: {
                  this.errors.push("Erro desconhecido!")
                }
              }*/
            });
          }
        });
  }
}
