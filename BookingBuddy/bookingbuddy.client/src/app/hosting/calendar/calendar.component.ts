import { Component, OnInit } from '@angular/core';
import { CalendarOptions } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import { HostingService } from '../hosting.service'; 

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.css']
})
export class CalendarComponent implements OnInit {
  calendarOptions: any;
  selectedStartDate: string | null = null;
  selectedEndDate: string | null = null;

  constructor(private hostingService: HostingService) { }

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
    console.log("Data: " + info.dateStr);
  }

  handleEventClick(info: any) {
    if (info.event.groupId == 'blocked') {
      this.selectedStartDate = info.event.start;
      this.selectedEndDate = info.event.start;
    }
  }

  handleSelect(info: any) {
    this.selectedStartDate = info.startStr;
    this.selectedEndDate = info.endStr;
    console.log("Selecionado entre " + this.selectedStartDate + " a " + this.selectedEndDate);
  }

  blockSelectedDates() {
    if (this.selectedStartDate && this.selectedEndDate) {
      this.hostingService.blockDates(this.selectedStartDate, this.selectedEndDate).forEach(
        response => {
          if (response) {
            
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
