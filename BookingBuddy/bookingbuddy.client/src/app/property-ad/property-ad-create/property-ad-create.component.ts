import {Component, OnInit} from '@angular/core';
import {
  FormBuilder,
  Validators
} from "@angular/forms";
import {Router} from '@angular/router';

import {BehaviorSubject, interval, map, Observable, of, Subject, timeout} from 'rxjs';
import {AmenityEnum, AmenitiesHelper} from '../../models/amenityEnum';
import {PropertyAdService} from '../property-ad.service';
import {AuthorizeService} from '../../auth/authorize.service';
import {UserInfo} from "../../auth/authorize.dto";
import {Property} from "../../models/property";
import {MapLocation} from "./location-step/location-step.component";

@Component({
  selector: 'app-property-ad-create',
  templateUrl: './property-ad-create.component.html',
  styleUrl: './property-ad-create.component.css'
})
export class PropertyAdCreateComponent implements OnInit {
  protected user: UserInfo | undefined;
  protected submitting: boolean = false;

  // Amenities
  protected readonly AmenityEnum = AmenityEnum;
  protected readonly AmenitiesHelper = AmenitiesHelper;

  // Formulário
  protected selectedFiles: File[] = [];
  protected errors: string[] = [];
  protected currentStep: number = 0;
  protected readonly numberOfSteps: number = 3;
  protected step: Subject<number> = new BehaviorSubject(0);

  protected onStepChange() {
    return this.step.asObservable();
  }

  // Localização

  protected location: MapLocation | undefined;
  protected isLocationValid: boolean = false;

  constructor(private authService: AuthorizeService,
              private formBuilder: FormBuilder,
              private router: Router,
              private propertyService: PropertyAdService) {
    this.errors = [];
  }

  ngOnInit(): void {
    this.authService.user().forEach(user => {
      this.user = user;
    });
    this.onStepChange().forEach(step => {
      this.currentStep = step;
    });
  }

  previousStep() {
    if (this.currentStep > 0) {
      switch (this.currentStep) {
        case 1:
          this.location = undefined;
          this.isLocationValid = false;
          break;
      }
      this.step.next(this.currentStep - 1);
    }
  }

  nextStep() {
    if (this.currentStep < this.numberOfSteps)
      this.step.next(this.currentStep + 1);
  }

  get currentCompletePercentage() {
    return ((this.currentStep) / this.numberOfSteps) * 100;
  }

  setLocation($event: MapLocation | undefined) {
    this.location = $event;
  }

  locationValid($event: boolean) {
    this.isLocationValid = $event;
  }

  isValid(): boolean {
    switch (this.currentStep) {
      case 1:
        return this.isLocationValid;
      default :
        return true;
    }
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

  /*public create(_: any) {
    this.submitting = true;
    this.errors = [];

    this.propertyService.uploadImages(this.selectedFiles).pipe(
      timeout(10000),
    ).forEach(images => {
      const newProperty: Property = {
        propertyId: '',
        applicationUserId: '',
        name: this.createPropertyAdForm.get('name')?.value as string,
        location: this.createPropertyAdForm.get('location')?.value as string,
        pricePerNight: this.createPropertyAdForm.get('pricePerNight')?.value as number,
        description: this.createPropertyAdForm.get('description')?.value as string,
        imagesUrl: images,
        amenityIds: this.comodidadesSelecionadas.map(comodidade => Object.values(CheckboxOptions).indexOf(comodidade).toString()) as string[],
      };

      this.propertyService.createPropertyAd(newProperty).forEach(success => {
          if (success) {
            this.router.navigateByUrl('?success-create=true');
          }
        }
      ).catch(() => {
        this.errors.push('Erro ao criar propriedade.');
        this.createPropertyFailed = true;
        this.submitting = false;
        scroll(0, 0);
        return of(null);
      });
    }).catch(() => {
      this.errors.push('Erro ao fazer upload das imagens.');
      this.createPropertyFailed = true;
      this.submitting = false;
      scroll(0, 0);
      return of(null);
    });
  }*/
  setAmenities($event: string[]) {

  }
}
