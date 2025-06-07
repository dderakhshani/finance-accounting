import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CorrectionRequestsListComponent } from './correction-requests-list.component';

describe('CorrectionRequestsListComponent', () => {
  let component: CorrectionRequestsListComponent;
  let fixture: ComponentFixture<CorrectionRequestsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CorrectionRequestsListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CorrectionRequestsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
