import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MoadianInvoicesListComponent } from './moadian-invoices-list.component';

describe('MoadianInvoicesListComponent', () => {
  let component: MoadianInvoicesListComponent;
  let fixture: ComponentFixture<MoadianInvoicesListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MoadianInvoicesListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MoadianInvoicesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
