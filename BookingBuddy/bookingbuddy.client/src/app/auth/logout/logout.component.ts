import {Component, OnInit} from '@angular/core';
import {AuthorizeService} from "../authorize.service";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrl: './logout.component.css'
})
export class LogoutComponent implements OnInit {

  constructor(private authService: AuthorizeService, private router: Router) {
  }

  ngOnInit(): void {
    this.submitting = true;
    this.authService.signOut().forEach(
      response => {
        if (response) {
          this.submitting = false;
          setTimeout(() => {
            this.router.navigate([""])
          }, 5000);
        }
      }).catch(
      error => {
        this.router.navigate([""]).then(
          () => {
            this.submitting = false;
          }
        )
      });
  }

  submitting: boolean = false;

}
