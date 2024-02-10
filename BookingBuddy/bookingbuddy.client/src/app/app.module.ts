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
import { PropertyAdCreateComponent } from './property-ad/property-ad-create/property-ad-create.component';
import { PropertyAdRetrieveComponent } from './property-ad/property-ad-retrieve/property-ad-retrieve.component';


@NgModule({
  declarations: [
    AppComponent,
    HomepageComponent,
    MenuComponent,
    PropertyAdCreateComponent,
    PropertyAdRetrieveComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, FormsModule,
    ReactiveFormsModule, AuthModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    AuthGuard,
    AuthorizeService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
