import {Component, Input} from '@angular/core';
import {Property} from "../../../models/property";

@Component({
  selector: 'app-homepage-property',
  templateUrl: './homepage-property.component.html',
  styleUrl: './homepage-property.component.css'
})
export class HomepagePropertyComponent {
  @Input() property: Property | undefined;
  protected readonly console = console;
}
