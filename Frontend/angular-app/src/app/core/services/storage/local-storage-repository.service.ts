import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageRepository {

  constructor() { }

  public get(name: string) {
    // let ca: Array<string> = document.cookie.split(';');
    // let caLen: number = ca.length;
    // let cookieName = `${name}=`;
    // let c: string;
    //
    // for (let i: number = 0; i < caLen; i += 1) {
    //   c = ca[i].replace(/^\s+/g, '');
    //   if (c.indexOf(cookieName) == 0) {
    //     return c.substring(cookieName.length, c.length);
    //   }
    // }
    // return '';
    return localStorage.getItem(name);
  }

  public delete(name : string) {
    // this.setCookie(name, "", -1);
    localStorage.removeItem(name);
  }

  public set(name: string, value: any, expireDays: number = 1, path: string = "/") {
    // let d: Date = new Date();
    // d.setTime(d.getTime() + expireDays * 24 * 60 * 60 * 1000);
    // let expires: string = "expires=" + d.toUTCString();
    // document.cookie = name + "=" + value + "; " + expires + (path.length > 0 ? "; path=" + path : "/");
    localStorage.setItem(name, value);
  }

  public update(name:string, value:any) {

    this.delete(name)
    this.set(name,value);
  }
}
