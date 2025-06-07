import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { TableColumn } from 'src/app/core/components/custom/table/models/table-column';
import { TableColumnDataType } from 'src/app/core/components/custom/table/models/table-column-data-type';
import { TableColumnFilter } from 'src/app/core/components/custom/table/models/table-column-filter';
import { TableColumnFilterTypes } from 'src/app/core/components/custom/table/models/table-column-filter-types';
import { TableConfigurations } from 'src/app/core/components/custom/table/models/table-configurations';
import { TableOptions } from 'src/app/core/components/custom/table/models/table-options';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { TicketModel } from '../../entities/ticket';
import { GetTicketListQuery } from '../../repositories/tickets/queries/get-ticket-list-query';
import { SearchQuery } from 'src/app/shared/services/search/models/search-query';
import { Router } from '@angular/router';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { PageModes } from 'src/app/core/enums/page-modes';
import { CreateTicketComponent } from '../create-ticket/create-ticket.component';
import { TicketPriority, TicketPriority2ClassMapping, TicketPriority2LabelMapping } from '../../entities/ticket-priority';
import { TicketStatus, TicketStatus2ClassMapping, TicketStatus2LabelMapping } from '../../entities/ticket-status';
import { FormAction } from 'src/app/core/models/form-action';
import { FormActionTypes } from 'src/app/core/constants/form-action-types';
import { PreDefinedActions } from 'src/app/core/components/custom/action-bar/action-bar.component';
import {FormControl, FormGroup} from "@angular/forms";

@Component({
  selector: 'app-ticket-list',
  templateUrl: './ticket-list.component.html',
  styleUrls: ['./ticket-list.component.scss']
})
export class TicketListComponent extends BaseComponent {

  @ViewChild('statusTemplate', { read: TemplateRef }) statusTemplate!: TemplateRef<any>;
  @ViewChild('priorityTemplate', { read: TemplateRef }) priorityTemplate!: TemplateRef<any>;

  public entities: TicketModel[] = [];

  tableConfigurations!: TableConfigurations;

  public ticketPriority2LabelMapping = TicketPriority2LabelMapping;
  public ticketPriority = Object.values(TicketPriority).filter(value => typeof value === 'number');


  public ticketStatus2LabelMapping = TicketStatus2LabelMapping;
  public ticketStatus = Object.values(TicketStatus);

  public ticketStatus2ClassMapping = TicketStatus2ClassMapping;
  public ticketPriority2ClassMapping = TicketPriority2ClassMapping;

  // Getting the numeric values of TicketStatus enum
  ticketStatuses = Object.values(TicketStatus).filter(value => typeof value === 'number');

  // Importing the TicketStatus2LabelMapping for displaying labels
  TicketStatus2LabelMapping = TicketStatus2LabelMapping;

  ticketForm: FormGroup = new FormGroup(
    {
      ticketPriority: new FormControl(1),
      ticketStatuses: new FormControl(0),
    }
  );


  constructor(private _mediator: Mediator, private router: Router, public dialog: MatDialog) {
    super();
  }

  // listActions: FormAction[] = [
  //   FormActionTypes.add,
  // ]
  async ngOnInit() {

  }
  async ngAfterViewInit() {

    await this.resolve()

    this.actionBar.actions = [
      PreDefinedActions.add(),
      PreDefinedActions.refresh()
    ]

  }
  async resolve() {

    let statusColumn = new TableColumn(
      'status',
      'وضعیت',
      TableColumnDataType.Template,
      '',
      true
    );
    let priorityColumn = new TableColumn(
      'priority',
      'اولویت',
      TableColumnDataType.Template,
      '',
      true
    );
    statusColumn.template = this.statusTemplate;
    priorityColumn.template = this.priorityTemplate;
    let tableColumns = [
      new TableColumn('selected', '', TableColumnDataType.Select, ''),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, ''),
      new TableColumn(
        'id',
        'شناسه',
        TableColumnDataType.Number,
        '',
        true,
        new TableColumnFilter('id', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'title',
        'عنوان',
        TableColumnDataType.Text,
        '',
        true,
        new TableColumnFilter('title', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'roleTitle',
        'دریافت کننده',
        TableColumnDataType.Text,
        '',
        true,
        new TableColumnFilter('roleTitle', TableColumnFilterTypes.Text)
      ),  new TableColumn(
        'createUserTitle',
        'ارسال کننده',
        TableColumnDataType.Text,
        '',
        true,
        new TableColumnFilter('createUserTitle', TableColumnFilterTypes.Text)
      ),
      statusColumn,
      priorityColumn,
      new TableColumn(
        'createDate',
        'تاریخ ایجاد',
        TableColumnDataType.Date,
        '',
        true,
        new TableColumnFilter('createDate', TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        'updateDate',
        'اخرین آپدیت',
        TableColumnDataType.Date,
        '',
        true,
        new TableColumnFilter('updateDate', TableColumnFilterTypes.Date)
      ),

    ];
    this.tableConfigurations = new TableConfigurations(tableColumns, new TableOptions(false, true));

    this.tableConfigurations.options.showSumRow = true;

    await this.get();
  }
  initialize() {
    throw new Error('Method not implemented.');
  }
  async get() {

    let searchQueries: SearchQuery[] = []
    if (this.tableConfigurations.filters) {
      this.tableConfigurations.filters.forEach(filter => {
        searchQueries.push(new SearchQuery({
          propertyName: filter.columnName,
          values: filter.multipleSearchValues.length > 0 ? filter.multipleSearchValues : [filter.searchValue],
          comparison: filter.searchCondition,
          nextOperand: filter.nextOperand
        }))
      })
    }
    console.log('this.ticketForm.controls.ticketStatuses.value', this.ticketForm.controls.ticketStatuses.value)
    if (this.ticketForm.controls.ticketStatuses.value !== null && this.ticketForm.controls.ticketStatuses.value !== undefined) {
      searchQueries.push(new SearchQuery({
        propertyName: 'status',
        values: [this.ticketForm.controls.ticketStatuses.value],
        comparison: 'equal',
        nextOperand: 'and'
      }))


    }
    if (this.ticketForm.controls.ticketPriority.value !== null && this.ticketForm.controls.ticketPriority.value !== undefined) {
      searchQueries.push(new SearchQuery({
        propertyName: 'priority',
        values: [this.ticketForm.controls.ticketPriority.value],
        comparison: 'equal',
        nextOperand: 'and'
      }))


    }
    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }

    if (!orderByProperty) {
      // orderByProperty = "status ASC"
      orderByProperty = "createDate DESC"
    }
    this.isLoading = true;
    let request = new GetTicketListQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.entities = response.data;
    response.totalCount ? this.tableConfigurations.pagination.totalItems = response.totalCount : '';
    this.isLoading= false;

  }
  async navigateToTicketDetails(ticket: TicketModel) {
    await this.router.navigateByUrl(`/tickets/detail?ticketId=${ticket.id}`)
  }

  add() {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(CreateTicketComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      this.get();
    })
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
