import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PropertyPromoteComponent } from './property-promote.component';

describe('PropertyPromoteComponent', () => {
  let component: PropertyPromoteComponent;
  let fixture: ComponentFixture<PropertyPromoteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PropertyPromoteComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PropertyPromoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
