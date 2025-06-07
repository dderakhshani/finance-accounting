import { Component, Input, TemplateRef, ViewChild } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from "@angular/router";
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { Receipt } from '../../../entities/receipt';

@Component({
  selector: 'app-correction-request',
  templateUrl: './correction-request.component.html',
  styleUrls: ['./correction-request.component.scss']
})
export class correctionRequestListComponent extends BaseComponent {
 
  @Input() public receipt: Receipt | undefined = undefined;
  panelOpenState:Boolean=false
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
  

  async ngOnInit() {
    await this.initialize();
    
  }
  async ngOnChanges() {
    this.get();
  }

  async ngAfterViewInit() {

   
  }

  async resolve() {

  
  }

 async initialize() {
    
    
  }

  async get() {
    
   

  }
  
  async print() {

  
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
