import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransactionHandlerComponent } from './transaction-handler.component';

describe('TransactionHandlerComponent', () => {
  let component: TransactionHandlerComponent;
  let fixture: ComponentFixture<TransactionHandlerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TransactionHandlerComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TransactionHandlerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
