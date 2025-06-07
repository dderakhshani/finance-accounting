import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { TableConfigurations} from 'src/app/core/components/custom/table/models/table-configurations';
//import { TableColumn, TableColumnDataType, TableColumnFilter, TableColumnFilterTypes, TableConfigurations, TableOptions } from 'src/app/core/components/table/models/table-configurations';
import { FormActionTypes } from 'src/app/core/constants/form-action-types';
import { PageModes } from 'src/app/core/enums/page-modes';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { ChequeSheet } from 'src/app/modules/bursary/entities/cheque-sheet';
import { GetChqueSheetsQuery } from 'src/app/modules/bursary/repositories/cheque/queries/get-cheque-sheets-query';
import {TableColumnFilter} from "../../../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../../../core/components/custom/table/models/table-column-data-type";
import {
  TableColumnFilterTypes
} from "../../../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../../core/components/custom/table/models/table-column";
import { SearchQuery } from 'src/app/shared/services/search/models/search-query';

@Component({
  selector: 'app-receive-cheque-list',
  templateUrl: './receive-cheque-list.component.html',
  styleUrls: ['./receive-cheque-list.component.scss']
})
export class ReceiveChequeList extends BaseComponent {


  chequeSheets : ChequeSheet [] =[];

  tableConfigurations!: TableConfigurations;
  constructor(
    private _mediator: Mediator,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    super(route, router);
  }

   @Input() set formSetter(form: FormGroup) {

         this.get();
  }




  initialize(entity?:ChequeSheet): any {



  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve(params?: any) {
    this.formActions = [
      FormActionTypes.refresh,
      FormActionTypes.delete
    ];


    let columns: TableColumn[] = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        "sheetSeqNumber", "سریال چک",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter(
          "sheetSeqNumber",
          TableColumnFilterTypes.Text
        )
      ),
      new TableColumn(
        "sheetSeriNumber",
        "سری چک",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("sheetSeriNumber", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "sheetUniqueNumber",
        "شماره یکتا",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("sheetUniqueNumber", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "totalCost",
        "مبلغ",
        TableColumnDataType.Money,
        "15%",
        true,
        new TableColumnFilter("totalCost", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "branchName",
        "شعبه",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("branchName", TableColumnFilterTypes.Text)
      ),


      new TableColumn(
        "description",
        "توضیحات",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("description", TableColumnFilterTypes.Text)
      )
    ];


    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true, ''));
    this.tableConfigurations.options.showSumRow = true;


    await this.get()


  }

  async get(param?: any) {

    this.isLoading = true;

    try {



      let searchQueries: SearchQuery[] = []
      if (this.tableConfigurations.filters) {
        this.tableConfigurations.filters.forEach(filter => {
          searchQueries.push(new SearchQuery({
            propertyName: filter.columnName,
            values: filter.multipleSearchValues.length > 0 ? filter.multipleSearchValues : [filter.searchValue],
            comparison: filter.searchCondition,
            nextOperand: "and"
          }))
        })
      }

      let orderByProperty = '';
      if (this.tableConfigurations.sortKeys) {
        this.tableConfigurations.sortKeys.forEach((key, index) => {
          orderByProperty += index ? `,${key}` : key
        })
      }


      searchQueries.push(new SearchQuery({
        propertyName: 'IsUsed',
        values: [false],
        comparison: 'equal'
      }))


      var response = await this._mediator.send(new GetChqueSheetsQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty));
      this.isLoading = false;
      this.chequeSheets = response.data;
      // this.form = new FormArray(this.financialRequests.map((x) => this.createForm(x)));

      // this.calcSelectedSum();
      response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

    }
    catch {
      this.isLoading = false;
    }

  }




  add(param?: any) {
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
