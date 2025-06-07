import { Component } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from "@angular/router";
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';


@Component({
  selector: 'app-import-tadbir-inventory-excel-validate',
  templateUrl: './import-tadbir-inventory-excel-validate.component.html',
  styleUrls: ['./import-tadbir-inventory-excel-validate.component.scss']
})
export class ImportTadbirInventoryExcelValidateComponent extends BaseComponent {
 
  constructor(public _mediator: Mediator,
    private router: Router,
    private sanitizer: DomSanitizer,
    public _notificationService: NotificationService,
    public Service: PagesCommonService,

  ) {
    super();
  }

  async ngOnInit() {

  }
  async ngAfterViewInit() {
    await this.resolve()
  }
  async resolve() {
    

  }
  
  initialize() {
  }
  async get() {
   
  }
  
  
  async update() {
  }

  async add() {
  }

  close(): any {
  }

  delete(): any {
  }


}
