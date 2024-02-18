import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AmenitiesStepComponent } from './amenities-step.component';

describe('AmenitiesStepComponent', () => {
  let component: AmenitiesStepComponent;
  let fixture: ComponentFixture<AmenitiesStepComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AmenitiesStepComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AmenitiesStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
