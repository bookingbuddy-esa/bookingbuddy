import {Component, OnInit} from '@angular/core';
import {AuthorizeService} from '../authorize.service';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrl: './confirm-email.component.css'
})
export class ConfirmEmailComponent implements OnInit {
  submitting: boolean = false;
  errors: string[] = [];

  constructor(private authService: AuthorizeService, private route: ActivatedRoute, private router: Router) {
  }

  ngOnInit(): void {
    this.submitting = true;
    const uid = this.route.snapshot.queryParamMap.get("uid");
    const token = this.route.snapshot.queryParamMap.get("token");
    if (uid !== null && token !== null) {
      this.authService.confirmEmail(uid, token).forEach(
        response => {
          if (response) {
            this.submitting = false;
          }
        }).catch(
        error => {
          if (error.error) {
            const errorObj = JSON.parse(error.error);
            Object.entries(errorObj).forEach((entry) => {
              const [key, value] = entry;
              var code = (value as any)["code"]
              var description = (value as any)["description"];
              if (code === "InvalidToken" || code === "UserNotFound") {
                //em caso de o token ou o user serem inválidos
                //this.validUrl = false;
              }
              this.errors.push(description);
            });
          }
          this.submitting = false;
        });
    } else {
      this.router.navigate(["bad-request"]);
    }
  }
}
