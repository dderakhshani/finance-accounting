import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowAttachmentComponent } from './show-attachment.component';

describe('ShowAttachmentComponent', () => {
  let component: ShowAttachmentComponent;
  let fixture: ComponentFixture<ShowAttachmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowAttachmentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowAttachmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
