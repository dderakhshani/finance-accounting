import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifierRequestsListComponent } from './verifier-requests-list.component';

describe('VerifierRequestsListComponent', () => {
  let component: VerifierRequestsListComponent;
  let fixture: ComponentFixture<VerifierRequestsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VerifierRequestsListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VerifierRequestsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
