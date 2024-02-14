import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FullCalendarModule } from '@fullcalendar/angular';
import { BrowserModule } from '@angular/platform-browser';
import { CalendarComponent } from './calendar/calendar.component';
import { HostingService } from './hosting.service';
import { AppRoutingModule } from '../app-routing.module';
//import { RouterModule } from '@angular/router';
import { HomepagePropertyComponent } from './calendar/homepage-property/homepage-property.component';
import { PropertyPromoteComponent } from './property-promote/property-promote.component';
import { SidePropertiesComponent } from './property-promote/side-properties/side-properties.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    CalendarComponent , HomepagePropertyComponent, PropertyPromoteComponent, SidePropertiesComponent
  ],
  imports: [
    CommonModule, BrowserModule,
    FullCalendarModule, AppRoutingModule,
    ReactiveFormsModule
  ],
  providers: [HostingService],
  bootstrap: [CalendarComponent]
})
export class HostingModule { }
