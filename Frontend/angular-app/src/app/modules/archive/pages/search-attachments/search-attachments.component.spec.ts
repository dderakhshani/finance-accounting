import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchAttachmentsComponent } from './search-attachments.component';

describe('SearchAttachmentsComponent', () => {
  let component: SearchAttachmentsComponent;
  let fixture: ComponentFixture<SearchAttachmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SearchAttachmentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchAttachmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
