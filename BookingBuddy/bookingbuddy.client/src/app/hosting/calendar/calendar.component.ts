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
      events: this.getEventRanges.bind(this), // Chama a função que obtém os eventos
      selectable: true,
      eventClick: this.handleEventClick.bind(this),
      selectOverlap: function (event: { groupId: string; }) {
        return !(event.groupId == "blocked");
      },
    };
  }

  handleDateClick(info: any) {
    console.log("Data: " + info.dateStr);
    /*if (this.selectedStartDate) {
      this.selectedEndDate = info.dateStr;
      if (this.selectedEndDate) {
        const endDate = new Date(this.selectedEndDate);
        endDate.setDate(endDate.getDate() + 1);

        console.log('Destacar datas entre', this.selectedStartDate, 'e', this.selectedEndDate);

        // Adicione as datas no intervalo selecionado ao objeto events
        this.calendarOptions.events = [{
          start: this.selectedStartDate,
          end: endDate.toISOString().split('T')[0],
          rendering: 'background',
          color: 'rgba(0, 123, 255, 0.3)',
        }];

        this.selectedStartDate = null;
      }
    } else {
      this.selectedStartDate = info.dateStr;
    }*/
  }

  handleEventClick(info: any) {
    alert("ID: " + info.event.id);
  }

  handleSelect(info: any) {
    console.log("Selecionado entre " + info.startStr + " a " + info.endStr);

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
