import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowChequeDetailComponent } from './show-cheque-detail.component';

describe('ShowChequeDetailComponent', () => {
  let component: ShowChequeDetailComponent;
  let fixture: ComponentFixture<ShowChequeDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowChequeDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowChequeDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
