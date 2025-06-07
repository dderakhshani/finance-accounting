import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DirectReceiptListComponent } from './direct-receipt-list.component';

describe('DirectReceiptListComponent', () => {
  let component: DirectReceiptListComponent;
  let fixture: ComponentFixture<DirectReceiptListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DirectReceiptListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DirectReceiptListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
