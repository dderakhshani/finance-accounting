import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecentTabsGadgetComponent } from './recent-tabs-gadget.component';

describe('RecentTabsGadgetComponent', () => {
  let component: RecentTabsGadgetComponent;
  let fixture: ComponentFixture<RecentTabsGadgetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RecentTabsGadgetComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RecentTabsGadgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
