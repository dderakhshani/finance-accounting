import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceiptsListInsertedByCustomersComponent } from './receipts-list-inserted-by-customers.component';

describe('ReceiptsListInsertedByCustomersComponent', () => {
  let component: ReceiptsListInsertedByCustomersComponent;
  let fixture: ComponentFixture<ReceiptsListInsertedByCustomersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReceiptsListInsertedByCustomersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReceiptsListInsertedByCustomersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
