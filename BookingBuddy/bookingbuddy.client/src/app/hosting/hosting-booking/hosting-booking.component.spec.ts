import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HostingBookingComponent } from './hosting-booking.component';

describe('HostingBookingComponent', () => {
  let component: HostingBookingComponent;
  let fixture: ComponentFixture<HostingBookingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HostingBookingComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HostingBookingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
