import {Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import {HostingService} from '../hosting.service';
import {AuthorizeService} from '../../auth/authorize.service';
import {Router} from '@angular/router';
import {UserInfo} from '../../auth/authorize.dto';
import {Property} from '../../models/property';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {PropertyPromote} from '../../models/property-promote';
import { PaymentComponent } from '../../payment/payment.component';

@Component({
  selector: 'app-property-promote',
  templateUrl: './property-promote.component.html',
  styleUrl: './property-promote.component.css'
})
export class PropertyPromoteComponent implements OnInit {
  user: UserInfo | undefined;
  property_list: Property[] = [];
  currentProperty: Property | null = null;
  propertyPromoteForm!: FormGroup;
  propertyPromote: PropertyPromote | null = null;

  constructor(private hostingService: HostingService, private authService: AuthorizeService, private router: Router, private formBuilder: FormBuilder) {
    this.propertyPromoteForm = this.formBuilder.group({
      duration: ['week', Validators.required]
    });
  }

  ngOnInit(): void {
    this.authService.user().forEach(async user => {
      this.user = user;
      this.loadUserProperties();
    });
  }

  submitPromotion() {
    if (!this.propertyPromoteForm.valid) {
      console.log('inválido');
      return;
    }

    const durationSelected: string = this.propertyPromoteForm.get('duration')?.value;

    let calculatedEndDate: Date = new Date();

    switch (durationSelected) {
      case "week": {
        calculatedEndDate = new Date(calculatedEndDate.setDate(calculatedEndDate.getDate() + 7));
        break
      }
      case "month": {
        calculatedEndDate = new Date(calculatedEndDate.setMonth(calculatedEndDate.getMonth() + 1));
        break
      }
      case "year": {
        calculatedEndDate = new Date(calculatedEndDate.setFullYear(calculatedEndDate.getFullYear() + 1));
        break
      }
    }

    this.propertyPromote = {
      propertyId: this.currentProperty?.propertyId!,
      startDate: new Date(),
      endDate: calculatedEndDate
    }
  }

  setCurrentProperty(property: Property) {
    this.currentProperty = property;
    this.propertyPromote = null;
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
