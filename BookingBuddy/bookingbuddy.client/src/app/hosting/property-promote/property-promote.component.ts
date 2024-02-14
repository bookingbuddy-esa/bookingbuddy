import { Component, OnInit } from '@angular/core';
import { HostingService } from '../hosting.service';
import { AuthorizeService } from '../../auth/authorize.service';
import { Router } from '@angular/router';
import { UserInfo } from '../../auth/authorize.dto';
import { Property } from '../../models/property';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { PropertyPromote } from '../../models/property-promote';

@Component({
  selector: 'app-property-promote',
  templateUrl: './property-promote.component.html',
  styleUrl: './property-promote.component.css'
})
export class PropertyPromoteComponent implements OnInit {
  signedIn: boolean = false;
  user: UserInfo | undefined;
  property_list: Property[] = [];
  currentProperty: Property | null = null;
  propertyPromoteForm!: FormGroup;

  constructor(private hostingService: HostingService, private authService: AuthorizeService, private router: Router, private formBuilder: FormBuilder) {
    this.propertyPromoteForm = this.formBuilder.group({
      duration: ['', Validators.required],
      paymentMethod: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.authService.isSignedIn().forEach(isSignedIn => {
      this.signedIn = isSignedIn;
      if (this.signedIn) {
        this.authService.user().forEach(async user => {
          this.user = user;

          await this.loadUserProperties();

        });
      } else {
        this.router.navigateByUrl('signin');
      }
    });
  }

  submitPromotion() {
    const durationSelected: string = this.propertyPromoteForm.get('duration')?.value;

    let calculatedEndDate: Date = new Date();

    switch (durationSelected) {
      case "1": {
        calculatedEndDate = new Date(calculatedEndDate.setDate(calculatedEndDate.getDate() + 7)); break
      }
      case "2": {
        calculatedEndDate = new Date(calculatedEndDate.setMonth(calculatedEndDate.getMonth() + 1)); break
      }
      case "3": {
        calculatedEndDate = new Date(calculatedEndDate.setFullYear(calculatedEndDate.getFullYear() + 1)); break
      }
    }

    let propertyPromote: PropertyPromote = {
      propertyId: this.currentProperty?.propertyId!,
      startDate: new Date(),
      endDate: calculatedEndDate
    }

    console.log(propertyPromote);
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
