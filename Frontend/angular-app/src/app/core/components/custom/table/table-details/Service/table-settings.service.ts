import {Injectable} from '@angular/core';
import {IDBPDatabase, openDB} from "idb";
import {Column} from "../../models/column";
import {TableOptions} from "../../models/table-options";
import {TableScrollingConfigurations} from "../../models/table-scrolling-configurations";

@Injectable({
  providedIn: 'root'
})
export class TableSettingsService {
  private CURRENT_VERSION = '1.0.4';
  private dbPromise!: Promise<IDBPDatabase>;


  constructor() {
    this.initializeDB().then(r => {
    });
  }

  // initializeDB
  private async initializeDB() {
    this.dbPromise = openDB('TableSettingsDB', 1, {
      upgrade(db) {
        if (!db.objectStoreNames.contains('tableSettings')) {
          db.createObjectStore('tableSettings');
        }
      },
    });
  }

  // saveSettings
  async saveSettings(pathname: string, columns: Column[], options: TableOptions, username?: string, roleId?: number
  ): Promise<void> {
    // this.validateParameters(pathname, username, roleId, 'saving settings');
    const settings = {
      version: this.CURRENT_VERSION,
      username,
      roleId,
      // filter
      columns: columns.map(column => {
        const {
          sumRowDisplayFn,
          filterOptionsFunction,
          displayFunction,
          displayFn,
          filterOptionsFn,
          asyncOptions,
          groupRemainingNameOrFn,
          filter,
          template,
          expandRowWithTemplate,
          ...rest
        } = column;
        return removeNonSerializableProps(rest);
      }),
      //options.exportOptions.customExportCallbackFn
      options: {
        ...options,
        isExternalChange: true,
        exportOptions: {
          ...options.exportOptions,
          customExportCallbackFn: undefined, // حذف تابع
        },
      },
    };

    // const key = `${pathname}|${username}|${roleId}`;
    const db = await this.dbPromise;
    await db.put('tableSettings', settings, pathname);
  }

  // getSettings
  async getSettings(pathname: string, tableConfigurations: TableScrollingConfigurations, username?: string, roleId?: number): Promise<any> {
    // this.validateParameters(pathname, username, roleId, 'retrieving settings');
    if (!pathname) {
      console.error('Invalid pathname provided.');
      throw new Error('Pathname is required for retrieving settings.');
    }

    const db = await this.dbPromise;
    if (!db) {
      console.error('Database connection is not initialized.');
      throw new Error('Database not available.');
    }
    const key = `${pathname}|${username}|${roleId}`;
    const settings = await db.get('tableSettings', pathname);
    if (settings) {
      if (settings.version !== this.CURRENT_VERSION) {
        console.log(`Version mismatch. Expected: ${this.CURRENT_VERSION}, Found: ${settings.version}`);

        await this.deleteSettings(pathname);

        return tableConfigurations;
      }

      settings.columns = settings.columns.filter((column: any) =>
        tableConfigurations.columns.some((configColumn: any) => configColumn.field === column.field)
      );
      tableConfigurations.columns.forEach((configColumn: any) => {
        const existingColumn = settings.columns.find((column: any) => column.field === configColumn.field);
        if (!existingColumn) {
          settings.columns.push({ ...configColumn });
        }
      });
      // اعمال تنظیمات ستون‌ها
      settings.columns.forEach((column: any, index: number) => {
        const originalColumn = tableConfigurations.columns[index];
        if (originalColumn) {
          column.sumRowDisplayFn = originalColumn.sumRowDisplayFn;
          column.filterOptionsFunction = originalColumn.filterOptionsFunction;
          column.displayFunction = originalColumn.displayFunction;
          column.displayPrintFun = originalColumn.displayPrintFun;
          column.displayFn = originalColumn.displayFn;
          column.asyncOptions = originalColumn.asyncOptions;
          column.filterOptionsFn = originalColumn.filterOptionsFn;
          column.groupRemainingNameOrFn = originalColumn.groupRemainingNameOrFn;
          column.filter = tableConfigurations.columns.find((c: any) => c.field === column.field)?.filter;
          column.template = tableConfigurations.columns.find((c: any) => c.field === column.field)?.template;
          column.expandRowWithTemplate =tableConfigurations.columns.find((c: any) => c.field === column.field)?.expandRowWithTemplate;
        }




      });
      if (settings.options) {
        settings.options.isExternalChange = tableConfigurations.options?.isExternalChange;
        if (settings.options?.exportOptions) {
          const originalExportOptions = tableConfigurations.options.exportOptions;
          settings.options.exportOptions.customExportCallbackFn = originalExportOptions?.customExportCallbackFn || null;
        }
      }

      return settings;
    } else {
      return tableConfigurations;
    }

  }

  //deleteSettings
  async deleteSettings(pathname: string, username?: string, roleId?: number): Promise<void> {
    // this.validateParameters(pathname, username, roleId, 'deleting settings');
    try {
      const db = await this.dbPromise;
      await db.delete('tableSettings', pathname);
      console.log(`Setting with pathname "${pathname}" deleted successfully.`);
    } catch (error) {
      console.error(`Failed to delete setting with pathname "${pathname}":`, error);
      throw error;
    }
  }
  deleteEntireDatabase(databaseName: string ='TableSettingsDB') {
    try {
      const request = indexedDB.deleteDatabase(databaseName);

      request.onsuccess = () => {
        console.log(`Database "${databaseName}" deleted successfully.`);
      };

      request.onerror = (event) => {
        console.error(`Failed to delete database "${databaseName}".`, event);
      };

      request.onblocked = () => {
        console.warn(`Deletion of database "${databaseName}" is blocked. Please close all tabs or connections using this database.`);
      };
    } catch (error) {
      console.error(`Error deleting database "${databaseName}":`, error);
      throw error;
    }
  }
  validateParameters(pathname: string, username: string | undefined, roleId: number | null | undefined, operation: string): void {
    if (!pathname || !username || roleId == null) {
      console.error(
        `Invalid parameters provided for ${operation}. Pathname: ${pathname}, Username: ${username}, RoleId: ${roleId}`
      );
      throw new Error(`Pathname, username, and roleId are required for ${operation}.`);
    }
  }

//   deepCopy
  deepCopy(oldObj: any) {
    let newObj = oldObj;
    if (oldObj && typeof oldObj === "object") {
      if (oldObj instanceof Date) {
        return new Date(oldObj.getTime());
      }
      newObj = Object.prototype.toString.call(oldObj) === "[object Array]" ? [] : {};
      for (let i in oldObj) {
        newObj[i] = this.deepCopy(oldObj[i]);
      }
    }
    return newObj;
  }

  deepClone<T>(obj: T): T {
    return deepCloneT(obj)
  }
}

function deepCloneT<T>(obj: T): T {
  if (obj === null || typeof obj !== 'object') return obj;

  if (Array.isArray(obj)) {
    return obj.map(item => deepCloneT(item)) as unknown as T;
  }

  const clonedObj = {} as T;
  for (const key in obj) {
    if (Object.prototype.hasOwnProperty.call(obj, key)) {
      (clonedObj as any)[key] = deepCloneT((obj as any)[key]);
    }
  }

  return clonedObj;
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
