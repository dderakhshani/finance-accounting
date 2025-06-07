import { AfterViewInit, Directive, ElementRef, HostListener, Input, Renderer2, OnChanges, SimpleChanges } from '@angular/core';

@Directive({
  selector: '[appDynamicTableHeight]'
})
export class DynamicTableHeightDirective implements AfterViewInit, OnChanges {

  @Input() appDynamicTableHeight: number = 40;
  @Input() tableData: any[] = [];  // ورودی برای داده‌های جدول
  @Input() type_height : string = 'max-height'

  constructor(private el: ElementRef, private renderer: Renderer2) {}

  ngAfterViewInit() {
    this.adjustTableHeight();
  }

  ngOnChanges(changes: SimpleChanges) {
    // اگر داده‌های جدول تغییر کردند، ارتفاع را دوباره تنظیم کنید
    if (changes['tableData'] && changes['tableData'].currentValue) {

      this.adjustTableHeight();
    }
  }

  @HostListener('window:resize')
  @HostListener('window:scroll')
  onResize() {
    this.adjustTableHeight();
  }

  public adjustTableHeight() {
    const tableContainer = this.el.nativeElement;
    const rect = tableContainer.getBoundingClientRect();
    const windowHeight = window.innerHeight;

    const tableOffsetTop = rect.top;
    const remainingHeight = windowHeight - tableOffsetTop;
    const newHeight = remainingHeight - this.appDynamicTableHeight;


    let heightTable = newHeight < 150 ? 300 : newHeight;

    this.renderer.setStyle(tableContainer, this.type_height, `${heightTable}px`);
  }
}
