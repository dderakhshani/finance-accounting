import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TableBodyVirtualScrollingComponent } from './table-body-virtual-scrolling.component';

describe('TableBodyVirtualScrollingComponent', () => {
  let component: TableBodyVirtualScrollingComponent;
  let fixture: ComponentFixture<TableBodyVirtualScrollingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TableBodyVirtualScrollingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TableBodyVirtualScrollingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
