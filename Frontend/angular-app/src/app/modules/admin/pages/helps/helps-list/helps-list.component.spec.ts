import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HelpsListComponent } from './helps-list.component';

describe('HelpsListComponent', () => {
  let component: HelpsListComponent;
  let fixture: ComponentFixture<HelpsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HelpsListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HelpsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
