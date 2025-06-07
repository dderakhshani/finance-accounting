import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import {forkJoin, Observable, of} from 'rxjs';
import {AccountingHubService} from "../../accounting/services/accounting-hub.service";
import {AccountManagerService} from "../../accounting/services/account-manager.service";
import {IdentityService} from "../repositories/identity.service";
import {ApplicationUser} from "../repositories/application-user";


@Injectable({
  providedIn: 'root',
})
export class IdentityResolver {
  constructor(private identityService: IdentityService) {}

  async resolve() : Promise<any> {
   return await this.identityService.init()
  }
}
