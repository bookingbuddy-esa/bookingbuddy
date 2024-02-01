import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './auth/register/register.component';
import { RecoverPwComponent } from './auth/recover-pw/recover-pw.component';
import { SignInComponent } from './auth/signin/signin.component';

const routes: Routes = [
  //{ path: 'register', component: RegisterComponent },
  //{ path: 'signin', component: SignInComponent },
  { path: '', component: RegisterComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
