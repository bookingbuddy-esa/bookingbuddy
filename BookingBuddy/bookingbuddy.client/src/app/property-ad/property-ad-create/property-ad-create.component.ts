import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators, AbstractControl, FormArray } from "@angular/forms";
import { Router } from '@angular/router';

import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject, catchError, map, of } from 'rxjs';
import { CheckboxOptions } from '../../models/checkboxes';
import { PropertyAdService } from '../property-ad.service';

@Component({
  selector: 'app-property-ad-create',
  templateUrl: './property-ad-create.component.html',
  styleUrl: './property-ad-create.component.css'
})
export class PropertyAdCreateComponent {
  selectedFile: File | undefined;
  comodidades = Object.values(CheckboxOptions);
  comodidadesSelecionadas: CheckboxOptions[] = [];
  errors: string[] = [];
  createPropertyAdForm!: FormGroup;
  createPropertyFailed: boolean;
  checkboxOptions = CheckboxOptions;

  constructor(private formBuilder: FormBuilder, private router: Router, private propertyService: PropertyAdService) {
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
  //função para guardar as imagens num array // TODO:Restrições de tipo de ficheiro
  onFileSelected(event: any): void {
    const inputElement = event.target;
    if (inputElement.files.length > 0) {
      this.selectedFile = inputElement.files[0];
      console.log('Ficheiro selecionado:', this.selectedFile);
    }
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
    const images = this.selectedFile ? this.selectedFile.name : '';

    const newProperty = {
      name: this.createPropertyAdForm.get('name')?.value,
      location: this.createPropertyAdForm.get('location')?.value,
      pricePerNight: this.createPropertyAdForm.get('pricePerNight')?.value,
      description: this.createPropertyAdForm.get('description')?.value,
      imagesUrl: [images], //["test.png"], // TODO: hardcoded, meter dinamico
      amenityIds: this.comodidadesSelecionadas.map(comodidade => Object.values(CheckboxOptions).indexOf(comodidade).toString())
    };

    this.propertyService.createPropertyAd(newProperty.name, newProperty.location, newProperty.pricePerNight,  newProperty.description, newProperty.imagesUrl, newProperty.amenityIds).forEach(
      response => {
        if (response) {
          console.log(response);
        }
      }).catch(
        error => {
          console.error(error);
        }
      );
  }
}
