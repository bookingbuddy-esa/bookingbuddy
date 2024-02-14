import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SidePropertiesComponent } from './side-properties.component';

describe('SidePropertiesComponent', () => {
  let component: SidePropertiesComponent;
  let fixture: ComponentFixture<SidePropertiesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SidePropertiesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SidePropertiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
