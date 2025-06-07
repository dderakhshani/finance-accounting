import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecieptWarhouseLayoutComponent } from './direct-warhouse-layout.component';

describe('RecieptWarhouseLayoutComponent', () => {
  let component: RecieptWarhouseLayoutComponent;
  let fixture: ComponentFixture<RecieptWarhouseLayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RecieptWarhouseLayoutComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RecieptWarhouseLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
