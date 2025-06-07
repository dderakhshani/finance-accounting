import { Injectable } from '@angular/core';
import {IDBPDatabase, openDB} from "idb";

@Injectable({
  providedIn: 'root'
})
export class RequestsCacheService {

  private  CURRENT_VERSION = '1.0.1';
  private dbPromise!: Promise<IDBPDatabase>;
  constructor() {
    this.initializeDB().then(r => {
    });
  }
  // initializeDB
  private async initializeDB() {
    this.dbPromise = openDB('RequestsCache', 1, {
      upgrade(db) {
        if (!db.objectStoreNames.contains('RequestsCache')) {
          db.createObjectStore('RequestsCache');
        }
      },
    });
  }
  // saveSettings
  async saveRequest(pathname: string, request: any): Promise<void> {
    const requestCache = {
      version: this.CURRENT_VERSION,
      // filter
      request: request
    };
    const db = await this.dbPromise;
    await db.put('RequestsCache', requestCache, pathname);
  }
  // getSettings
  async getRequest(pathname: string): Promise<any> {
    if (!pathname) {
      console.error('Invalid pathname provided.');
      throw new Error('Pathname is required for retrieving settings.');
    }
    const db = await this.dbPromise;
    if (!db) {
      console.error('Database connection is not initialized.');
      throw new Error('Database not available.');
    }
    const requestCache = await db.get('RequestsCache', pathname);
    if (requestCache) {
      if (requestCache.version !== this.CURRENT_VERSION) {
        console.log(`Version mismatch. Expected: ${this.CURRENT_VERSION}, Found: ${requestCache.version}`);
        await this.deleteSettings(pathname);
        return null;
      }
      return requestCache.request;
    }else {
      return null;
    }
  }
  //deleteSettings
  async deleteSettings(pathname: string): Promise<void> {
    try {
      const db = await this.dbPromise;
      await db.delete('RequestsCache', pathname);
      console.log(`RequestsCache with pathname "${pathname}" deleted successfully.`);
    } catch (error) {
      console.error(`Failed to delete RequestsCache with pathname "${pathname}":`, error);
      throw error;
    }
  }
}
function removeNonSerializableProps(obj: any): any {
  const copy = {...obj};
  Object.keys(copy).forEach(key => {
    if (typeof copy[key] === 'function' || copy[key] instanceof Date || copy[key] instanceof Map || copy[key] instanceof Set) {
      delete copy[key];
    }
  });
  return copy;
}
