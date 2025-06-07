import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TreeBranchItemComponent } from './tree-branch-item.component';

describe('TreeBranchItemComponent', () => {
  let component: TreeBranchItemComponent;
  let fixture: ComponentFixture<TreeBranchItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TreeBranchItemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TreeBranchItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
