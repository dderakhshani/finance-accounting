import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BarWidgetComponent } from './bar-widget.component';

describe('BarWidgetComponent', () => {
  let component: BarWidgetComponent;
  let fixture: ComponentFixture<BarWidgetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BarWidgetComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BarWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
