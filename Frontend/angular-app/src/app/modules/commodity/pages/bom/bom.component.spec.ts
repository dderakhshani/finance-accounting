import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BomComponent } from './bom.component';

describe('BomComponent', () => {
  let component: BomComponent;
  let fixture: ComponentFixture<BomComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BomComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
