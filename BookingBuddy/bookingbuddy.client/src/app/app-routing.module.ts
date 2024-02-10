import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomepageComponent} from './homepage/homepage.component';
import {PropertyAdCreateComponent} from './property-ad/property-ad-create/property-ad-create.component';
import {NotfoundComponent} from "./auxiliary/notfound/notfound.component";
import {UnauthorizedComponent} from "./auxiliary/unauthorized/unauthorized.component";
import {BadRequestComponent} from "./auxiliary/bad-request/bad-request.component";
import {CalendarComponent} from "./hosting/calendar/calendar.component";

const routes: Routes = [
  {path: '', component: HomepageComponent, pathMatch: 'full'},
  {path: 'property-ad-create', component: PropertyAdCreateComponent},
  {path: 'forbidden', component: UnauthorizedComponent},
  {path: 'bad-request', component: BadRequestComponent},
  {path: '*', component: NotfoundComponent},
  { path: 'calendar', component: CalendarComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
