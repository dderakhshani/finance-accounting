import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateFormulaComponent } from './create-formula.component';

describe('CreateFormulaComponent', () => {
  let component: CreateFormulaComponent;
  let fixture: ComponentFixture<CreateFormulaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateFormulaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateFormulaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
