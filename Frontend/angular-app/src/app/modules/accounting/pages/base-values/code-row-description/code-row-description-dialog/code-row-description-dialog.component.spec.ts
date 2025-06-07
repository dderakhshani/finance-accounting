import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CodeRowDescriptionDialogComponent } from './code-row-description-dialog.component';

describe('CodeRowDescriptionDialogComponent', () => {
  let component: CodeRowDescriptionDialogComponent;
  let fixture: ComponentFixture<CodeRowDescriptionDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CodeRowDescriptionDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CodeRowDescriptionDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
