import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomepageComponent} from './homepage/homepage.component';
import {PropertyAdCreateComponent} from './property-ad/property-ad-create/property-ad-create.component';
import {PropertyAdRetrieveComponent} from './property-ad/property-ad-retrieve/property-ad-retrieve.component';
import {NotfoundComponent} from "./auxiliary/notfound/notfound.component";
import {UnauthorizedComponent} from "./auxiliary/unauthorized/unauthorized.component";
import {BadRequestComponent} from "./auxiliary/bad-request/bad-request.component";
import {CalendarComponent} from "./hosting/calendar/calendar.component";
import {AuthModule} from "./auth/auth.module";
import {PropertyPromoteComponent} from './hosting/property-promote/property-promote.component';
import {ForbiddenComponent} from "./auxiliary/forbidden/forbidden.component";
import {AuthGuard} from "./auth/authorize.guard";
import {LandlordRoleGuardService} from "./auth/role-guard/landlord-role-guard.service";

const routes: Routes = [
  {path: '', component: HomepageComponent},
  {path: 'property/:id', component: PropertyAdRetrieveComponent},
  {path: 'properties/create', component: PropertyAdCreateComponent, canActivate: [AuthGuard]},
  {path: 'properties/calendar', component: CalendarComponent, canActivate: [AuthGuard, LandlordRoleGuardService]},
  {path: 'properties/promote', component: PropertyPromoteComponent, canActivate: [AuthGuard, LandlordRoleGuardService]},
  {path: 'unauthorized', component: UnauthorizedComponent},
  {path: 'bad-request', component: BadRequestComponent},
  {path: 'forbidden', component: ForbiddenComponent},
  {path: '**', component: NotfoundComponent}
];

@NgModule({
  imports: [AuthModule, RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
