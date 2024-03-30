import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { AmenitiesHelper } from '../../../../models/amenity-enum';

@Component({
  selector: 'app-amenity',
  templateUrl: './amenity.component.html',
  styleUrl: './amenity.component.css'
})
export class AmenityComponent implements OnInit{
  @Input() amenity: string = '';
  @Input() selected: boolean = false;
  @Output() onSelected = new EventEmitter<string>();
  protected displayAmenity: string = '';
  protected icon : string = '';

  ngOnInit() {
    this.displayAmenity = AmenitiesHelper.getAmenityDisplayName(this.amenity);
    this.icon = AmenitiesHelper.getAmenityIcon(this.amenity);
  }
}
