import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PropertyAdCreateComponent } from './property-ad-create.component';

describe('PropertyAdCreateComponent', () => {
  let component: PropertyAdCreateComponent;
  let fixture: ComponentFixture<PropertyAdCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PropertyAdCreateComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PropertyAdCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
