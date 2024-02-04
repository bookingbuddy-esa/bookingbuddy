declare var google: any;
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-google',
  templateUrl: './google.component.html',
  styleUrl: './google.component.css'
})
export class GoogleComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit(): void {
    google.accounts.id.initialize({
      client_id: '780818883221-79bvdf437f1rkq51jjvdl1l3n62pp5nv.apps.googleusercontent.com',
      ux_mode: 'redirect',
      login_uri: 'https://localhost:7213/api/google'
    });
    google.accounts.id.renderButton(document.getElementById("googleButton"), {
      type: 'icon',
      width: '400',
    })
    google.accounts.id.prompt();
  }

}
