import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReplyTicketComponent } from './reply-ticket.component';

describe('ReplyTicketComponent', () => {
  let component: ReplyTicketComponent;
  let fixture: ComponentFixture<ReplyTicketComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReplyTicketComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReplyTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
