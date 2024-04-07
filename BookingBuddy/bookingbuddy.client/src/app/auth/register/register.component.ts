import {Component, OnInit} from "@angular/core";
import {FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl} from "@angular/forms";
import {AuthorizeService} from "../authorize.service";

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
  submitting: boolean = false;
  emailFailed: boolean = false;

  constructor(private authService: AuthorizeService,
              private formBuilder: FormBuilder) {
    this.authService.isSignedIn().forEach(
      isSignedIn => {
        this.signedIn = isSignedIn;
      });
  }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group(
      {
        name: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        password: new FormControl<string>('', {
          validators: [Validators.required, Validators.pattern(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*?_=."/()]).{6,}$/)],
        }),
        confirmPassword: new FormControl<string>('', {
          validators: [Validators.required]
        })
      });
    this.confirmPasswordFormField?.valueChanges.forEach(value => {
      this.confirmPasswordFormField?.setErrors(value !== this.passwordFormField?.value ? {mismatch: true} : null);
    });
  }

  get nameFormField() {
    return this.registerForm.get('name');
  }

  get emailFormField() {
    return this.registerForm.get('email');
  }

  get passwordFormField() {
    return this.registerForm.get('password');
  }

  get confirmPasswordFormField() {
    return this.registerForm.get('confirmPassword');
  }

  public register(_: any) {
    this.submitting = true;
    if (!this.registerForm.valid) {
      this.submitting = false;
      return;
    }
    this.registerFailed = false;
    this.errors = [];
    const name = this.registerForm.get('name')?.value;
    const userName = this.registerForm.get('email')?.value;
    const password = this.registerForm.get('password')?.value;
    this.authService.register(name, userName, password).forEach(
      response => {
        if (response) {
          this.registerSucceeded = true;
          this.submitting = false;
        }
      }).catch(
      error => {
        this.registerFailed = true;
        if (error.error) {
          const errorObj = JSON.parse(error.error);
          Object.entries(errorObj).forEach((entry) => {
            const [key, value] = entry;
            if (value && typeof value === 'object' && 'description' in value) {
              const description = value.description as string;
              this.errors.push(description);
              if (value && typeof value === 'object' && 'code' in value) {
                const code = value.code as string;
                if (code === 'DuplicateEmail') {
                  this.emailFailed = true;
                }
              }
            }
          });
        }
        this.submitting = false;
      });
  }

  public resendEmail() {
    this.submitting = true;
    this.authService.resendConfirmationEmail(
      this.registerForm.get('email')?.value
    ).forEach(response => {
      if (response) {
        this.submitting = false;
      }
    }).catch(error => {
      this.submitting = false;
    });
  }
}
