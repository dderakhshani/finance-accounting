import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddReceiveChequeComponent } from './add-receive-cheque.component';

describe('AddReceiveChequeComponent', () => {
  let component: AddReceiveChequeComponent;
  let fixture: ComponentFixture<AddReceiveChequeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddReceiveChequeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddReceiveChequeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
