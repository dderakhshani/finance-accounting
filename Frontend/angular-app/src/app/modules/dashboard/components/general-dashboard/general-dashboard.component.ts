import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {DomSanitizer, SafeResourceUrl} from "@angular/platform-browser";

@Component({
  selector: 'app-general-dashboard',
  templateUrl: './general-dashboard.component.html',
  styleUrls: ['./general-dashboard.component.scss']
})
export class GeneralDashboardComponent implements OnInit {
  @ViewChild('card_iframe', { static: false }) cardIframe!: ElementRef;
  safeUrl!: SafeResourceUrl;
  largeSize: boolean = false;
  constructor(private sanitizer: DomSanitizer) {}

  ngOnInit(): void {
    this.safeUrl = this.sanitizer.bypassSecurityTrustResourceUrl('https://bi.eefaceram.com/pbireports/powerbi/EefaPB?rs:Embed=true');

  }

  onIsLargeSize() {
    this.largeSize = !this.largeSize;

    if (this.cardIframe) {
      if (this.largeSize) {
        // حالت تمام صفحه
        this.cardIframe.nativeElement.style.position = 'fixed';
        this.cardIframe.nativeElement.style.top = '0';
        this.cardIframe.nativeElement.style.left = '0';
        this.cardIframe.nativeElement.style.width = '100vw';
        this.cardIframe.nativeElement.style.height = '100vh';
        this.cardIframe.nativeElement.style.zIndex = '9999'; // برای قرار دادن card در بالای همه عناصر
      } else {
        // حالت عادی
        this.cardIframe.nativeElement.style.position = 'static';
        this.cardIframe.nativeElement.style.width = '100%';
        this.cardIframe.nativeElement.style.height = 'auto'; // یا مقدار دلخواه خود را تعیین کنید
        this.cardIframe.nativeElement.style.zIndex = '1';
      }
    } else {
      console.error('cardIframe is still undefined when accessing style');
    }
    }


}
