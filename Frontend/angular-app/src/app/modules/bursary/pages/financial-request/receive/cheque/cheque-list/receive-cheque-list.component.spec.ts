import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceiveChequeList } from './receive-cheque-list.component';

describe('ReceiveChequeList', () => {
  let component: ReceiveChequeList;
  let fixture: ComponentFixture<ReceiveChequeList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReceiveChequeList ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReceiveChequeList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
