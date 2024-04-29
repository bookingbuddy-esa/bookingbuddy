import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output
} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {MapGeocoder} from "@angular/google-maps";

@Component({
  selector: 'app-location-step',
  templateUrl: './location-step.component.html',
  styleUrl: './location-step.component.css'
})
export class LocationStepComponent implements OnInit {
  @Input() location: MapLocation | undefined;
  @Output() locationSubmit: EventEmitter<MapLocation> = new EventEmitter<MapLocation>();
  @Output() locationValid: EventEmitter<boolean> = new EventEmitter<boolean>();

  protected locationForm = this.formBuilder.group({
    location: ['', [Validators.required, Validators.minLength(10)]]
  });

  // Google Maps

  protected googleAutoComplete?: google.maps.places.Autocomplete;
  protected center: google.maps.LatLngLiteral = {lat: 39.69738149392836, lng: -8.322673356323522};
  protected zoom: number = 6;
  protected options: google.maps.MapOptions = {
    gestureHandling: 'greedy',
    streetViewControl: false,
    zoomControl: true,
  };
  protected markerLocation: google.maps.LatLngLiteral | undefined;
  protected markerOptions: google.maps.MarkerOptions = {
    clickable: false,
  };

  constructor(private formBuilder: FormBuilder, private geocoder: MapGeocoder) {
  }

  ngOnInit(): void {
    this.locationForm.get('location')?.valueChanges.forEach(value => {
      if (this.locationForm.get('location')?.valid) {
        this.locationValid.emit(true);
      } else {
        this.locationValid.emit(false);
      }
    });

    if (this.location) {
      this.center = this.location.location!;
      this.markerLocation = this.location.location!;
      this.zoom = 15;
      this.locationForm.get('location')?.setValue(this.location.address!);
      this.locationForm.get('location')?.markAsDirty();
    } else {
      this.location = {location: this.center, address: ''};
    }

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
          this.location!.address = place.formatted_address!;
          this.location!.location = place.geometry?.location!.toJSON()!;
          this.center = this.location!.location;
          this.zoom = 15;
          this.locationSubmit.emit({location: this.location!.location, address: this.location!.address});
          this.markerLocation = this.location!.location;
        }
      });
    });
  }


  get locationFormField() {
    return this.locationForm.get('location');
  }

  setLocation(event: google.maps.MapMouseEvent): void {
    this.location!.location = event.latLng!.toJSON();
    this.geocoder.geocode({location: this.location?.location}).forEach(results => {
      if (results) {
        this.locationForm.get('location')?.setValue(results.results[0].formatted_address!);
        this.locationForm.get('location')?.markAsDirty();
        this.location!.address = results.results[0].formatted_address!;
        this.center = this.location!.location!;
        this.locationSubmit.emit({location: this.location!.location, address: this.location!.address});
        this.markerLocation = this.location!.location;
      }
    });
  }
}

export interface MapLocation {
  location: google.maps.LatLngLiteral;
  address: string;
}
