import { DOCUMENT } from "@angular/common";
import {AfterViewInit, Directive, ElementRef, Inject, Output, Renderer2} from "@angular/core";
import { distinctUntilChanged, map, switchMap, takeUntil, tap } from "rxjs/operators";
import { fromEvent } from "rxjs";

@Directive({
  selector: "[resizableDiv]"
})
export class ResizableDivDirective implements AfterViewInit{
  @Output()
  readonly resizable = fromEvent<MouseEvent>(
    this.elementRef.nativeElement,
    "mousedown"
  ).pipe(
    tap(e => e.preventDefault()),
    switchMap(() => {
      const resizableDiv = this.elementRef.nativeElement; // المان div مورد نظر
      const { width, right, left } = resizableDiv.getBoundingClientRect();
      const isRTL = getComputedStyle(resizableDiv).direction === "rtl";

      return fromEvent<MouseEvent>(this.documentRef, "mousemove").pipe(
        map(({ clientX }) => {
          if (isRTL) {
            return Math.max(Math.min(width - (clientX - left), 50));
          } else {
            return Math.max(Math.min(width + (clientX - right), 500), 50); // محدودیت حداکثر عرض 500px
          }
        }),
        distinctUntilChanged(),
        tap(newWidth => {
          this.renderer.setStyle(resizableDiv, "width", `${newWidth}px`);
          this.saveWidth(resizableDiv.id, newWidth); // ذخیره بر اساس id المان
        }),
        takeUntil(fromEvent(this.documentRef, "mouseup"))
      );
    })
  );

  constructor(
    @Inject(DOCUMENT) private readonly documentRef: Document,
    @Inject(ElementRef) private readonly elementRef: ElementRef<HTMLElement>,
    private renderer: Renderer2
  ) {}

  ngAfterViewInit() {
    this.loadWidth();
  }

  private saveWidth(elementId: string, width: number): void {
    saveToIndexedDB(elementId, width) // ذخیره بر اساس id
      .catch(error => console.error("Error saving width:", error));
  }

  private loadWidth(): void {
    const elementId = this.elementRef.nativeElement.id;
    if (!elementId) {
      console.warn("Element ID is required for loading width!");
      return;
    }

    loadFromIndexedDB(elementId).then(width => {
      if (width) {
        this.renderer.setStyle(this.elementRef.nativeElement, "width", `${width}px`);
      }
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


