import { Component } from '@angular/core';

@Component({
  selector: 'app-group-name-step',
  templateUrl: './group-name-step.component.html',
  styleUrl: './group-name-step.component.css'
})
export class GroupNameStepComponent {

}

export interface GroupName {
  name: string;
}
