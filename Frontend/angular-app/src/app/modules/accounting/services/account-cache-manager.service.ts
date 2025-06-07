import { Injectable } from '@angular/core';
import {IDBPDatabase, openDB} from "idb";

@Injectable({
  providedIn: 'root'
})
export class AccountCacheManagerService {

   dbPromise: Promise<IDBPDatabase>;

  constructor() {
    this.dbPromise = this.initDB();
  }

  private async initDB(): Promise<IDBPDatabase> {
    return openDB('AccountingDB', 1, {
      upgrade(db) {
        // ایجاد جداول (object stores) مورد نیاز
        if (!db.objectStoreNames.contains('accountHeads')) {
          db.createObjectStore('accountHeads', { keyPath: 'id' });
        }
        if (!db.objectStoreNames.contains('accountReferenceGroups')) {
          db.createObjectStore('accountReferenceGroups', { keyPath: 'id' });
        }
        if (!db.objectStoreNames.contains('accountReferences')) {
          db.createObjectStore('accountReferences', { keyPath: 'id' });
        }
      },
    });
  }

  // ذخیره داده
  async storeData(storeName: string, data: any[]) {
    const db = await this.dbPromise;
    const tx = db.transaction(storeName, 'readwrite');
    const store = tx.store;

    const chunkSize = 100;
    for (let i = 0; i < data.length; i += chunkSize) {
      const chunk = data.slice(i, i + chunkSize);
      for (const item of chunk) {
        await store.put(item); // فقط await کافیه
      }
      // اجازه بده UI نفس بکشه
      await new Promise(resolve => setTimeout(resolve, 0));
    }

    await tx.done;
  }



  // بازیابی داده
  async getAllData(storeName: string) {
    const db = await this.dbPromise;
    return db.transaction(storeName).store.getAll();
  }

  // پاک کردن کش
  async clearStore(storeName: string) {
    const db = await this.dbPromise;
    const tx = db.transaction(storeName, 'readwrite');
    await tx.store.clear();
    await tx.done;
  }
}

