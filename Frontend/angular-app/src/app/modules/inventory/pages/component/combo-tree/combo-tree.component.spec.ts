import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComboTreeComponent } from './combo-tree.component';

describe('ComboTreeComponent', () => {
  let component: ComboTreeComponent;
  let fixture: ComponentFixture<ComboTreeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ComboTreeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ComboTreeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
