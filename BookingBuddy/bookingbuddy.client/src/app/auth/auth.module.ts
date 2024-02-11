import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterComponent } from './register/register.component';
import { SignInComponent } from './signin/signin.component';
import { RecoverPwComponent } from './recover-pw/recover-pw.component';

import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { ResetPwComponent } from './reset-pw/reset-pw.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { AuxiliaryModule } from '../auxiliary/auxiliary.module';
import { GoogleComponent } from './external-providers/google/google.component';
import { MicrosoftComponent } from './external-providers/microsoft/microsoft.component';
import { LogoutComponent } from './logout/logout.component';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    AuxiliaryModule,
    RouterModule.forChild(
      [
        { path: 'register', component: RegisterComponent },
        { path: 'signin', component: SignInComponent },
        { path: 'recover-password', component: RecoverPwComponent },
        { path: 'reset-password', component: ResetPwComponent },
        { path: 'confirm-email', component: ConfirmEmailComponent },
        { path: 'logout', component: LogoutComponent }
      ]
    )
  ],
  declarations: [ RegisterComponent, RecoverPwComponent, SignInComponent, ResetPwComponent, ConfirmEmailComponent, GoogleComponent, MicrosoftComponent, LogoutComponent],
  exports: [ RegisterComponent, RecoverPwComponent, SignInComponent, ResetPwComponent]
})
export class AuthModule { }

