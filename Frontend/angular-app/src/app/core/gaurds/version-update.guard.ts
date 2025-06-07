import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import {LocalStorageRepository} from "../services/storage/local-storage-repository.service";
import {environment} from "../../../environments/environment.prod";

@Injectable({
  providedIn: 'root'
})
export class VersionUpdateGuard implements CanActivate {
  constructor(
    private _cookieService : LocalStorageRepository,
  ) {
  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    let version = this._cookieService.get('version');

    // versions doesnt match
    if(version && version !== environment.currentVersion) {
      // this._cookieService.deleteCookie('token');
      this._cookieService.set('version', environment.currentVersion, 30);
      return true
    }
    else {
      this._cookieService.set('version', environment.currentVersion, 30);
      return true;
    }
  }

}
