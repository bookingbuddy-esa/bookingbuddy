import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FullCalendarModule } from '@fullcalendar/angular';
import { BrowserModule } from '@angular/platform-browser';
import { CalendarComponent } from './calendar/calendar.component';
import { HostingService } from './hosting.service';
import { AppRoutingModule } from '../app-routing.module';
import { MatSliderModule } from '@angular/material/slider';
import { HomepagePropertyComponent } from './calendar/homepage-property/homepage-property.component';
import { PropertyPromoteComponent } from './property-promote/property-promote.component';
import { SidePropertiesComponent } from './property-promote/side-properties/side-properties.component';
import { ReactiveFormsModule } from '@angular/forms';
import { CalendarPopupComponent } from './calendar/calendar-popup/calendar-popup.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { PaymentComponent } from '../payment/payment.component';
import { PropertyPerformanceComponent } from './property-performance/property-performance.component';
import {AuxiliaryModule} from "../auxiliary/auxiliary.module";
import { HostingBookingComponent } from './hosting-booking/hosting-booking.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    CalendarComponent, HomepagePropertyComponent, PropertyPromoteComponent, SidePropertiesComponent,
    CalendarPopupComponent, PropertyPerformanceComponent, HostingBookingComponent
  ],
    imports: [
        CommonModule, BrowserModule, FormsModule,
        FullCalendarModule, AppRoutingModule,
        ReactiveFormsModule, MatDialogModule, MatIconModule, MatSliderModule, PaymentComponent, AuxiliaryModule
    ],
  providers: [HostingService],
  bootstrap: [CalendarComponent]
})
export class HostingModule { }
