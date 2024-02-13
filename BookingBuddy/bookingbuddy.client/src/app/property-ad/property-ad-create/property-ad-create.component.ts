import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators, AbstractControl, FormArray } from "@angular/forms";
import { Router } from '@angular/router';

import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject, catchError, map, of } from 'rxjs';
import { CheckboxOptions } from '../../models/checkboxes';
import { PropertyAdService } from '../property-ad.service';
import { AuthorizeService } from '../../auth/authorize.service';

@Component({
  selector: 'app-property-ad-create',
  templateUrl: './property-ad-create.component.html',
  styleUrl: './property-ad-create.component.css'
})
export class PropertyAdCreateComponent {
  selectedFiles: File[] = [];
  comodidades = Object.values(CheckboxOptions);
  comodidadesSelecionadas: CheckboxOptions[] = [];
  errors: string[] = [];
  createPropertyAdForm!: FormGroup;
  createPropertyFailed: boolean;
  checkboxOptions = CheckboxOptions;
  signedIn: boolean = false;

  constructor(private authService: AuthorizeService, private formBuilder: FormBuilder, private router: Router, private propertyService: PropertyAdService) {
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
  
  ngOnInit(): void {
    this.authService.isSignedIn().forEach(isSignedIn => {
      this.signedIn = isSignedIn;
      if (this.signedIn) {
        this.authService.user().forEach(async user => {
        });
      } else {
        this.router.navigateByUrl('signin');
      }
    });
  }

  //função para guardar as imagens num array // TODO:Restrições de tipo de ficheiro
  onFileSelected(event: any): void {
    this.selectedFiles = [];
    const inputElement = event.target;
    if (inputElement.files.length > 0) {
      const files = inputElement.files;

      for (let i = 0; i < files.length; i++) {
        const file = files[i];

        if (file.type === 'image/jpeg' || file.type === 'image/png') {
          this.selectedFiles.push(file);
        } else {
          this.errors.push('Tipo de arquivo inválido. Por favor, selecione um arquivo JPEG ou PNG.');
          console.log("Tipo de arquivo inválido. Por favor, selecione um arquivo JPEG ou PNG.")
        }
      }
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

    this.propertyService.uploadImages(this.selectedFiles).pipe(
      catchError((error: HttpErrorResponse) => {
        this.errors.push('Erro ao fazer upload das imagens.');
        this.createPropertyFailed = true;
        return of([]);
      }),
      map((images: string[]) => {
        const newProperty = {
          name: this.createPropertyAdForm.get('name')?.value,
          location: this.createPropertyAdForm.get('location')?.value,
          pricePerNight: this.createPropertyAdForm.get('pricePerNight')?.value,
          description: this.createPropertyAdForm.get('description')?.value,
          imagesUrl: images,
          amenityIds: this.comodidadesSelecionadas.map(comodidade => Object.values(CheckboxOptions).indexOf(comodidade).toString())
        };

        return newProperty;
      }),
      catchError((error: HttpErrorResponse) => {
        this.errors.push('Erro ao criar propriedade.');
        this.createPropertyFailed = true;
        return of(null);
      }),
      map((newProperty: any) => {
        if (newProperty) {
          this.propertyService.createPropertyAd(newProperty.name, newProperty.location, newProperty.pricePerNight, newProperty.description, newProperty.imagesUrl, newProperty.amenityIds).forEach(
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
      })
    ).subscribe();

    /*const images = this.selectedFile ? this.selectedFile.name : '';
    console.log(this.selectedFiles);
    this.propertyService.uploadImages(this.selectedFiles).subscribe(
      responses => {
        console.log("Respostas do upload:", responses);
      },
      error => {
        console.error(error);
      }
    );

    /*const newProperty = {
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
      );*/
  }
}
