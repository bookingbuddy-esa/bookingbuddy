import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Property } from '../../models/property';

import { Injectable } from '@angular/core';

import { PropertyAdService } from '../property-ad.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-property-ad-retrieve',
  templateUrl: './property-ad-retrieve.component.html',
  styleUrl: './property-ad-retrieve.component.css'
})

@Injectable({
  providedIn: 'root'
})

export class PropertyAdRetrieveComponent implements OnInit {
  // get the property ad id from the route
  // retrieve the property ad from the server
  // display the property ad
  // handle the case when the property ad is not found
  // handle the case when the server is not available

  property: Property | undefined;
  reservarPropriedadeForm!: FormGroup;
  reservarPropriedadeFailed: boolean;
  errors: string[];

  constructor(private propertyService: PropertyAdService, private route: ActivatedRoute, private formBuilder: FormBuilder) {
    this.errors = [];
    
    this.reservarPropriedadeFailed = false;
    this.reservarPropriedadeForm = this.formBuilder.group({
      checkIn: ['', Validators.required],
      checkOut: ['', Validators.required],
      numHospedes: ['1', Validators.required]
    });
  }

  ngOnInit() {
    // TODO: obter as datas bloqueadas da propriedade
    this.propertyService.getProperty(this.route.snapshot.params['id']).forEach(
      response => {
        if (response) {
          console.log(response);
          this.property = response as Property;
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
