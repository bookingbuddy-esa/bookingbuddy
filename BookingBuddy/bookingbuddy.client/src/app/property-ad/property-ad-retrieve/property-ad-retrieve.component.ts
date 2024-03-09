import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Property } from '../../models/property';

import { Injectable } from '@angular/core';
import { AuthorizeService } from "../../auth/authorize.service";
import { PropertyAdService } from '../property-ad.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {AmenitiesHelper} from "../../models/amenityEnum";
import { AppComponent } from '../../app.component';

import { UserInfo } from "../../auth/authorize.dto";
import { Router } from '@angular/router';
import { MatCalendarCellClassFunction, MatDatepickerInputEvent } from '@angular/material/datepicker';

@Component({
  selector: 'app-property-ad-retrieve',
  templateUrl: './property-ad-retrieve.component.html',
  styleUrl: './property-ad-retrieve.component.css'
})

@Injectable({
  providedIn: 'root'
})


export class PropertyAdRetrieveComponent implements OnInit {
  property: Property | undefined;
  reservarPropriedadeForm!: FormGroup;
  reservarPropriedadeFailed: boolean;
  errors: string[];
  signedIn: boolean = false;
  isPropertyInFavorites: boolean = false;
  blockedDates: Date[] = [];
  discounts: Date[] = [];
  checkInDate: Date | undefined;
  checkOutDate: Date | undefined;
  protected readonly AmenitiesHelper = AmenitiesHelper;
  protected isLandlord: boolean = false;

  protected user: UserInfo | undefined;

  constructor(private appComponent: AppComponent, private propertyService: PropertyAdService, private route: ActivatedRoute, private formBuilder: FormBuilder, private authService: AuthorizeService) {
    this.appComponent.showChat = true;
    this.errors = [];

    this.reservarPropriedadeFailed = false;
    this.reservarPropriedadeForm = this.formBuilder.group({
      checkIn: ['', Validators.required],
      checkOut: ['', Validators.required],
      numHospedes: ['1', Validators.required]
    });

    this.authService.isSignedIn().forEach(
      isSignedIn => {
        this.signedIn = isSignedIn;
        this.checkPropertyIsFavorite();
        if (isSignedIn) {
          this.authService.user().forEach(user => {
            this.user = user
            if (user.roles.includes('landlord') || user.roles.includes('admin')) {
              this.isLandlord = true;
            }
          });
        }
      });
  }

  ngOnInit() {

    this.propertyService.getProperty(this.route.snapshot.params['id']).forEach(
      response => {
        if (response) {
          console.log(response);
          this.property = response as Property;
          this.loadDiscounts();
          this.loadBlockedDates();
        }
      }).catch(
        error => {
          // TODO return error message
        }
    );
    
  }

  loadBlockedDates() {

    if (this.property) {
      this.propertyService.getPropertyBlockedDates(this.property.propertyId)
        .forEach(
          (dateRanges: any[]) => {
            console.log("Todos os intervalos de datas obtidos:", dateRanges);

            // Processa os intervalos de datas
            this.blockedDates = [];

            dateRanges.forEach(dateRange => {
              const startDate = new Date(dateRange.start);
              const endDate = new Date(dateRange.end);

              // Adiciona todas as datas entre startDate e endDate (inclusive)
              const currentDate = new Date(startDate);
              while (currentDate <= endDate) {
                this.blockedDates.push(new Date(currentDate));
                currentDate.setDate(currentDate.getDate() + 1);
              }
            });

            // Log das datas bloqueadas
            console.log("DATAS BLOQUEADAS: ", this.blockedDates);
          }).catch(error => {
            console.error('Erro ao carregar intervalos de datas bloqueadas:', error);
          }
        );
    }
  }

