import * as env from '../environments/environment.json';

export const environment = {
  production: false,
  apiUrl: env.apiUrl,
  googleClientId: env.googleClientId,
  microsoftClientId: env.microsoftClientId,
  microsoftTenantId: env.microsoftTenantId,
};
