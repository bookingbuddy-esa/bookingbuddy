import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-microsoft',
  templateUrl: './microsoft.component.html',
  styleUrl: './microsoft.component.css'
})
export class MicrosoftComponent {
  public microsoftLogin() {
    var tenantId = "46464aab-10be-4730-96d1-7f0d52c6fefe";
    var clientId = "def42587-7c39-4a86-8114-2d22b5362517";
    var responseType = "id_token";
    var redirectUri = "https://localhost:7213/api/microsoft";
    var scope = "openid email profile user.read";
    var response_mode = "form_post";
    var nonce = "something-random";
    var loginLink = `https://login.microsoftonline.com/${tenantId}/oauth2/v2.0/authorize?client_id=${clientId}&response_type=${responseType}&redirect_uri=${redirectUri}&scope=${scope}&response_mode=${response_mode}&nonce=${nonce}`
    var encodedLink = encodeURI(loginLink);
    document.location.href = encodedLink;
  }
}
