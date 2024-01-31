import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { RecoverPWComponent } from './recover-pw/recover-pw.component';

const routes: Routes = [
  //{ path: 'register', component: RegisterComponent },
  //{ path: 'recoverpassword', component: RecoverPWComponent }
  { path: '', component: RegisterComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
