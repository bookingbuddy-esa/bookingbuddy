import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterComponent } from './register/register.component';
import { SignInComponent } from './signin/signin.component';
import { RecoverPwComponent } from './recover-pw/recover-pw.component';

import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { ResetPwComponent } from './reset-pw/reset-pw.component';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule.forChild(
      [
        //{ path: 'sigin', component: SignInComponent },
        { path: 'recover-password', component: RecoverPwComponent },
        { path: 'register', component: RegisterComponent },
        { path: 'signin', component: SignInComponent },
        { path: 'reset-pw', component: ResetPwComponent },
      ]
    )
  ],
  declarations: [/*LoginMenuComponent,*/ RegisterComponent, RecoverPwComponent, SignInComponent, ResetPwComponent],
  exports: [/*LoginMenuComponent, */ RegisterComponent, RecoverPwComponent, SignInComponent, ResetPwComponent]
})
export class AuthModule { }

