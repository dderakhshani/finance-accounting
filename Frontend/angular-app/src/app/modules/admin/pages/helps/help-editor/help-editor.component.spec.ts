import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HelpEditorComponent } from './help-editor.component';

describe('HelpEditorComponent', () => {
  let component: HelpEditorComponent;
  let fixture: ComponentFixture<HelpEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HelpEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HelpEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
