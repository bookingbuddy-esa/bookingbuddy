import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Property } from '../../models/property';


import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { PropertyAdService } from '../property-ad.service';

@Component({
  selector: 'app-property-ad-retrieve',
  templateUrl: './property-ad-retrieve.component.html',
  styleUrl: './property-ad-retrieve.component.css'
})

@Injectable({
  providedIn: 'root'
})

export class PropertyAdRetrieveComponent implements OnInit {
  // get the property ad id from the route
  // retrieve the property ad from the server
  // display the property ad
  // handle the case when the property ad is not found
  // handle the case when the server is not available

  property: Property | undefined;
  id: string = "";

  constructor(private service: PropertyAdService, private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.service.getProperty(this.route.snapshot.params['id']).forEach(
      response => {
        if (response) {
          console.log(response);
          this.property = response as Property;
        }
      }).catch(
        error => {
          //this.errors.push("Acesso negado. Verifique as credenciais.");
        }
      );
    
    //this.property = { propertyId: "1", landlordId: "1", name: "Casa do Forte", location: "Peniche, Portugal", pricePerNight: 100, imagesUrl: [""] };
  }

}
