import { TestBed } from '@angular/core/testing';

import { VersionUpdateGuard } from './version-update.guard';

describe('VersionUpdateGuard', () => {
  let guard: VersionUpdateGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(VersionUpdateGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
