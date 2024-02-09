import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FullCalendarModule } from '@fullcalendar/angular';
import { BrowserModule } from '@angular/platform-browser';
import { CalendarComponent } from './calendar/calendar.component';
import { HostingService } from './hosting.service';
//import { RouterModule } from '@angular/router';


@NgModule({
  declarations: [
    CalendarComponent
  ],
  imports: [
    CommonModule, BrowserModule,
    FullCalendarModule
  ],
  providers: [HostingService],
  bootstrap: [CalendarComponent]
})
export class HostingModule { }
