import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {AuthorizeService} from "../authorize.service";
import {ActivatedRoute, Router} from '@angular/router';
import {el} from "@fullcalendar/core/internal-common";

/**
 * Componente responsável pelo formulário de login.
 * - Verifica se o utilizador já esta autenticado.
 * - Caso esteja autenticado redireciona para a pagina home.
 * @selector 'app-signin-component'
 * @templateUrl './signin.component.html'
 * @styleUrls './signin.component.css'
 */
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
  signedIn: boolean = false;
  submitting: boolean = false;


  /**
   * Construtor da classe SignInComponent.
   *
   * @param authService Serviço de autenticação.
   * @param formBuilder Construtor de formulários do Angular.
   * @param router Router do Angular.
   */
  constructor(private authService: AuthorizeService, private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router) {
    this.authService.isSignedIn().forEach(
      isSignedIn => {
        this.signedIn = isSignedIn;
        if (this.signedIn) {
          this.router.navigateByUrl('');
        }
      });
  }

  /**
   * Método do ciclo de vida do Angular, chamado na inicialização do componente.
   * - Inicializa o formulário de login ('signinForm').
   */
  ngOnInit(): void {
    this.submitting = true;
    this.errors = [];
    this.signinForm = this.formBuilder.group(
      {
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required]]
      });
    const providerId = this.route.snapshot.queryParamMap.get('providerId');
    const token = this.route.snapshot.queryParamMap.get('token');
    if (providerId !== null && token !== null) {
      this.authService.signInWithProviders(providerId, token).forEach(
        response => {
          if (response) {
            this.signedIn = true;
            this.router.navigateByUrl('').then(r => this.submitting = false);
          }
        }).catch(
        error => {
          this.errors.push("Acesso negado. Verifique as credenciais.");
          this.submitting = false;
        });
    } else {
      this.router.navigateByUrl('signin').then(r => this.submitting = false);
    }
    this.submitting = false;
  }

  get emailFormField() {
    return this.signinForm.get('email');
  }

  get passwordFormField() {
    return this.signinForm.get('password');
  }

  /**
   * Tentativa de login com as credenciais fornecidas no formulário.
   * - Verifica se o formulário é válido.
   * - Chama o serviço de autenticação ('AuthorizeService') para fazer login.
   * - Navega para a página home em caso de sucesso.
   * - Caso o acesso seja negado, é apresentada uma mensagem de erro.
   *
   * @param _
   */
  public signin(_: any) {
    this.submitting = true;
    if (!this.signinForm.valid) {
      this.submitting = false;
      return;
    }
    this.errors = [];

    const userName = this.signinForm.get('email')?.value;
    const password = this.signinForm.get('password')?.value
    this.authService.signIn(userName, password).forEach(
      response => {
        if (response) {
          this.signedIn = true;
          this.submitting = false;
          this.router.navigateByUrl('');
        }
      }).catch(
      error => {
        this.errors.push("Acesso negado. Verifique as credenciais.");
        this.submitting = false;
      }
    );
  }
}

