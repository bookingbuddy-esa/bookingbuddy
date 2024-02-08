import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './auth/register/register.component';
import { RecoverPwComponent } from './auth/recover-pw/recover-pw.component';
import { SignInComponent } from './auth/signin/signin.component';
import { HomepageComponent } from './homepage/homepage.component';
import { CalendarComponent } from './hosting/calendar/calendar.component';

const routes: Routes = [
  //{ path: 'register', component: RegisterComponent },
  //{ path: 'signin', component: SignInComponent },
  { path: '', component: HomepageComponent },
  { path: 'calendar', component: CalendarComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
