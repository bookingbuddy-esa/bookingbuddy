import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from '@angular/router';

import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject, catchError, map, of } from 'rxjs';

@Component({
  selector: 'app-property-ad-create',
  templateUrl: './property-ad-create.component.html',
  styleUrl: './property-ad-create.component.css'
})
export class PropertyAdCreateComponent {
  errors: string[] = [];
  createPropertyAdForm!: FormGroup;

  /**
   * Construtor da classe SignInComponent.
   * @param formBuilder Construtor de formulários do Angular.
   * @param router Router do Angular.
   */
  constructor(private http: HttpClient, private formBuilder: FormBuilder, private router: Router) {
    
  }
  ngOnInit(): void {
    this.errors = [];
    this.createPropertyAdForm = this.formBuilder.group(
      {
        // campos do form aqui
        name: ['', [Validators.required]],
        location: ['', [Validators.required]],
        pricePerNight: ['', [Validators.required]],
        amenityIds: [''],
        imagesUrl: ['']
      });
  }

  public create(_: any) {
    console.log(this.createPropertyAdForm.value)
    //return this.http.post<any>('/api/properties/create', this.createPropertyAdForm.value)
  }
}
