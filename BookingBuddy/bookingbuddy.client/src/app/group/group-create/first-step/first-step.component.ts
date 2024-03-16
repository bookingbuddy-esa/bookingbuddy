import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-first-step',
  templateUrl: './first-step.component.html',
  styleUrl: './first-step.component.css'
})
export class FirstStepComponent {
  @Input() currentPercentage: number = 0;
  @Output() nextStep: EventEmitter<void> = new EventEmitter<void>();
}
