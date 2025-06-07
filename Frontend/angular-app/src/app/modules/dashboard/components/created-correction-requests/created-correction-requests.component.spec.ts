import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatedCorrectionRequestsComponent } from './created-correction-requests.component';

describe('CreatedCorrectionRequestsComponent', () => {
  let component: CreatedCorrectionRequestsComponent;
  let fixture: ComponentFixture<CreatedCorrectionRequestsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreatedCorrectionRequestsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreatedCorrectionRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
