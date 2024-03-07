import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FavoritebarPropertyComponent } from './favoritebar-property.component';

describe('HomepagePropertyComponent', () => {
  let component: FavoritebarPropertyComponent;
  let fixture: ComponentFixture<FavoritebarPropertyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FavoritebarPropertyComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FavoritebarPropertyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
