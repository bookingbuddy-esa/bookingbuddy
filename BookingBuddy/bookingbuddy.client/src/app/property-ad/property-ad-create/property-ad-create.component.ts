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

  checkboxOptions = CheckboxOptions;

  /**
   * Construtor da classe SignInComponent.
   * @param formBuilder Construtor de formulários do Angular.
   * @param router Router do Angular.
   */
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

  public onChange(_: any): void {
    console.log(_.target.name);
  }

  public create(_: any): void {
    const teste = this.comodidadesSelecionadas.map(comodidade => Object.values(CheckboxOptions).indexOf(comodidade));
    this.createPropertyFailed = false;
    this.errors = [];

    const name = this.createPropertyAdForm.get('name')?.value;
    const location = this.createPropertyAdForm.get('location')?.value;
    const pricePerNight = this.createPropertyAdForm.get('pricePerNight')?.value;
    const landlordId = this.createPropertyAdForm.get('landlordId')?.value;
    const description = this.createPropertyAdForm.get('description')?.value;
    const imagesUrl = this.createPropertyAdForm.get('imagesUrl')?.value;

    
    // Adicionar o landlordId ao objeto de dados
    const landlord = {
      ...this.createPropertyAdForm.value,
      landlordId: this.createPropertyAdForm.get('landlordId')?.value
    };
    // Fazer POST com os dados atualizados
    this.http.post<any>('/api/properties/create', landlord);


    console.log(this.createPropertyAdForm.value);
   /* console.log(checkboxesSelecionadas);*/
    //return this.http.post<any>('/api/properties/create', this.createPropertyAdForm.value)
  }
}
