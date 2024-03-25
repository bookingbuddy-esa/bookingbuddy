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
import { ChatComponent } from './chat/chat.component';
import { BookingComponent } from './booking/booking.component';
import { ProfileComponent } from './profile/profile.component';
import {PropertyPerformanceComponent} from './hosting/property-performance/property-performance.component';
import { HostingBookingComponent } from './hosting/hosting-booking/hosting-booking.component';
import { TransactionHandlerComponent } from './payment/transaction-handler/transaction-handler.component';
import { GroupComponent } from './group/group.component';
import { GroupCreateComponent } from './group/group-create/group-create.component';
import { FaqPageComponent } from './faq-page/faq-page.component';
import { AboutComponent } from './about/about.component';

const routes: Routes = [
  {path: '', component: HomepageComponent},
  {path: 'hosting/create', component: PropertyAdCreateComponent, canActivate: [AuthGuard]},
  {path: 'hosting/calendar', component: CalendarComponent, canActivate: [AuthGuard, LandlordRoleGuardService]},
  {path: 'hosting/promote', component: PropertyPromoteComponent, canActivate: [AuthGuard, LandlordRoleGuardService]},
  {path: 'hosting/performance', component: PropertyPerformanceComponent, canActivate: [AuthGuard, LandlordRoleGuardService] },
  {path: 'hosting/bookings', component: HostingBookingComponent, canActivate: [AuthGuard, LandlordRoleGuardService] },
  {path: 'property/:id', component: PropertyAdRetrieveComponent},
  {path: 'transaction-handler', component: TransactionHandlerComponent, canActivate: [AuthGuard]},
  {path: 'groups', component: GroupComponent, canActivate: [AuthGuard]},
  {path: 'group-booking', component: GroupCreateComponent, canActivate: [AuthGuard]},
  {path: 'bookings', component: BookingComponent, canActivate: [AuthGuard]},
  {path: 'profile', component: ProfileComponent, canActivate: [AuthGuard]},
  {path: 'profile/:id', component: ProfileComponent},
  //{path: 'chat', component: ChatComponent/*, canActivate: [AuthGuard] */},
  { path: 'unauthorized', component: UnauthorizedComponent },
  { path: 'faq', component: FaqPageComponent },
  { path: 'about', component: AboutComponent },
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
