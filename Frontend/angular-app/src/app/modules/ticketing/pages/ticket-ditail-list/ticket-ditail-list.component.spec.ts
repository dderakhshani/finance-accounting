import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketDitailListComponent } from './ticket-ditail-list.component';

describe('TicketDitailListComponent', () => {
  let component: TicketDitailListComponent;
  let fixture: ComponentFixture<TicketDitailListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TicketDitailListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TicketDitailListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
