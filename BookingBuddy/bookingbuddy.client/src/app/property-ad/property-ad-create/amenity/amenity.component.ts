import {Component, Input} from '@angular/core';
import {AmenitiesHelper} from "../../../models/amenityEnum";

@Component({
  selector: 'app-amenity',
  templateUrl: './amenity.component.html',
  styleUrl: './amenity.component.css'
})
export class AmenityComponent {
  @Input() amenity: string | undefined;
  protected readonly AmenitiesHelper = AmenitiesHelper;
}
