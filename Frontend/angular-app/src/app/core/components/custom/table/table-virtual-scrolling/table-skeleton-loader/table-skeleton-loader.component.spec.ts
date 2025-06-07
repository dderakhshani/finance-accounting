import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TableSkeletonLoaderComponent } from './table-skeleton-loader.component';

describe('TableSkeletonLoaderComponent', () => {
  let component: TableSkeletonLoaderComponent;
  let fixture: ComponentFixture<TableSkeletonLoaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TableSkeletonLoaderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TableSkeletonLoaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
