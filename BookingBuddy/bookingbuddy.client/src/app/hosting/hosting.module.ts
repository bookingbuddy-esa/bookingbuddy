import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FullCalendarModule } from '@fullcalendar/angular';
import { BrowserModule } from '@angular/platform-browser';
import { CalendarComponent } from './calendar/calendar.component';
import { HostingService } from './hosting.service';
import { AppRoutingModule } from '../app-routing.module';
//import { RouterModule } from '@angular/router';
// import { HomepagePropertyComponent } from './calendar/calendar-property/calendar-property.component';


@NgModule({
  declarations: [
    CalendarComponent /*, HomepagePropertyComponent*/
  ],
  imports: [
    CommonModule, BrowserModule,
    FullCalendarModule, AppRoutingModule
  ],
  providers: [HostingService],
  bootstrap: [CalendarComponent]
})
export class HostingModule { }
