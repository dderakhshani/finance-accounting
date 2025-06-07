import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, Resolve, RouterStateSnapshot} from "@angular/router";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../repositories/base-value/queries/get-base-values-by-unique-name-query";

@Injectable({
  providedIn: 'root'
})
export class AddUserDataResolver implements Resolve<any> {
  constructor(private _mediator: Mediator) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this._mediator.send(new GetBaseValuesByUniqueNameQuery('BlockedReason'))
  }
}
