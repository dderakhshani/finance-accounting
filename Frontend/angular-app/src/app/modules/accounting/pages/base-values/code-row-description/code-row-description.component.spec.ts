import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CodeRowDescriptionComponent } from './code-row-description.component';

describe('CodeRowDescriptionComponent', () => {
  let component: CodeRowDescriptionComponent;
  let fixture: ComponentFixture<CodeRowDescriptionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CodeRowDescriptionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CodeRowDescriptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
