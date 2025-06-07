import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PersonsDebitedCommodityListComponent } from './persons-debited-commodities-list.component';

describe('PersonsDebitedCommodityListComponent', () => {
  let component: PersonsDebitedCommodityListComponent;
  let fixture: ComponentFixture<PersonsDebitedCommodityListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PersonsDebitedCommodityListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonsDebitedCommodityListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
