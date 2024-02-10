import { Component, OnInit, ViewChild } from '@angular/core';
import { CalendarOptions } from '@fullcalendar/core';
import { FullCalendarComponent } from '@fullcalendar/angular';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import { HostingService } from '../hosting.service';
import { AuthorizeService } from "../../auth/authorize.service";
import { Router } from '@angular/router';
import { Property } from '../../models/property';

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

    const testPhotosUrl = [
      "https://www.usatoday.com/gcdn/-mm-/05b227ad5b8ad4e9dcb53af4f31d7fbdb7fa901b/c=0-64-2119-1259/local/-/media/USATODAY/USATODAY/2014/08/13/1407953244000-177513283.jpg",
      "https://png.pngtree.com/thumb_back/fh260/background/20230425/pngtree-living-room-with-window-and-wooden-furniture-image_2514066.jpg",
      "https://media.istockphoto.com/id/119926339/photo/resort-swimming-pool.jpg?s=612x612&w=0&k=20&c=9QtwJC2boq3GFHaeDsKytF4-CavYKQuy1jBD2IRfYKc=",
      "https://upload.wikimedia.org/wikipedia/commons/7/79/Ponta_Negra_Beach_Hotel.jpg",
      "https://digital.ihg.com/is/image/ihg/ihg-lp-refresh-hero-imea-gben-lvp-1440x617"
    ];
    const testLocation = [
      "Atouguia da Baleia, Portugal",
      "Lisboa, Portugal",
      "Porto, Portugal",
      "Funchal, Portugal",
      "Portimão, Portugal"
    ];
    for (let i = 0; i < 3; i++) {
      const number = Math.floor(Math.random() * 5);
      this.property_list.push({
        propertyId: i.toString(),
        landlordId: "landlord",
        name: "Property " + i,
        location: testLocation[number],
        pricePerNight: Math.floor(Math.random() * 1000),
        amenityIds: [],
        imagesUrl: [testPhotosUrl[number]]
      });
    }

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
