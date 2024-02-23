import { Component, Input } from '@angular/core';
import { Property } from "../../../models/property";

@Component({
  selector: 'app-side-properties',
  templateUrl: './side-properties.component.html',
  styleUrl: './side-properties.component.css'
})
export class SidePropertiesComponent {
  @Input() property: Property | undefined;
  protected readonly console = console;
}
