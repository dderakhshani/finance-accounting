import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CacheService {
  private inMemoryData: any;

  constructor() {
  }


  public get(key: string): any[] {
    if (key && this.inMemoryData[key]) return this.inMemoryData[key];
    else return []
  }

  public cache(key: string, entities: any[]) {
    this.inMemoryData[key] = entities;
  }
}
