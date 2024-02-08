import { Component } from '@angular/core';
import { CalendarOptions } from '@fullcalendar/core';
import  dayGridPlugin  from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrl: './calendar.component.css'
})
export class CalendarComponent {
  calendarOptions: any;
  selectedStartDate: string | null = null;
  selectedEndDate: string | null = null;
  constructor() {
    this.calendarOptions = {
      plugins: [dayGridPlugin, interactionPlugin],
      dateClick: this.handleDateClick.bind(this),
    };
  }


  handleDateClick(info: any) {
    if (this.selectedStartDate) {
      this.selectedEndDate = info.dateStr;
      if (this.selectedEndDate) {
        const endDate = new Date(this.selectedEndDate);
        endDate.setDate(endDate.getDate() + 1);

        console.log('Destacar datas entre', this.selectedStartDate, 'e', this.selectedEndDate);

        // Adicione as datas no intervalo selecionado ao objeto events
        this.calendarOptions.events = [{
          start: this.selectedStartDate,
          end: endDate.toISOString().split('T')[0],
          rendering: 'background', // Define o estilo de destaque
          color: 'rgba(0, 123, 255, 0.3)' // Adapte a cor conforme necessário
        }];

        this.selectedStartDate = null;
      }
    } else {
      this.selectedStartDate = info.dateStr;
    }
  }

}
