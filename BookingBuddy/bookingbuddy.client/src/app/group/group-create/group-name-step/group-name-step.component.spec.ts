import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupNameStepComponent } from './group-name-step.component';

describe('GroupNameStepComponent', () => {
  let component: GroupNameStepComponent;
  let fixture: ComponentFixture<GroupNameStepComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GroupNameStepComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GroupNameStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
