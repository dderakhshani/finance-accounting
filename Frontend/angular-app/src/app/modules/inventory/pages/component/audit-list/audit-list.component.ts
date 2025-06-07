import { Component, Input, TemplateRef, ViewChild } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from "@angular/router";

import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { FormActionTypes } from '../../../../../core/constants/form-action-types';
import { FormAction } from '../../../../../core/models/form-action';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";
import { GetAuditByIdQuery } from '../../../repositories/autit/queries/get-audit-query-by-Id';
import { SpAudit } from '../../../entities/spAudit';

@Component({
  selector: 'app-audit-list',
  templateUrl: './audit-list.component.html',
  styleUrls: ['./audit-list.component.scss']
})
export class AuditListComponent extends BaseComponent {
 
  SpAudits: SpAudit[] = [];

  tableConfigurations!: TableConfigurations;


  listActions: FormAction[] = [

    FormActionTypes.refresh,

  ]
  @Input() primaryId!: number;
  @Input() tableName!: string;
  
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

    await this.resolve()
  }

  async resolve() {

   
    let columns: TableColumn[] = [

      
      new TableColumn(
        'descriptionType',
        'نوع عملیات',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('descriptionType', TableColumnFilterTypes.Text)

      ),
      
      new TableColumn(
        'createDate',
        'تاریخ',
        TableColumnDataType.Date,
        '10%',
        true,
        new TableColumnFilter('createDate', TableColumnFilterTypes.Date)

      ),
      new TableColumn(
        'createTime',
        'ساعت',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('createTime', TableColumnFilterTypes.Text)

      ),
      new TableColumn(
        'username',
        'کاربر',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('username', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'summery',
        'شرح',
        TableColumnDataType.Text,
        '50%',
        true,
        new TableColumnFilter('summery', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'primaryId',
        'شناسه جدول',
        TableColumnDataType.Number,
        '10%',
        true,
        new TableColumnFilter('primaryId', TableColumnFilterTypes.Number)
      ),
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    this.get();
  }

 async initialize() {
    
    
  }

  async get() {
    if (this.primaryId != undefined) {
      let request = new GetAuditByIdQuery(this.primaryId, this.tableName)
      let response = await this._mediator.send(request);
      this.SpAudits = response.data;
      response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);
    }
   

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
