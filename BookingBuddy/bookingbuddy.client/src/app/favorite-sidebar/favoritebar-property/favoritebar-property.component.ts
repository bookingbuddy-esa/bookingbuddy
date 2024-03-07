import {Component, Input} from '@angular/core';
import {Property} from "../../models/property";

@Component({
  selector: 'app-favoritebar-property',
  templateUrl: './favoritebar-property.component.html',
  styleUrl: './favoritebar-property.component.css'
})
export class FavoritebarPropertyComponent {
  @Input() property: Property | undefined;
  protected readonly console = console;
}
