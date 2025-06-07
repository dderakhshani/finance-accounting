
import { EventEmitter, OnChanges, Output, SimpleChanges } from '@angular/core';
import { Component, Input } from '@angular/core';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { FormControl, FormGroup } from '@angular/forms';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import * as XLSX from 'xlsx';
import { pagging } from '../../../entities/pagging';
@Component({
  selector: 'app-table-pagging',
  templateUrl: './table-pagging.component.html',
  styleUrls: ['./table-pagging.component.scss']
})
export class tablePaggingComponent extends BaseComponent {

  
  @Input() RowsCount: number = 0;
  @Input() pageSize: number = 0;
  @Input() activePage: number = 1;
  @Output() ActivePage = new EventEmitter<number>();
  @Output() ExportToALLExcel = new EventEmitter<number>();
  

  PageCount: number = 1;
  pageList: number[] = [];
 
 
  constructor(
    private router: Router,
    public _mediator: Mediator,
    private route: ActivatedRoute,
    private sanitizer: DomSanitizer,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,

  ) {
    super(route, router);
  }

  async ngOnChanges(changes: SimpleChanges) {


    this.setting();
    
  }

  async ngOnInit() {

    this.setting();
    this.onSelectPage(1);

  }

  setting() {
   
    this.PageCount = Math.ceil(Number(this.RowsCount / this.pageSize));

  }
 
  onSelectPage(activePage: number) {

    
    
    if (this.PageCount >= activePage && activePage >= 1) {
      

      this.activePage = activePage

      this.ActivePage.emit(this.activePage);
     
    }
    
   
  }
  
  async ExportToExcel() {
    this.ExportToALLExcel.emit(0);
    
  }
  

  resolve(params?: any) {
    throw new Error('Method not implemented.');
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

}



