import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {LoaderComponent} from './loader/loader.component';
import {BadRequestComponent} from './bad-request/bad-request.component';
import {NotfoundComponent} from './notfound/notfound.component';
import {RouterLink} from "@angular/router";
import {UnauthorizedComponent} from './unauthorized/unauthorized.component';
import {ForbiddenComponent} from './forbidden/forbidden.component';


@NgModule({
  declarations: [
    LoaderComponent,
    BadRequestComponent,
    NotfoundComponent,
    UnauthorizedComponent,
    BadRequestComponent,
    ForbiddenComponent
  ],
  imports: [
    CommonModule,
    RouterLink
  ],
  exports: [
    LoaderComponent
  ]
})
export class AuxiliaryModule {
}
