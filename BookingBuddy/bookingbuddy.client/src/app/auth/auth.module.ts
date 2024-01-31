import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterComponent } from './register/register.component';
import { SigninComponent } from './signin/signin.component';
import { RecoverPwComponent } from './recover-pw/recover-pw.component';

import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';

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
      ]
    )
  ],
  declarations: [/*LoginMenuComponent, SignInComponent,*/ RegisterComponent, RecoverPwComponent],
  exports: [/*LoginMenuComponent, SignInComponent,*/ RegisterComponent, RecoverPwComponent]
})
export class AuthModule { }

