import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookmarkedTabsGadgetComponent } from './bookmarked-tabs-gadget.component';

describe('BookmarkedTabsGadgetComponent', () => {
  let component: BookmarkedTabsGadgetComponent;
  let fixture: ComponentFixture<BookmarkedTabsGadgetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BookmarkedTabsGadgetComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BookmarkedTabsGadgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
