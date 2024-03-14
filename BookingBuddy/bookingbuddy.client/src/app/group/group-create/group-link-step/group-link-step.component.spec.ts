import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupLinkStepComponent } from './group-link-step.component';

describe('GroupLinkStepComponent', () => {
  let component: GroupLinkStepComponent;
  let fixture: ComponentFixture<GroupLinkStepComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GroupLinkStepComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GroupLinkStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
