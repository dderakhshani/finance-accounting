import {Component, Inject, Optional} from '@angular/core';
import {Mediator} from '../../../../../core/services/mediator/mediator.service';
import {NotificationService} from '../../../../../shared/services/notification/notification.service';
import {PagesCommonService} from '../../../../../shared/services/pages/pages-common.service';
import {BaseComponent} from '../../../../../core/abstraction/base.component';
import {ActivatedRoute, Router} from '@angular/router';
import {DomSanitizer} from '@angular/platform-browser';
import {FilesByPaymentNumber} from '../../../entities/files-by-payment-number';
import {GetFilesByPaymentNumberQuery} from '../../../repositories/receipt/queries/receipt/get-files-by-payment-number';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {da} from "date-fns/locale";

@Component({
  selector: 'app-files-by-payment-number',
  templateUrl: './files-by-payment-number.component.html',
  styleUrls: ['./files-by-payment-number.component.scss']
})
export class FilesByPaymentNumberComponent extends BaseComponent {

  files: FilesByPaymentNumber[] = [];
  isLargSizeShow: boolean = false
  modalImg: any;
  captionText: any;

  financialRequestNo!: string;

  constructor(
    private router: Router,
    public _mediator: Mediator,
    private route: ActivatedRoute,
    private sanitizer: DomSanitizer,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,
    @Optional() @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super(route, router);
    if (data) {
      this.financialRequestNo = data;
    } else {
      this.financialRequestNo = this.getQueryParam('PaymentNumber')
    }
  }


  async ngOnInit() {

    this.resolve();

  }

  resolve(params?: any) {
    this._mediator.send(new GetFilesByPaymentNumberQuery(this.financialRequestNo)).then(response => {
      this.files = response;
    })

  }

  closeFullScreenImage() {

    this.isLargSizeShow = false;
  }

  onclick(src: any, alt: any) {


    this.modalImg = src;
    this.captionText = alt;

    this.isLargSizeShow = true;
  }

  initialize(params?: any) {
    throw new Error('Method not implemented.');
  }

  add(param?: any) {
    throw new Error('Method not implemented.');
  }

  get(param?: any) {
    throw new Error('Method not implemented.');
  }

  update(param?: any) {
    throw new Error('Method not implemented.');
  }

  delete(param?: any) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }

  handleModalClicks(event: MouseEvent) {
    if(event.target !== document.getElementById('img01')) {
      this.closeFullScreenImage()
    }
  }
}



