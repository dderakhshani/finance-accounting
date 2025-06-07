import { Component, Input, TemplateRef, ViewChild } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from "@angular/router";
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { Receipt } from '../../../entities/receipt';
import { BaseSetting } from '../../../entities/base';

@Component({
  selector: 'app-button-search',
  templateUrl: './button-search.component.html',
  styleUrls: ['./button-search.component.scss']
})
export class ButtonSearchComponent extends BaseSetting {
    exportexcel(data: any[]) {
        throw new Error('Method not implemented.');
    }
    CalculateSum(param?: any) {
        throw new Error('Method not implemented.');
    }
 
  
  constructor(
      private router: Router
    , public _mediator: Mediator
    , private route: ActivatedRoute
    , private sanitizer: DomSanitizer
    , public Service: PagesCommonService
    , public _notificationService: NotificationService,

  ) {
    super(route, router);
  }
  
  ondeleteFilter() {
    this.currentPage = 1;
    

  }
  

}
