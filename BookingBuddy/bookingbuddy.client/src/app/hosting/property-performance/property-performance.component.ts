import { Component } from '@angular/core';
import { Property } from '../../models/property';
import { UserInfo } from '../../auth/authorize.dto';
import { AuthorizeService } from '../../auth/authorize.service';
import { HostingService } from '../hosting.service';

@Component({
  selector: 'app-property-performance',
  templateUrl: './property-performance.component.html',
  styleUrl: './property-performance.component.css'
})
export class PropertyPerformanceComponent {
  user: UserInfo | undefined;
  property_list: Property[] = [];
  currentProperty: Property | null = null;

  constructor(private hostingService: HostingService, private authService: AuthorizeService) {
   
  }


  ngOnInit(): void {
    this.authService.user().forEach(async user => {
      this.user = user;
      this.loadUserProperties();
    });
  }

  setCurrentProperty(property: Property) {
    this.currentProperty = property;
  }


  private loadUserProperties() {
    if (this.user) {
      this.hostingService.getPropertiesByUserId(this.user?.userId).subscribe(
        properties => {
          this.property_list = properties;
          this.currentProperty = this.property_list[0];
        },
        error => {
          console.error('Erro ao obter propriedades do utilizador:', error);
        }
      );
    }
  }


}
