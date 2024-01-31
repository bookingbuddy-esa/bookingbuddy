import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterComponent } from './register/register.component';
import { SignInComponent } from './signin/signin.component';
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
        { path: 'signin', component: SignInComponent }
      ]
    )
  ],
  declarations: [/*LoginMenuComponent,*/ RegisterComponent, RecoverPwComponent, SignInComponent],
  exports: [/*LoginMenuComponent, */ RegisterComponent, RecoverPwComponent, SignInComponent]
})
export class AuthModule { }

