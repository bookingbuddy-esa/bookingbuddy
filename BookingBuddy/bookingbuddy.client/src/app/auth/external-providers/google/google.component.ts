declare var google: any;
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-google',
  templateUrl: './google.component.html',
  styleUrl: './google.component.css'
})
export class GoogleComponent implements OnInit {

  ngOnInit(): void {
    google.accounts.id.initialize({
      client_id: environment.googleClientId,
      ux_mode: 'redirect',
      login_uri: `${environment.apiUrl}/api/google`
    });
    google.accounts.id.renderButton(document.getElementById("googleButton"), {
      type: 'icon',
      width: '400',
    })
    google.accounts.id.prompt();
  }

}
