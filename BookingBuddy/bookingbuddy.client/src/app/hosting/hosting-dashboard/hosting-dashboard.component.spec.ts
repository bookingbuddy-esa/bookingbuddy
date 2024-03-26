import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HostingDashboardComponent } from './hosting-dashboard.component';

describe('HostingDashboardComponent', () => {
  let component: HostingDashboardComponent;
  let fixture: ComponentFixture<HostingDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HostingDashboardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HostingDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
