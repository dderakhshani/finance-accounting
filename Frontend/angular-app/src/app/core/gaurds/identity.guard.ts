import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate, CanLoad,
  Route,
  Router,
  RouterStateSnapshot,
  UrlSegment,
  UrlTree
} from '@angular/router';
import { Observable } from 'rxjs';
import {IdentityService} from "../../modules/identity/repositories/identity.service";
import {tr} from "date-fns/locale";

@Injectable({
  providedIn: 'root'
})
export class IdentityGuard implements CanActivate,CanLoad  {
  constructor(
    private router: Router,
    private identityService : IdentityService,
  ) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if(this.identityService.isAuthenticated()) {
      return true;
    } else {
      return false;
    }
  }

  canLoad(
    route: Route,
    segments: UrlSegment[]
  ): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    // Check if the user is authenticated or any other condition
    // if (this.identityService.isAuthenticated()) {
    //   return true; // Allow the module to load
    // } else {
      // Redirect to login or perform any other action
      return true; // Prevent the module from loading
    // }
  }
}
