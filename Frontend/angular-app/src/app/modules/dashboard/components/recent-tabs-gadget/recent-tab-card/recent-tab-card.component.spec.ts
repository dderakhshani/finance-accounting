import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecentTabCardComponent } from './recent-tab-card.component';

describe('RecentTabCardComponent', () => {
  let component: RecentTabCardComponent;
  let fixture: ComponentFixture<RecentTabCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RecentTabCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RecentTabCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
