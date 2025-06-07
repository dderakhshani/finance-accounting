import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LangaugesListComponent } from './langauges-list.component';

describe('LangaugesListComponent', () => {
  let component: LangaugesListComponent;
  let fixture: ComponentFixture<LangaugesListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LangaugesListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LangaugesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
