import {Component, OnInit} from '@angular/core';
import {AuthorizeService} from '../auth/authorize.service';
import {UserInfo} from '../auth/authorize.dto';
import {Router} from '@angular/router';
import {Property} from '../models/property';
import { PropertyAdService } from '../property-ad/property-ad.service';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrl: './homepage.component.css'
})
export class HomepageComponent implements OnInit {
  signedIn: boolean = false;
  submitting: boolean = false;
  user: UserInfo | undefined;
  property_list: Property[] = [];

  constructor(private authService: AuthorizeService, private propertyService: PropertyAdService, private router: Router) {
  }

  ngOnInit(): void {
    this.authService.isSignedIn().forEach(
      isSignedIn => {
        this.signedIn = isSignedIn;
        if (isSignedIn) {
          this.authService.user().forEach(user => this.user = user);
        }
      });
    this.authService.onStateChanged().forEach(isSignedIn => {
      this.signedIn = isSignedIn;
      if (isSignedIn) {
        this.authService.user().forEach(user => this.user = user);
      }
    });
    /*const testPhotosUrl = [
      "https://www.usatoday.com/gcdn/-mm-/05b227ad5b8ad4e9dcb53af4f31d7fbdb7fa901b/c=0-64-2119-1259/local/-/media/USATODAY/USATODAY/2014/08/13/1407953244000-177513283.jpg",
      "https://png.pngtree.com/thumb_back/fh260/background/20230425/pngtree-living-room-with-window-and-wooden-furniture-image_2514066.jpg",
      "https://media.istockphoto.com/id/119926339/photo/resort-swimming-pool.jpg?s=612x612&w=0&k=20&c=9QtwJC2boq3GFHaeDsKytF4-CavYKQuy1jBD2IRfYKc=",
      "https://upload.wikimedia.org/wikipedia/commons/7/79/Ponta_Negra_Beach_Hotel.jpg",
      "https://digital.ihg.com/is/image/ihg/ihg-lp-refresh-hero-imea-gben-lvp-1440x617"
    ];

    const testLocation = [
      "Atouguia da Baleia, Portugal",
      "Lisboa, Portugal",
      "Porto, Portugal",
      "Funchal, Portugal",
      "Portimão, Portugal"
    ];
    for (let i = 0; i < 30; i++) {
      const number = Math.floor(Math.random() * 5);
      this.property_list.push({
        propertyId: i.toString(),
        landlordId: "landlord",
        name: "Property " + i,
        location: testLocation[number],
        description: "Random description",
        pricePerNight: Math.floor(Math.random() * 1000),
        amenityIds: [],
        imagesUrl: [testPhotosUrl[number]]
      });
    }*/

    this.propertyService.getProperties().forEach(
      response => {
        if (response) {
          this.property_list = response as Property[];
        }
      }).catch(
        error => {
          //this.errors.push("TODO");
        }
      );
  }

  public logout() {
    this.authService.signOut().forEach(response => {
      if (response) {
        this.router.navigateByUrl('');
      }
    });
  }
}
