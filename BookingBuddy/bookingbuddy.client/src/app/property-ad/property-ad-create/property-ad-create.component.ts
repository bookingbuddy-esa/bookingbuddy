import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';

import {BehaviorSubject, interval, map, Observable, of, Subject, timeout} from 'rxjs';
import {PropertyAdService} from '../property-ad.service';
import {AuthorizeService} from '../../auth/authorize.service';
import {UserInfo} from "../../auth/authorize.dto";
import {Property} from "../../models/property";
import {MapLocation} from "./location-step/location-step.component";
import {AdInfo} from "./ad-info-step/ad-info-step.component";
import {coerceStringArray} from "@angular/cdk/coercion";
import {Amenity} from "../../models/amenity";

@Component({
  selector: 'app-property-ad-create',
  templateUrl: './property-ad-create.component.html',
  styleUrl: './property-ad-create.component.css'
})
export class PropertyAdCreateComponent implements OnInit {
  protected user: UserInfo | undefined;
  protected submitting: boolean = false;
  protected errors: string[] = [];
  protected currentStep: number = 0;
  protected readonly numberOfSteps: number = 4;
  protected step: Subject<number> = new BehaviorSubject(0);

  protected onStepChange() {
    return this.step.asObservable();
  }

  // Location
  protected location: MapLocation | undefined;
  protected isLocationValid: boolean = false;

  // Amenities
  protected selectedAmenities: string[] = [];

  // Photos
  protected photos: File[] = [];
  protected isPhotosValid: boolean = false;

  // Ad Info
  protected adInfo: AdInfo | undefined;
  protected isAdInfoValid: boolean = false;

  constructor(private authService: AuthorizeService,
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
        case 2:
          this.selectedAmenities = [];
          break;
        case 3:
          this.photos = [];
          this.isPhotosValid = false;
          break;
        case 4:
          this.adInfo = undefined;
          this.isAdInfoValid = false;
      }
      this.step.next(this.currentStep - 1);
    }
  }

  nextStep() {
    if (this.currentStep < this.numberOfSteps) {
      this.step.next(this.currentStep + 1);
    }
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

  setAmenities($event: string[]) {
    this.selectedAmenities = $event;
  }

  setPhotos($event: File[]) {
    this.photos = $event;
  }

  photosValid($event: boolean) {
    this.isPhotosValid = $event;
  }

  setAdInfo($event: AdInfo | undefined) {
    this.adInfo = $event;
  }

  adInfoValid($event: boolean) {
    this.isAdInfoValid = $event;
  }

  isValid(): boolean {
    switch (this.currentStep) {
      case 1:
        return this.isLocationValid;
      case 3:
        return this.isPhotosValid;
      case 4:
        return this.isAdInfoValid;
      default :
        return true;
    }
  }

  public create(_: any) {
    this.submitting = true;
    this.errors = [];

    this.propertyService.uploadImages(this.photos).pipe(
      timeout(10000),
    ).forEach(images => {
      const newProperty: Property = {
        propertyId: '',
        applicationUserId: '',
        name: this.adInfo?.name ?? '',
        location: this.location?.address ?? '',
        pricePerNight: this.adInfo?.pricePerNight ?? 0,
        description: this.adInfo?.description ?? '',
        imagesUrl: images,
        amenities: this.selectedAmenities,
      };
      console.log(newProperty);
      this.propertyService.createPropertyAd(newProperty).forEach(success => {
          if (success) {
            this.router.navigateByUrl('/');
          }
        }
      ).catch(() => {
        this.errors.push('Erro ao criar propriedade.');
        this.submitting = false;
        return of(null);
      });
    }).catch(() => {
      this.errors.push('Erro ao fazer upload das imagens.');
      this.submitting = false;
      return of(null);
    });
  }
}
