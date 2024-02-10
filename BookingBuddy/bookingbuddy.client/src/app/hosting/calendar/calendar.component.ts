import { Component, OnInit, ViewChild } from '@angular/core';
import { CalendarOptions } from '@fullcalendar/core';
import { FullCalendarComponent } from '@fullcalendar/angular';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import { HostingService } from '../hosting.service';
import { AuthorizeService } from "../../auth/authorize.service";
import { Router } from '@angular/router';

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
  constructor(private hostingService: HostingService, private authService: AuthorizeService, private router: Router) {
    this.authService.isSignedIn().forEach(
      isSignedIn => {
        this.signedIn = isSignedIn;
        if (!this.signedIn) { this.router.navigateByUrl('signin'); }
      });
  }

  ngOnInit(): void {
    this.calendarOptions = {
      plugins: [dayGridPlugin, interactionPlugin],
      dateClick: this.handleDateClick.bind(this),
      select: this.handleSelect.bind(this),
      events: this.getEventRanges.bind(this), 
      selectable: true,
      eventClick: this.handleEventClick.bind(this),
      selectOverlap: function (event: { groupId: string; }) {
        return !(event.groupId == "blocked");
      },
    };
  }

  handleDateClick(info: any) {
    //console.log("Data: " + info.dateStr);
  }

  handleEventClick(info: any) {
    if (info.event.groupId == 'blocked') {
      const startDate = new Date(info.event.start);
      const endDate = new Date(info.event.end);
      this.selectedEventId = info.event.id;
      this.selectedStartDate = `${startDate.getFullYear() }-${startDate.getMonth() + 1}-${startDate.getDate() }`;
      this.selectedEndDate = `${startDate.getFullYear() }-${endDate.getMonth() + 1}-${endDate.getDate() }`;
    }
  }

  handleSelect(info: any) {
    this.selectedEventId = null;
    this.selectedStartDate = info.startStr;
    this.selectedEndDate = info.endStr;
    console.log("Selecionado entre " + this.selectedStartDate + " a " + this.selectedEndDate);
  }

  blockSelectedDates() {
    if (this.selectedStartDate && this.selectedEndDate) {
      this.hostingService.blockDates(this.selectedStartDate, this.selectedEndDate).forEach(
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

  async getEventRanges(info: any) {
    try {
      const blockedDates = await this.hostingService.getDateRanges().toPromise();

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
    } catch (error) {
      console.error('Erro ao obter BlockedDates:', error);
      throw error;
    }
  }


}
