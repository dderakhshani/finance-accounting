import {AfterViewInit, ChangeDetectorRef, Component, ElementRef, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {animate, keyframes, state, style, transition, trigger} from "@angular/animations";
import {Toast, ToastPackage, ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-custom-toast',
  templateUrl: './custom-toast.component.html',
  styleUrls: ['./custom-toast.component.scss'],
  animations: [
    trigger('flyInOut', [
      state('inactive', style({
        opacity: 1,
      })),
      transition('inactive <=> active', animate('500ms ease-out', keyframes([
        style({
          transform: 'translateX(296px)',
          offset:0,
          opacity: 0,
        }),
        style({
          offset:.7,
          opacity: 1,
          transform: 'translateX(-20px)'
        }),
        style({
          offset: 1,
          transform: 'translateX(0)',
        })
      ]))),
      transition('active => removed', animate('500ms ease-in', keyframes([
        style({
          transform: 'translateX(-20px)',
          opacity: 1,
          offset: .2
        }),
        style({
          opacity:0,
          transform: 'translateX(296px)',
          offset: 1
        })
      ])))
    ]),
  ],
  preserveWhitespaces: false,

})
export class CustomToastComponent extends Toast  implements AfterViewInit, OnDestroy{
  @ViewChild('messageContent', { static: false }) messageContent!: ElementRef;
  undoString = 'undo';
  isExpanded = false;
  showExpandIcon = true;
  private resizeObserver!: ResizeObserver;
  // constructor is only necessary when not using AoT
  constructor(
    protected toastrService: ToastrService,
    public toastPackage: ToastPackage,
    private cdr: ChangeDetectorRef,
  ) {
    super(toastrService, toastPackage);
  }
  ngAfterViewInit() {
    this.resizeObserver = new ResizeObserver(() => {
      this.checkTextOverflow();
    });

    if (this.messageContent) {
      this.resizeObserver.observe(this.messageContent.nativeElement);
    }
    this.cdr.detectChanges();
  }
  action(event: Event) {
    event.stopPropagation();
    this.undoString = 'undid';
    this.toastPackage.triggerAction();
    return false;
  }
  get toastType(): string {
    return this.toastPackage.toastType.toLowerCase();
  }

  toggleMessage(event: Event) {
    event.stopPropagation();
    this.isExpanded = !this.isExpanded;
  }

  private checkTextOverflow() {
    setTimeout(() => {
      if (this.messageContent) {
        const element = this.messageContent.nativeElement;
        const lineHeight = parseInt(getComputedStyle(element).lineHeight);
        this.showExpandIcon = element.scrollHeight > lineHeight * 1.5;
      }
    }, 100);
  }
  ngOnDestroy() {
    if (this.resizeObserver) {
      this.resizeObserver.disconnect();
    }
  }
}
