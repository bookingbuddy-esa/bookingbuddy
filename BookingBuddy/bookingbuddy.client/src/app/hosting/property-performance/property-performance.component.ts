import {Component, OnInit} from '@angular/core';
import {Property, PropertyMetrics} from '../../models/property';
import {UserInfo} from '../../auth/authorize.dto';
import {AuthorizeService} from '../../auth/authorize.service';
import {HostingService} from '../hosting.service';
import {timeout} from "rxjs";

@Component({
  selector: 'app-property-performance',
  templateUrl: './property-performance.component.html',
  styleUrl: './property-performance.component.css'
})
export class PropertyPerformanceComponent implements OnInit {
  user: UserInfo | undefined;
  property_list: Property[] = [];
  currentProperty: Property | null = null;
  propertyMetrics: PropertyMetrics | undefined;
  submitting: boolean = false;

  constructor(private hostingService: HostingService, private authService: AuthorizeService) {

  }


  ngOnInit(): void {
    this.authService.user().forEach(async user => {
      this.user = user;
      this.loadUserProperties();
    });
  }


  setCurrentProperty(property: Property) {
    this.submitting = true;
    this.currentProperty = property;
    this.hostingService.getPropertyMetrics(this.currentProperty.propertyId).pipe(timeout(10000)).forEach(metrics => {
      this.propertyMetrics = metrics;
    })
      .then(() => this.submitting = false)
      .catch(() => {
        this.submitting = false;
      });
  }

  get propertyRating(): number | undefined {
    let rating = 0;
    if (this.propertyMetrics?.ratings.length) {
      for (let i = 0; i < this.propertyMetrics.ratings.length; i++) {
        rating += this.propertyMetrics.ratings[i].value;
      }
      return rating / this.propertyMetrics.ratings.length;
    }
    return undefined;
  }

  get propertyOccupancyByYearPercentage(): number {
    let occupancy = 0;
    if (this.propertyMetrics?.orders.length) {
      for (let i = 0; i < this.propertyMetrics.orders.length; i++) {
        let order = this.propertyMetrics.orders[i];
        let startDate = new Date(order.startDate);
        let endDate = new Date(order.endDate);
        occupancy += (endDate.getTime() - startDate.getTime()) / (1000 * 3600 * 24);
      }
      return (occupancy / 365) * 100;
    }
    return 0;
  }

  get propertyGains(): number {
    let gains = 0;
    if (this.propertyMetrics?.orders.length) {
      for (let i = 0; i < this.propertyMetrics.orders.length; i++) {
        gains += this.propertyMetrics.orders[i].amount;
      }
    }
    return gains;
  }


  private loadUserProperties() {
    if (this.user) {
      this.submitting = true;
      this.hostingService.getPropertiesByUserId(this.user?.userId).pipe(timeout(10000)).pipe(timeout(10000)).forEach(properties => {
        this.property_list = properties;
        this.setCurrentProperty(this.property_list[0]);
      }).then(() => this.submitting = false)
        .catch(() => {
          this.submitting = false;
        });
    }
  }


}
