import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Property } from '../../models/property';

import { Injectable } from '@angular/core';
import { AuthorizeService } from "../../auth/authorize.service";
import { PropertyAdService } from '../property-ad.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {AmenitiesHelper} from "../../models/amenityEnum";
import { AppComponent } from '../../app.component';

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
  protected readonly AmenitiesHelper = AmenitiesHelper;

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
      });
  }

  ngOnInit() {
    // TODO: obter as datas bloqueadas da propriedade
    this.propertyService.getProperty(this.route.snapshot.params['id']).forEach(
      response => {
        if (response) {
          console.log(response);
          this.property = response as Property;
          this.checkPropertyIsFavorite();
        }
      }).catch(
        error => {
          // TODO return error message
        }
    );
    
  }

  calcularDiferencaDias(): number {
    const checkInDateString: string = this.reservarPropriedadeForm.get('checkIn')?.value;
    const checkOutDateString: string = this.reservarPropriedadeForm.get('checkOut')?.value;
    const checkInDate: Date = new Date(checkInDateString);
    const checkOutDate: Date = new Date(checkOutDateString);

    if(checkInDateString === '' || checkOutDateString === '' || checkOutDate <= checkInDate) {
      return 1;
    }

    const diferencaMilissegundos: number = checkOutDate.getTime() - checkInDate.getTime();
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
    if (this.signedIn && this.property) {
      this.propertyService.isPropertyInFavorites(this.property?.propertyId).subscribe(
        (result) => {
          this.isPropertyInFavorites = result;
        },
        (error) => {
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
