import {Component} from '@angular/core';
import {environment} from '../../../../environments/environment';

@Component({
  selector: 'app-microsoft',
  templateUrl: './microsoft.component.html',
  styleUrl: './microsoft.component.css'
})
export class MicrosoftComponent {
  public microsoftLogin() {
    const tenantId = environment.microsoftTenantId;
    const clientId = environment.microsoftClientId;
    const responseType = "id_token";
    const redirectUri = `${environment.apiUrl}/api/microsoft`;
    const scope = "openid email profile user.read";
    const response_mode = "form_post";
    const nonce = "something-random";
    const loginLink = `https://login.microsoftonline.com/${tenantId}/oauth2/v2.0/authorize?client_id=${clientId}&response_type=${responseType}&redirect_uri=${redirectUri}&scope=${scope}&response_mode=${response_mode}&nonce=${nonce}`;
    document.location.href = encodeURI(loginLink);
  }
}
