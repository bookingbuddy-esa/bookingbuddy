import { Component, OnInit, ViewChild } from '@angular/core';
import { CalendarOptions } from '@fullcalendar/core';
import { FullCalendarComponent } from '@fullcalendar/angular';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import { HostingService } from '../hosting.service';
import { AuthorizeService } from "../../auth/authorize.service";
import { Router } from '@angular/router';
import { Property } from '../../models/property';
import { UserInfo } from '../../auth/authorize.dto';
import { MatDialog } from '@angular/material/dialog';
import { CalendarPopupComponent } from './calendar-popup/calendar-popup.component';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.css']
})
export class CalendarComponent implements OnInit {
  @ViewChild(FullCalendarComponent, { static: false }) fullcalendar!: FullCalendarComponent;
  calendarOptions: any;
  selectedStartDate: string | null = null;
  selectedEndDate: string | null = null;
  selectedEventId: number | null = null;
  signedIn: boolean = false;
  property_list: Property[] = [];
  user: UserInfo | undefined;
  currentProperty: Property | null = null;

  constructor(private hostingService: HostingService, private authService: AuthorizeService, private router: Router, private dialog: MatDialog) {}

  ngOnInit(): void {
    this.authService.isSignedIn().forEach(isSignedIn => {
      this.signedIn = isSignedIn;
      if (this.signedIn) {
        this.authService.user().forEach(async user => {
          this.user = user;

          await this.loadUserProperties();

        });
      } else {
        this.router.navigateByUrl('signin');
      }
    });
    this.initializeCalendar();
  }

  private initializeCalendar() {
    this.calendarOptions = {
      plugins: [dayGridPlugin, interactionPlugin],
      select: this.handleSelect.bind(this),
      events: this.getEventRanges.bind(this),
      selectable: true,
      eventClick: this.handleEventClick.bind(this),
      selectOverlap: function (event: { groupId: string; }) {
        return !(event.groupId == "blocked");
      },
    };
  }

  openPopup() {
    const dialogRef = this.dialog.open(CalendarPopupComponent, {
      data: {
        selectedStartDate: this.selectedStartDate,
        selectedEndDate: this.selectedEndDate,
        property: this.currentProperty,
        eventId: this.selectedEventId
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 'calendarUpdate') {
        this.fullcalendar.getApi().refetchEvents();
      }
    });
  }

  handleEventClick(info: any) {
    if (info.event.groupId == 'blocked') {
      const startDate = new Date(info.event.start);
      const endDate = new Date(info.event.end);
      this.selectedEventId = info.event.id;
      this.selectedStartDate = `${startDate.getFullYear()}-${startDate.getMonth() + 1}-${startDate.getDate()}`;
      this.selectedEndDate = `${startDate.getFullYear()}-${endDate.getMonth() + 1}-${endDate.getDate() - 1}`;
      this.openPopup();
    }
  }

  handleSelect(info: any) {
    this.selectedEventId = null;
    this.selectedStartDate = info.startStr;
    const endDate = new Date(info.endStr);
    endDate.setDate(endDate.getDate() - 1);

    this.selectedEndDate = endDate.toISOString().split('T')[0];
    this.openPopup();
  }

  setCurrentProperty(property: Property) {
    this.currentProperty = property;
    this.fullcalendar.getApi().refetchEvents(); 
  }

  async getEventRanges(info: any) {
    try {
      if (this.currentProperty) {
        const blockedDates = await this.hostingService.getPropertyBlockedDates(this.currentProperty?.propertyId).toPromise();
        if (!blockedDates) {
          return [];
        }
        const events = blockedDates.map((blocked) => ({
          groupId: "blocked",
          id: blocked.id,
          start: blocked.start,
          end: this.adjustEndDate(blocked.end),
          display: 'background',
          color: 'red',
        }));


        return events;
      }
      return [];
    } catch (error) {
      return [];
    }
  }

  private adjustEndDate(endDate: string): string {
    const parts = endDate.split('-');
    if (parts.length === 3) {
      const yearAux = parseInt(parts[0], 10);
      const monthAux = parseInt(parts[1], 10) - 1;
      const dayAux = parseInt(parts[2], 10);

      const adjustedEndDate = new Date(yearAux, monthAux, dayAux);

      adjustedEndDate.setDate(adjustedEndDate.getDate() + 1);

      const day = adjustedEndDate.getDate().toString().padStart(2, '0');
      const month = (adjustedEndDate.getMonth() + 1).toString().padStart(2, '0');
      const year = adjustedEndDate.getFullYear();

      return `${year}-${month}-${day}`;
    }

    return endDate;
  }

  private loadUserProperties() {
    if (this.user) {
      this.hostingService.getPropertiesByUserId(this.user?.userId).subscribe(
        properties => {
          this.property_list = properties;
          this.currentProperty = this.property_list[0];
          this.fullcalendar.getApi().refetchEvents();
        },
        error => {
          console.error('Erro ao obter propriedades do usuário:', error);
        }
      );
    }
  }


}
