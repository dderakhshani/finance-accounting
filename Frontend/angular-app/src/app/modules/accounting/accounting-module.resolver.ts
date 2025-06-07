import {Injectable} from '@angular/core';
import {AccountingHubService} from "./services/accounting-hub.service";
import {AccountManagerService} from "./services/account-manager.service";
import {forkJoin, Observable} from "rxjs";


@Injectable({
  providedIn: 'root',
})
export class AccountingModuleResolver {
  constructor(private accountingHubService: AccountingHubService,
  private accountManagerService:AccountManagerService) {}

  resolve() {
    forkJoin([
      this.accountingHubService.init(),
      this.accountManagerService.init()
    ])
  }
}
