import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {AmenitiesHelper} from "../../../models/amenity-enum";

@Component({
  selector: 'app-amenities-step',
  templateUrl: './amenities-step.component.html',
  styleUrl: './amenities-step.component.css'
})
export class AmenitiesStepComponent implements OnInit{
  protected readonly AmenitiesHelper = AmenitiesHelper;
  protected selectedAmenities: string[] = [];
  @Input() amenities: string[] = [];
  @Output() onAmenitiesChange = new EventEmitter<string[]>();

  ngOnInit(): void {
    this.selectedAmenities = this.amenities;
  }

  selectAmenity(amenity: string) {
    if (this.selectedAmenities.includes(amenity)) {
      this.selectedAmenities = this.selectedAmenities.filter(a => a !== amenity);
    } else {
      this.selectedAmenities.push(amenity);
    }
    this.onAmenitiesChange.emit(this.selectedAmenities);
  }

  isSelected(amenity: string): boolean {
    return this.selectedAmenities.includes(amenity);
  }
}
