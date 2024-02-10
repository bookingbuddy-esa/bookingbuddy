import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomepagePropertyComponent } from './homepage-property.component';

describe('HomepagePropertyComponent', () => {
  let component: HomepagePropertyComponent;
  let fixture: ComponentFixture<HomepagePropertyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HomepagePropertyComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HomepagePropertyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
