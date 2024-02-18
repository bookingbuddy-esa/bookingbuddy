import {
  AfterContentInit,
  AfterViewChecked,
  AfterViewInit,
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
  @Input() currentPercentage: number = 0;
  @Input() location: google.maps.LatLngLiteral | undefined;
  @Output() locationSubmit: EventEmitter<google.maps.LatLngLiteral | undefined> = new EventEmitter<google.maps.LatLngLiteral | undefined>();
  @Output() nextStep: EventEmitter<void> = new EventEmitter<void>();
  @Output() previousStep: EventEmitter<void> = new EventEmitter<void>();

  protected currentLocation: string = '';
  protected locationForm = this.formBuilder.group({
    location: ['', [Validators.required, Validators.minLength(10)]]
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

  constructor(private formBuilder: FormBuilder, private geocoder: MapGeocoder) {
  }

  ngOnInit(): void {
    if(this.location) {
      this.display = this.location;
      this.center = this.location;
      this.zoom = 15;
      this.geocoder.geocode({location: this.display}).forEach(results => {
        if (results) {
          this.locationForm.get('location')?.setValue(results.results[0].formatted_address!);
          this.locationForm.get('location')?.markAsDirty();
          this.currentLocation = results.results[0].formatted_address!;
        }
      });
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
          this.currentLocation = place.formatted_address!;
          this.display = place.geometry?.location!.toJSON();
          this.center = this.display!;
          this.zoom = 15;
        }
      });
    });
  }


  get locationFormField() {
    return this.locationForm.get('location');
  }

  setLocation(event: google.maps.MapMouseEvent): void {
    this.display = event.latLng!.toJSON();
    this.geocoder.geocode({location: this.display}).forEach(results => {
      if (results) {
        this.locationForm.get('location')?.setValue(results.results[0].formatted_address!);
        this.locationForm.get('location')?.markAsDirty();
        this.currentLocation = results.results[0].formatted_address!;
        this.center = this.display!;
        this.zoom = 15;
      }
    });
  }
}
