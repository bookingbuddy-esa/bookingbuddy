import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthModule } from './auth/auth.module';
import { AuthInterceptor } from './auth/authorize.interceptor';
import { AuthGuard } from './auth/authorize.guard';
import { AuthorizeService } from './auth/authorize.service';
import { HomepageComponent } from './homepage/homepage.component';
import { MenuComponent } from './menu/menu.component';
import { AuxiliaryModule } from './auxiliary/auxiliary.module';

@NgModule({
  declarations: [
    AppComponent,
    HomepageComponent,
    MenuComponent,
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, FormsModule,
    ReactiveFormsModule, AuthModule,
    AuxiliaryModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    AuthGuard,
    AuthorizeService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
