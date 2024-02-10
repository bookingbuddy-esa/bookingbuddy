import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators, AbstractControl, FormArray } from "@angular/forms";
import { Router } from '@angular/router';

import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject, catchError, map, of } from 'rxjs';
import { CheckboxOptions } from '../../models/checkboxes';



@Component({
  selector: 'app-property-ad-create',
  templateUrl: './property-ad-create.component.html',
  styleUrl: './property-ad-create.component.css'
})
export class PropertyAdCreateComponent {
  comodidades = Object.values(CheckboxOptions);
  comodidadesSelecionadas: CheckboxOptions[] = [];
  errors: string[] = [];
  createPropertyAdForm!: FormGroup;
  createPropertyFailed: boolean;

  constructor(private http: HttpClient, private formBuilder: FormBuilder, private router: Router) {
    this.errors = [];
    this.createPropertyFailed = false;
    this.createPropertyAdForm = this.formBuilder.group({
      name: [''],
      location: [''],
      pricePerNight: [''],
      landlordId: [''],
      description: [''],
      imagesUrl: ['']
    });
  }

  atualizarSelecionadas(comodidade: CheckboxOptions) {
    if (this.comodidadesSelecionadas.includes(comodidade)) {
      this.comodidadesSelecionadas = this.comodidadesSelecionadas.filter(c => c !== comodidade);
    } else {
      this.comodidadesSelecionadas.push(comodidade);
    }
  }

  public create(_: any) {
    this.createPropertyFailed = false;
    this.errors = [];

    const newProperty = {
      name: this.createPropertyAdForm.get('name')?.value,
      location: this.createPropertyAdForm.get('location')?.value,
      pricePerNight: this.createPropertyAdForm.get('pricePerNight')?.value,
      landlordId: "um", // TODO: hardcoded, meter dinamico
      description: this.createPropertyAdForm.get('description')?.value,
      imagesUrl: ["test.png"], // TODO: hardcoded, meter dinamico
      amenityIds: this.comodidadesSelecionadas.map(comodidade => Object.values(CheckboxOptions).indexOf(comodidade).toString())
    };

    // TODO: passar isto para um serviço
    this.http.post('api/properties/create', newProperty).subscribe(
      (response) => {
        console.log("RES:", response);
      },
      (error) => {
        console.log("ERR:", error);
        this.createPropertyFailed = true;
        this.errors.push(error.error.message);
      }
    );
  }
}
