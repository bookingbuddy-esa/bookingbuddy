import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdInfoStepComponent } from './ad-info-step.component';

describe('AdInfoStepComponent', () => {
  let component: AdInfoStepComponent;
  let fixture: ComponentFixture<AdInfoStepComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdInfoStepComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AdInfoStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
