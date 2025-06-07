import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsanceListComponent } from './usance-list.component';

describe('UsanceListComponent', () => {
  let component: UsanceListComponent;
  let fixture: ComponentFixture<UsanceListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UsanceListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UsanceListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
