import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PropertyAdRetrieveComponent } from './property-ad-retrieve.component';

describe('PropertyAdRetrieveComponent', () => {
  let component: PropertyAdRetrieveComponent;
  let fixture: ComponentFixture<PropertyAdRetrieveComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PropertyAdRetrieveComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PropertyAdRetrieveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
