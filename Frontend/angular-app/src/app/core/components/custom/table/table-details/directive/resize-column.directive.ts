import { Directive, ElementRef, Inject, OnDestroy, Renderer2 } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { fromEvent, Subject } from 'rxjs';
import { distinctUntilChanged, map, switchMap, takeUntil, tap } from 'rxjs/operators';

@Directive({
  selector: '[appResizeColumn]'
})
export class ResizeColumnDirective implements OnDestroy {
  private destroy$ = new Subject<void>();

  constructor(
      @Inject(DOCUMENT) private documentRef: Document,
      private elementRef: ElementRef<HTMLElement>,
      private renderer: Renderer2
  ) {
    this.initializeResize();
  }

  private initializeResize() {
    const columnElement = this.elementRef.nativeElement;


    const handle = this.renderer.createElement('div');
    this.renderer.setStyle(handle, 'position', 'absolute');
    this.renderer.setStyle(handle, 'top', '0');
    this.renderer.setStyle(handle, 'right', '0');
    this.renderer.setStyle(handle, 'width', '5px');
    this.renderer.setStyle(handle, 'height', '100%');
    this.renderer.setStyle(handle, 'cursor', 'ew-resize');
    this.renderer.setStyle(handle, 'z-index', '100');
    this.renderer.setStyle(columnElement, 'position', 'relative');
    this.renderer.appendChild(columnElement, handle);


    this.renderer.listen(handle, 'mouseenter', () => {
      this.renderer.setStyle(columnElement, 'cursor', 'ew-resize');
    });

    this.renderer.listen(handle, 'mouseleave', () => {
      this.renderer.setStyle(columnElement, 'cursor', 'auto');
    });

    const start$ = fromEvent<MouseEvent>(handle, 'mousedown').pipe(
        tap(event => event.preventDefault()),
        switchMap(startEvent => {
          const startX = startEvent.pageX;
          const startWidth = columnElement.offsetWidth;

          // بهبود عملکرد با استفاده از requestAnimationFrame
          return fromEvent<MouseEvent>(this.documentRef, 'mousemove').pipe(
              map(moveEvent => {
                const moveX = moveEvent.pageX - startX;
                const newWidth = startWidth + moveX;
                return { columnElement, newWidth };
              }),
              distinctUntilChanged((prev, curr) => prev.newWidth === curr.newWidth),
              takeUntil(fromEvent(this.documentRef, 'mouseup').pipe(
                  tap(() => this.cleanup())
              ))
          );
        })
    );

    start$.subscribe(({ columnElement, newWidth }) => {
      requestAnimationFrame(() => {
        const minWidth = 100; // Minimum width set
        if (newWidth > minWidth) {
          columnElement.style.width = `${newWidth}px`;

          // Update the next column's width
          const nextColumn = columnElement.nextElementSibling as HTMLElement;
          if (nextColumn) {
            requestAnimationFrame(() => {
              const nextWidth = nextColumn.offsetWidth - (newWidth - columnElement.offsetWidth);
              if (nextWidth > minWidth) {
                nextColumn.style.width = `${nextWidth}px`;
              }
            });
          }
        }
      });
    });
  }

  private cleanup() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngOnDestroy() {
    this.cleanup();
  }
}