  loadDiscounts() {
    if (this.property) {
      this.propertyService.getPropertyDiscounts(this.property.propertyId)
        .forEach(
          (dateRanges: any[]) => {
            console.log("Todos os intervalos de DESCONTOS obtidos:", dateRanges);

            this.discounts = [];

            dateRanges.forEach(dateRange => {
              const startDate = new Date(dateRange.startDate);
              const endDate = new Date(dateRange.endDate);

              const currentDate = new Date(startDate);
              while (currentDate <= endDate) {
                this.discounts.push(new Date(currentDate));
                currentDate.setDate(currentDate.getDate() + 1);
              }
            });

            console.log("DATAS COM DESCONTO: ", this.discounts);
          }).catch(error => {
            console.error('Erro ao carregar intervalos de datas bloqueadas:', error);
          }
          );
    }
  }

  discountClass: MatCalendarCellClassFunction<Date> = (cellDate, view) => {
    if (this.discounts.some(discountDate => this.isSameDay(cellDate, discountDate))) {
      return 'discount-date-class';
    }

    return '';
  };

  dateFilter = (date: Date | null): boolean => {
    if (!date) {
      return false;
    }

    return !this.blockedDates.some(blockedDate => this.isSameDay(date, blockedDate));
  };


  private isSameDay(date1: Date, date2: Date): boolean {
    return date1.getFullYear() === date2.getFullYear() &&
      date1.getMonth() === date2.getMonth() &&
      date1.getDate() === date2.getDate();
  }

  onDateChange(event: MatDatepickerInputEvent<Date>, type: 'start' | 'end'): void {
    if (type === 'start' && event.value) {
      this.checkInDate = event.value;
      console.log(this.checkInDate);
    } else if (event.value){        
      this.checkOutDate = event.value;
      console.log(this.checkOutDate);
    }
  }

  calcularDiferencaDias(): number {
    const checkInDateString: string = this.reservarPropriedadeForm.get('checkIn')?.value;
    const checkOutDateString: string = this.reservarPropriedadeForm.get('checkOut')?.value;
    const checkInDate: Date = new Date(checkInDateString);
    const checkOutDate: Date = new Date(checkOutDateString);

    if(!this.checkInDate || !this.checkOutDate|| this.checkOutDate <= this.checkInDate) {
      return 1;
    }

    const diferencaMilissegundos: number = this.checkOutDate.getTime() - this.checkInDate.getTime();
    const diferencaDias: number = diferencaMilissegundos / (1000 * 60 * 60 * 24);

    return diferencaDias;
  }

  addToFavorites() {
    if (this.property) {
      this.propertyService.addToFavorites(this.property?.propertyId).forEach(
        response => {
          if (response) {
            this.isPropertyInFavorites = true;
          }
        }).catch(
          error => {
            console.error('Erro ao adicionar ao favoritos:', error);
          }
        );
    }
  }

  removeFromFavorites() {
    if (this.property) {
      this.propertyService.removeFromFavorites(this.property?.propertyId).forEach(
        response => {
          if (response) {
            this.isPropertyInFavorites = false;
          }
        }).catch(
          error => {
            console.error('Erro ao adicionar ao favoritos:', error);
          }
        );
    }
  }
  
  checkPropertyIsFavorite() {
    if (this.signedIn) {
      this.propertyService.isPropertyInFavorites(this.route.snapshot.params['id']).forEach(
        result => {
          this.isPropertyInFavorites = result;
        }).catch(
          error => {
          console.error('Erro ao verificar se a propriedade está nos favoritos:', error);
        }
      );
    }
  }


  public reservar(_: any) {
    this.reservarPropriedadeFailed = false;

    const checkInDate: Date = new Date(this.reservarPropriedadeForm.get('checkIn')?.value);
    const checkOutDate: Date = new Date(this.reservarPropriedadeForm.get('checkOut')?.value);

    if(checkOutDate <= checkInDate){
      this.errors.push("A data de check-out não pode ser igual ou menor do que a data de check-in.");
      return;
    }

    // TODO: reservar a propriedade
  }
  
}
