import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomepageComponent} from './homepage/homepage.component';
import {PropertyAdCreateComponent} from './property-ad/property-ad-create/property-ad-create.component';
import {PropertyAdRetrieveComponent} from './property-ad/property-ad-retrieve/property-ad-retrieve.component';
import {NotfoundComponent} from "./auxiliary/notfound/notfound.component";
import {UnauthorizedComponent} from "./auxiliary/unauthorized/unauthorized.component";
import {BadRequestComponent} from "./auxiliary/bad-request/bad-request.component";

const routes: Routes = [
  {path: '', component: HomepageComponent, pathMatch: 'full'},
  {path: 'property-ad-create', component: PropertyAdCreateComponent},
  {path: 'property-ad/:id', component: PropertyAdRetrieveComponent },
  {path: 'forbidden', component: UnauthorizedComponent},
  {path: 'bad-request', component: BadRequestComponent},
  {path: '*', component: NotfoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
