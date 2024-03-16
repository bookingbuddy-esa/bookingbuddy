import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupMembersStepComponent } from './group-members-step.component';

describe('GroupMembersStepComponent', () => {
  let component: GroupMembersStepComponent;
  let fixture: ComponentFixture<GroupMembersStepComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GroupMembersStepComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GroupMembersStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
