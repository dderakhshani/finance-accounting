import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicationRequestLogsComponent } from './application-request-logs.component';

describe('ApplicationRequestLogsComponent', () => {
  let component: ApplicationRequestLogsComponent;
  let fixture: ComponentFixture<ApplicationRequestLogsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApplicationRequestLogsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ApplicationRequestLogsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
