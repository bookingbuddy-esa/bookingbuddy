import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { Host, NgModule } from '@angular/core';
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
import { HomepagePropertyComponent } from './homepage/homepage-property/homepage-property.component';
import {NgOptimizedImage} from "@angular/common";
import { PropertyAdCreateComponent } from './property-ad/property-ad-create/property-ad-create.component';
import { HostingModule } from './hosting/hosting.module';
import { PropertyAdRetrieveComponent } from './property-ad/property-ad-retrieve/property-ad-retrieve.component';
import {GoogleMap, MapMarker} from "@angular/google-maps";
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { LocationStepComponent } from './property-ad/property-ad-create/location-step/location-step.component';
import { InitialStepComponent } from './property-ad/property-ad-create/initial-step/initial-step.component';
import { AmenitiesStepComponent } from './property-ad/property-ad-create/amenities-step/amenities-step.component';
import { AmenityComponent } from './property-ad/property-ad-create/amenities-step/amenity/amenity.component';
import { AdInfoStepComponent } from './property-ad/property-ad-create/ad-info-step/ad-info-step.component';
import { PhotosStepComponent } from './property-ad/property-ad-create/photos-step/photos-step.component';
import { ChatComponent } from './chat/chat.component';
import { FavoriteSidebarComponent } from './favorite-sidebar/favorite-sidebar.component';
import { MatIconModule } from '@angular/material/icon';
import { FavoritebarPropertyComponent } from './favorite-sidebar/favoritebar-property/favoritebar-property.component';
import { ProfileComponent } from './profile/profile.component';
import { BookingComponent } from './booking/booking.component';

@NgModule({
  declarations: [
    AppComponent,
    HomepageComponent,
    MenuComponent,
    HomepagePropertyComponent,
    PropertyAdCreateComponent,
    PropertyAdRetrieveComponent,
    LocationStepComponent,
    InitialStepComponent,
    AmenitiesStepComponent,
    AmenityComponent,
    AdInfoStepComponent,
    PhotosStepComponent,
    ChatComponent,
    FavoriteSidebarComponent,
    ProfileComponent,
    BookingComponent, FavoritebarPropertyComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, FormsModule,
    ReactiveFormsModule, AuthModule,
    AuxiliaryModule, NgOptimizedImage,
    HostingModule, GoogleMap, MapMarker,
    MatIconModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
    AuthGuard,
    AuthorizeService,
    provideAnimationsAsync()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
