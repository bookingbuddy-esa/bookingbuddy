import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InitialStepComponent } from './initial-step.component';

describe('InitialStepComponent', () => {
  let component: InitialStepComponent;
  let fixture: ComponentFixture<InitialStepComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [InitialStepComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(InitialStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
