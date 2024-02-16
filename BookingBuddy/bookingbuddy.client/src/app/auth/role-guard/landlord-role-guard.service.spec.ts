import { TestBed } from '@angular/core/testing';

import { LandlordRoleGuardService } from './landlord-role-guard.service';

describe('LandlordRoleGuardService', () => {
  let service: LandlordRoleGuardService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LandlordRoleGuardService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
