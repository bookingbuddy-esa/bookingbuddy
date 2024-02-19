import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
  selector: 'app-initial-step',
  templateUrl: './initial-step.component.html',
  styleUrl: './initial-step.component.css'
})
export class InitialStepComponent {
  @Input() currentPercentage: number = 0;
  @Output() nextStep: EventEmitter<void> = new EventEmitter<void>();
}
