import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArchiveReceiptListComponent } from './archive-receipt-list.component';

describe('TemporaryReceiptListComponent', () => {
  let component: ArchiveReceiptListComponent;
  let fixture: ComponentFixture<ArchiveReceiptListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ArchiveReceiptListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ArchiveReceiptListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
