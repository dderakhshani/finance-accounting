import { TestBed } from '@angular/core/testing';

import { AccountingHubService } from './accounting-hub.service';

describe('AccountingHubService', () => {
  let service: AccountingHubService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AccountingHubService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
