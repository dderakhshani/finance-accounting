import { DOCUMENT } from "@angular/common";
import {AfterViewInit, Directive, ElementRef, Inject, Output, Renderer2} from "@angular/core";
import { distinctUntilChanged, map, switchMap, takeUntil, tap } from "rxjs/operators";
import { fromEvent } from "rxjs";

@Directive({
  selector: "[resizable]"
})
export class ResizableDirective implements AfterViewInit{

  @Output()
  readonly resizable = fromEvent<MouseEvent>(
    this.elementRef.nativeElement,
    "mousedown"
  ).pipe(
    tap(e => e.preventDefault()),
    switchMap(() => {
      const closestTh = this.elementRef.nativeElement.closest("th");
      if (!closestTh) return [];

      const parentNode = closestTh.parentNode as HTMLElement | null;
      if (!parentNode) return [];

      const { width = 0, right = 0, left = 0 } = closestTh.getBoundingClientRect();
      const isRTL = getComputedStyle(closestTh).direction === "rtl";
      const columnIndex = Array.from(parentNode.children).indexOf(closestTh);

      return fromEvent<MouseEvent>(this.documentRef, "mousemove").pipe(
        map(({ clientX }) => {
          if (isRTL) {
            return Math.max(Math.min(width - (clientX - left), right-50), 0);
          } else {
            return Math.max(Math.min(width + (clientX - right), left -50), 0);
          }
        }),
        distinctUntilChanged(),
        tap(newWidth => {
          this.renderer.setStyle(closestTh, "width", `${newWidth}px`);
          this.renderer.setStyle(closestTh, "max-width", `${newWidth}px`);
          this.renderer.setStyle(closestTh, "min-width", `${newWidth}px`);
          this.saveColumnWidth(columnIndex, newWidth);
        }),
        takeUntil(fromEvent(this.documentRef, "mouseup"))
      );
    })
  );

  constructor(
    @Inject(DOCUMENT) private readonly documentRef: Document,
    @Inject(ElementRef) private readonly elementRef: ElementRef<HTMLElement>,
    private renderer: Renderer2
  ) {

  }
  ngAfterViewInit() {
    // this.loadColumnWidths();
  }


  saveColumnWidth(index: number, width: number): void {
    const pagePath = window.location.pathname;
    const table = this.elementRef.nativeElement.closest("table");



    loadFromIndexedDB(pagePath).then((columnWidths) => {
      columnWidths = columnWidths || {};
      columnWidths[index] = width;

      // ذخیره داده‌ها در IndexedDB
      // saveToIndexedDB(pagePath, columnWidths)
      //   .then(() => {
      //     // console.log("Column width saved successfully");
      //   })
      //   .catch((error) => {
      //     console.error("Error saving column width", error);
      //   });
    });
  }
  loadColumnWidths(): void {
    const pagePath = window.location.pathname;
    const table = this.elementRef.nativeElement.closest("table");

    if (!table) {
      console.warn("Table element not found");
      return;
    }



    loadFromIndexedDB(pagePath).then((columnWidths) => {
      if (!columnWidths) {
        console.warn("No column widths found");
        return;
      }

      setTimeout(() => {
        const ths = table.querySelectorAll("th");
        if (ths.length === 0) {
          console.warn("No <th> elements found");
          return;
        }

        ths.forEach((th, index) => {
          if (columnWidths[index]) {
            this.renderer.setStyle(th, "width", `${columnWidths[index]}px`);
          }
        });
      });
    });
  }

}

function openDatabase(): Promise<IDBDatabase> {
  return new Promise((resolve, reject) => {
    const request = indexedDB.open("ColumnWidthsDB", 1);

    request.onupgradeneeded = (event: any) => {
      const db = event.target.result;
      if (!db.objectStoreNames.contains("columnWidths")) {
        db.createObjectStore("columnWidths", { keyPath: "id" });
      }
    };

    request.onsuccess = (event: any) => {
      resolve(event.target.result);
    };

    request.onerror = (event: any) => {
      reject("Error opening database");
    };
  });
}
async function saveToIndexedDB(id: string, data: any): Promise<void> {
  const db = await openDatabase();
  return await new Promise((resolve, reject) => {
    const transaction = db.transaction("columnWidths", "readwrite");
    const store = transaction.objectStore("columnWidths");
    const request = store.put({id, data});

    request.onsuccess = () => {
      resolve();
    };

    request.onerror = () => {
      reject("Error saving data");
    };
  });
}
async function loadFromIndexedDB(id: string): Promise<any> {
  const db = await openDatabase();
  return await new Promise((resolve, reject) => {
    const transaction = db.transaction("columnWidths", "readonly");
    const store = transaction.objectStore("columnWidths");
    const request = store.get(id);

    request.onsuccess = (event: any) => {
      resolve(event.target.result ? event.target.result.data : null);
    };

    request.onerror = () => {
      reject("Error loading data");
    };
  });
}


