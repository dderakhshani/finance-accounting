import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CButtonComponent } from './c-button.component';

describe('CButtonComponent', () => {
  let component: CButtonComponent;
  let fixture: ComponentFixture<CButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CButtonComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
