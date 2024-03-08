import { Component } from '@angular/core';
import { Property } from '../models/property';
import { AuthorizeService } from '../auth/authorize.service';
import { FavoriteService } from './favorite.service';
import { UserInfo } from '../auth/authorize.dto';

@Component({
  selector: 'app-favorite-sidebar',
  templateUrl: './favorite-sidebar.component.html',
  styleUrl: './favorite-sidebar.component.css'
})
export class FavoriteSidebarComponent {
  user: UserInfo | undefined;
  property_list: Property[] = [];
  signedIn: boolean = false;

  constructor(private favoriteService: FavoriteService, private authService: AuthorizeService) {
  }

  ngOnInit(): void {
    this.authService.isSignedIn().forEach(
      isSignedIn => {
        this.signedIn = isSignedIn;
        if (isSignedIn) {
          this.authService.user().forEach(user => this.user = user);
        }
      });
    this.authService.user().forEach(async user => {
      this.user = user;
      this.loadFavorites();
    });

  }

  loadFavorites() {
    if (this.user) {
      this.favoriteService.getUserFavorites(this.user?.userId).forEach(
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
  }
}
