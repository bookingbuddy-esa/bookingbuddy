import {Injectable} from '@angular/core';
import {AuthorizeService} from "../authorize.service";
import {Router} from "@angular/router";
import {map, Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class LandlordRoleGuardService {

  constructor(private authService: AuthorizeService, private router: Router) {
  }

  canActivate(): Observable<boolean> {
    return this.authService.user().pipe(
      map((user) => {
        if (!user.roles.includes("landlord") && !user.roles.includes("admin")) {
          this.router.navigate(['forbidden']);
          return false;
        }
        return true;
      })
    )
  }
}
