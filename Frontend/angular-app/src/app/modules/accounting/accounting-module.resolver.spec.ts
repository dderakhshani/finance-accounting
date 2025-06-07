import { TestBed } from '@angular/core/testing';

import { AccountingModuleResolver } from './accounting-module.resolver';

describe('AccountingModuleResolver', () => {
  let resolver: AccountingModuleResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    resolver = TestBed.inject(AccountingModuleResolver);
  });

  it('should be created', () => {
    expect(resolver).toBeTruthy();
  });
});
