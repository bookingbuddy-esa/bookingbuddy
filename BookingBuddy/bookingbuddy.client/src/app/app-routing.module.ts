import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './auth/register/register.component';
import { RecoverPwComponent } from './auth/recover-pw/recover-pw.component';
import { SignInComponent } from './auth/signin/signin.component';
import { HomepageComponent } from './homepage/homepage.component';
import { PropertyAdCreateComponent } from './property-ad/property-ad-create/property-ad-create.component';
import { PropertyAdRetrieveComponent } from './property-ad/property-ad-retrieve/property-ad-retrieve.component';

const routes: Routes = [
  //{ path: 'register', component: RegisterComponent },
  //{ path: 'signin', component: SignInComponent },
  { path: 'property-ad-create', component: PropertyAdCreateComponent },
  { path: 'property-ad', component: PropertyAdRetrieveComponent },
  { path: '', component: HomepageComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
