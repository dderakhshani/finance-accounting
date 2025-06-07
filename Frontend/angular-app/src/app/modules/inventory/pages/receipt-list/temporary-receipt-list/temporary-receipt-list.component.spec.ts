import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TemporaryReceiptListComponent } from './temporary-receipt-list.component';

describe('TemporaryReceiptListComponent', () => {
  let component: TemporaryReceiptListComponent;
  let fixture: ComponentFixture<TemporaryReceiptListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TemporaryReceiptListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TemporaryReceiptListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
