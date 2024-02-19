import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PhotosStepComponent } from './photos-step.component';

describe('PhotosStepComponent', () => {
  let component: PhotosStepComponent;
  let fixture: ComponentFixture<PhotosStepComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PhotosStepComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PhotosStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
