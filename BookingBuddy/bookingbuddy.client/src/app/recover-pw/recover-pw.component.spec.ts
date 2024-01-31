import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecoverPWComponent } from './recover-pw.component';

describe('RecoverPWComponent', () => {
  let component: RecoverPWComponent;
  let fixture: ComponentFixture<RecoverPWComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RecoverPWComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RecoverPWComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
