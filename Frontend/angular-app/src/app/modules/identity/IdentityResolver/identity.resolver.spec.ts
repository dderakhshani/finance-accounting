import { TestBed } from '@angular/core/testing';

import { IdentityResolver } from './identity.resolver';

describe('IdentityResolver', () => {
  let resolver: IdentityResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(IdentityResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
