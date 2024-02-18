import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
  selector: 'app-amenities-step',
  templateUrl: './amenities-step.component.html',
  styleUrl: './amenities-step.component.css'
})
export class AmenitiesStepComponent {
  @Output() nextStep = new EventEmitter<void>();
  @Output() previousStep = new EventEmitter<void>();
  @Output() amenitiesSubmit = new EventEmitter<string[]>();
  @Input() currentPercentage!: number;
}
