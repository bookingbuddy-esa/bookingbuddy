import { Component } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Inject } from '@angular/core';
import { Property } from '../../../models/property';
import { HostingService } from '../../hosting.service';

@Component({
  selector: 'app-calendar-popup',
  templateUrl: './calendar-popup.component.html',
  styleUrls: ['./calendar-popup.component.css']
})
export class CalendarPopupComponent {
  selectedStartDate: string | null = null;
  selectedEndDate: string | null = null;
  currentProperty: Property | null = null;
  startDateOutput: string | null = null;
  endDateOutput: string | null = null;
  eventId: number | null = null;
  discountValue: number = 10;
  isDiscountEvent: boolean | null = null;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private hostingService: HostingService,
    private dialogRef: MatDialogRef<CalendarPopupComponent>
  ) { }

  ngOnInit(): void {
    this.selectedStartDate = this.data.selectedStartDate;
    this.selectedEndDate = this.data.selectedEndDate;
    this.currentProperty = this.data.property;
    this.eventId = this.data.eventId;
    this.startDateOutput = this.formatDate(this.selectedStartDate);
    this.endDateOutput = this.formatDate(this.selectedEndDate);
    this.isDiscountEvent = this.data.isDiscountEvent;
  }

  blockSelectedDates() {
    if (this.selectedStartDate && this.selectedEndDate && this.currentProperty) {
      const endDate = new Date(this.selectedEndDate);
      endDate.setDate(endDate.getDate() + 1);
      const adjustedEndDate = endDate.toISOString().split('T')[0];
      this.hostingService.blockDates(this.selectedStartDate, this.selectedEndDate, this.currentProperty.propertyId).forEach(
        response => {
          if (response) {
            this.dialogRef.close('calendarUpdate');
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

  applyDiscount() {
    if (this.selectedStartDate && this.selectedEndDate && this.currentProperty) {
      const startDate = new Date(this.selectedStartDate);
      const endDate = new Date(this.selectedEndDate);
      endDate.setDate(endDate.getDate());
      const adjustedEndDate = endDate.toISOString().split('T')[0];
      this.hostingService.applyDiscount(this.discountValue,startDate, endDate, this.currentProperty.propertyId).forEach(
        response => {
          if (response) {
            this.dialogRef.close('calendarUpdate');
          }
        }).catch(
          error => {
            console.error('Erro ao bloquear datas:', error);
          }
        );
    } else {
      console.warn('Selecione as datas.');
    }
  }

  removeDiscount() {
    if (this.eventId) {
      this.hostingService.removeDiscount(this.eventId).forEach(
        response => {
          if (response) {
            this.dialogRef.close('calendarUpdate');
          }
        }).catch(
          error => {
            console.error('Erro ao remover desconto:', error);
          }
        );
    }
  }

  formatLabel(value: number): string {
    if (value >= 1000) {
      return Math.round(value / 1000) + 'k';
    }

    return `${value}`;
  }

  unblockDates() {
    if (this.eventId) {
      this.hostingService.unblockDates(this.eventId).forEach(
        response => {
          if (response) {
            this.dialogRef.close('calendarUpdate');
          }
        }).catch(
        error => {
          console.error('Erro ao desbloquear datas:', error);
        }
      );
    }
  }

  private formatDate(date: string | null): string | null {
    if (!date) {
      return null;
    }

    const parts = date.split('-');
    if (parts.length === 3) {

      const yearAux = parseInt(parts[0], 10);
      const monthAux = parseInt(parts[1], 10) - 1;
      const dayAux = parseInt(parts[2], 10);

      const formattedDate = new Date(yearAux, monthAux, dayAux);

      const day = formattedDate.getDate().toString().padStart(2, '0');
      const month = (formattedDate.getMonth() + 1).toString().padStart(2, '0');
      const year = formattedDate.getFullYear();

      return `${day}/${month}/${year}`;
    }

    return null;
  }
}
