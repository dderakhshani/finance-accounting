import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GaugeWidgetComponent } from './gauge-widget.component';

describe('GaugeWidgetComponent', () => {
  let component: GaugeWidgetComponent;
  let fixture: ComponentFixture<GaugeWidgetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GaugeWidgetComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GaugeWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
