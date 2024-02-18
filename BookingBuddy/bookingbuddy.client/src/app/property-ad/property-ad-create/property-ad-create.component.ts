import {AfterViewInit, Component, OnDestroy, OnInit} from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
  AbstractControl,
  FormArray,
  ReactiveFormsModule
} from "@angular/forms";
import {Router} from '@angular/router';

import {BehaviorSubject, interval, map, Observable, of, Subject, timeout} from 'rxjs';
import {AmenityEnum, AmenitiesHelper} from '../../models/amenityEnum';
import {PropertyAdService} from '../property-ad.service';
import {AuthorizeService} from '../../auth/authorize.service';
import {GoogleMap, MapGeocoder} from '@angular/google-maps';
import {UserInfo} from "../../auth/authorize.dto";
import {Property} from "../../models/property";

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

  protected nameAndDescriptionForm = this.formBuilder.group({
    name: ['', Validators.required],
    description: ['']
  });
  protected locationForm = this.formBuilder.group({
    location: ['', [Validators.required, Validators.minLength(10)]]
  });
  protected priceAndAmenitiesForm = this.formBuilder.group({
    pricePerNight: ['', Validators.required],
  });

  // Google Maps
  protected googleAutoComplete?: google.maps.places.Autocomplete;
  protected center: google.maps.LatLngLiteral = {lat: 39.69738149392836, lng: -8.322673356323522};
  protected zoom: number = 6;
  protected display: google.maps.LatLngLiteral | undefined;
  protected options: google.maps.MapOptions = {
    gestureHandling: 'greedy',
    streetViewControl: false,
    zoomControl: true,
  };
  protected markerOptions: google.maps.MarkerOptions = {
    clickable: false,
  };

  constructor(private authService: AuthorizeService,
              private formBuilder: FormBuilder,
              private router: Router,
              private propertyService: PropertyAdService,
              private geocoder: MapGeocoder) {
    this.errors = [];
  }

  ngOnInit(): void {
    this.authService.user().forEach(user => {
      this.user = user;
    });
    this.onStepChange().forEach(step => {
      this.currentStep = step;
      switch (step) {
        case 1: {
          google.maps.importLibrary("places").then(() => {
            this.googleAutoComplete = new google.maps.places.Autocomplete(
              document.getElementById('location') as HTMLInputElement,
              {
                types: ['address'],
                componentRestrictions: {country: 'pt'},
              }
            );
            this.googleAutoComplete.addListener('place_changed', () => {
              const place = this.googleAutoComplete?.getPlace();
              if (place) {
                this.locationForm.get('location')?.setValue(place.formatted_address!);
                this.display = place.geometry?.location!.toJSON();
                this.center = this.display!;
                this.zoom = 15;
              }
            });
          });
          break;
        }
        case 2: {
          break;
        }
        case 3: {
          break;
        }
        default : {
          this.locationForm.reset();
          this.display = undefined;
          this.nameAndDescriptionForm.reset();
          this.priceAndAmenitiesForm.reset();
        }
      }
    });
  }


  setLocation(event: google.maps.MapMouseEvent): void {
    this.display = event.latLng!.toJSON();
    this.geocoder.geocode({location: this.display}).forEach(results => {
      if (results) {
        this.locationForm.get('location')?.setValue(results.results[0].formatted_address!);
        this.locationForm.get('location')?.markAsDirty();
      }
    });
  }

  previousStep() {
    if (this.currentStep > 0)
      this.step.next(this.currentStep - 1);
  }

  nextStep() {
    if (this.currentStep < this.numberOfSteps)
      this.step.next(this.currentStep + 1);
  }

  get currentCompletePercentage() {
    return ((this.currentStep) / this.numberOfSteps) * 100;
  }

  get nameFormField() {
    return this.nameAndDescriptionForm.get('name');
  }

  get locationFormField() {
    return this.locationForm.get('location');
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
}
