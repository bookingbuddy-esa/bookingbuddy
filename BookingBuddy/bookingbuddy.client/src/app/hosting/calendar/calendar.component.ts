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

  constructor(private hostingService: HostingService, private authService: AuthorizeService, private router: Router) {

  }

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

  handleEventClick(info: any) {
    if (info.event.groupId == 'blocked') {
      const startDate = new Date(info.event.start);
      const endDate = new Date(info.event.end);
      this.selectedEventId = info.event.id;
      this.selectedStartDate = `${startDate.getFullYear()}-${startDate.getMonth() + 1}-${startDate.getDate()}`;
      this.selectedEndDate = `${startDate.getFullYear()}-${endDate.getMonth() + 1}-${endDate.getDate() - 1}`;
    }
  }

  handleSelect(info: any) {
    this.selectedEventId = null;
    this.selectedStartDate = info.startStr;
    const endDate = new Date(info.endStr);
    endDate.setDate(endDate.getDate() - 1);

    this.selectedEndDate = endDate.toISOString().split('T')[0];
    console.log("Selecionado entre " + this.selectedStartDate + " a " + this.selectedEndDate);
  }

  blockSelectedDates() {
    if (this.selectedStartDate && this.selectedEndDate && this.currentProperty) {
      this.hostingService.blockDates(this.selectedStartDate, this.selectedEndDate, this.currentProperty.propertyId).forEach(
        response => {
          if (response) {
            this.fullcalendar.getApi().refetchEvents();
          }
        }).catch(
          error => {
            console.error('Erro ao bloquear datas:', error);
          }
        );
    } else {
      console.warn('Selecione as datas antes de bloquear.');
    }
  }

  unblockDates() {
    if (this.selectedEventId) {
      this.hostingService.unblockDates(this.selectedEventId).subscribe(
        response => {
          if (response) {
            console.log('Datas desbloqueadas com sucesso.');
            this.fullcalendar.getApi().refetchEvents();
          }
        },
        error => {
          console.error('Erro ao desbloquear datas:', error);
        }
      );
    }
  }

  setCurrentProperty(property: Property) {
    this.currentProperty = property;
    this.fullcalendar.getApi().refetchEvents(); 
  }

  async getEventRanges(info: any) {
    try {
      console.log('Current Property:'+ this.currentProperty?.propertyId);
      if (this.currentProperty) {
        const blockedDates = await this.hostingService.getPropertyBlockedDates(this.currentProperty?.propertyId).toPromise();
        console.log('Datas obtidas:'+ blockedDates);
        if (!blockedDates) {
          console.error('DateRanges não está definido.');
          return [];
        }
        const events = blockedDates.map((blocked) => ({
          groupId: "blocked",
          id: blocked.id,
          start: blocked.start,
          end: blocked.end,
          display: 'background',
          color: 'red',
        }));

        console.log('Datas obtidas:', events);

        return events;
      }
      return [];
    } catch (error) {
      console.error('Erro ao obter BlockedDates:', error);
      return [];
    }
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
