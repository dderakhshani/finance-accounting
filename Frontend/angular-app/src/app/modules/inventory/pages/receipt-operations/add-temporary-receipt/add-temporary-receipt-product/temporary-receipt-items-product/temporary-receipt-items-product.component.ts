import { Component, Input, TemplateRef, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../../../core/services/mediator/mediator.service";
import { ActivatedRoute, Router } from "@angular/router";
import {
  TableConfigurations
} from "../../../../../../../core/components/custom/table/models/table-configurations";
import { FormArray, FormGroup } from "@angular/forms";
import { Receipt } from '../../../../../entities/receipt';
import { PagesCommonService } from '../../../../../../../shared/services/pages/pages-common.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { TableColumnDataType } from "../../../../../../../core/components/custom/table/models/table-column-data-type";
import { TableOptions } from "../../../../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../../../../core/components/custom/table/models/table-column";
import { TableComponent } from '../../../../../../../core/components/custom/table/table.component';
import { TableColumnFilter } from '../../../../../../../core/components/custom/table/models/table-column-filter';
import { TableColumnFilterTypes } from '../../../../../../../core/components/custom/table/models/table-column-filter-types';


@Component({
  selector: 'app-temporary-receipt-items-product',
  templateUrl: './temporary-receipt-items-product.component.html',
  styleUrls: ['./temporary-receipt-items-product.component.scss']
})
export class TemporaryReceiptItemsProductComponent extends BaseComponent {
  child: any;
  @ViewChild(TableComponent)
  set appShark(child: TableComponent) {
    this.child = child
  };

  @ViewChild('btnDelete', { read: TemplateRef }) btnDelete!: TemplateRef<any>;
  @ViewChild('txtunitPrice', { read: TemplateRef }) txtunitPrice!: TemplateRef<any>;
  @ViewChild('txtdescription', { read: TemplateRef }) txtdescription!: TemplateRef<any>;
  @ViewChild('dropWarehouse', { read: TemplateRef }) dropWarehouse!: TemplateRef<any>;

  tableConfigurations!: TableConfigurations;


  constructor(private _mediator: Mediator,
    private route: ActivatedRoute,
    private router: Router,
    public Service: PagesCommonService,
    public dialog: MatDialog
  ) {
    super(route, router);

  }

  @Input() set formSetter(form: FormArray) {
    if (form) {
      this.form = form;

    }
  }
  @Input() receipt: Receipt | undefined = undefined;
  @Input() codeVoucherGroupId: number | undefined = undefined;
  @Input() pageModeTypeUpdate: boolean = false;

  get form1(): FormArray {
    return this.form as FormArray
  }

  async ngOnInit() {

  }
  ngAfterViewInit() {

    this.resolve()
  }


  async resolve(params?: any) {
    
    let coldropWarehouse = new TableColumn(
      'warehouseId',
      'انبار',
      TableColumnDataType.Template,
      '15%',
     
    );
    let colunitPrice = new TableColumn(
      'unitPrice',
      'قیمت تمام شده',
      TableColumnDataType.Template,
      '15%',
      true,
      new TableColumnFilter('unitPrice', TableColumnFilterTypes.Text),
    );
    
    let description = new TableColumn(
      'description',
      'شرح',
      TableColumnDataType.Template,
      '30%',
      true,
      new TableColumnFilter('description', TableColumnFilterTypes.Text),
    )
    let colDelete = new TableColumn(
      'delete',
      'حذف',
      TableColumnDataType.Template,
      '10%',
      false
    );
   
   
    colDelete.template = this.btnDelete;
    colunitPrice.template = this.txtunitPrice;
    description.template = this.txtdescription;
    coldropWarehouse.template = this.dropWarehouse;
    let columns = [
      new TableColumn('index', '', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'commodityCode',
        'کد کالا',
        TableColumnDataType.Text,
        '15%',
        true,
        new TableColumnFilter('commodityCode', TableColumnFilterTypes.Text),
      ),

      new TableColumn(
        'commodityTitle',
        'نام کالا',
        TableColumnDataType.Text,
        '15%',
        true,
        new TableColumnFilter('commodityTitle', TableColumnFilterTypes.Text),
      ),
      new TableColumn(
        'mainMeasureTitle',
        'واحد  کالا',
        TableColumnDataType.Text,
        '10%',

      ),
      new TableColumn(
        'quantity',
        'مقدار (واحد اصلی)',
        TableColumnDataType.Money,
        '7%',
        true,
        new TableColumnFilter('quantity', TableColumnFilterTypes.Text),
      ),
      
      coldropWarehouse,
      description,
    ];


    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true, undefined, true, true, true))
    this.tableConfigurations.options.showSumRow = true;
    this.tableConfigurations.pagination.pageSize = 1000

  }


 

 
  WarehouseIdSelect(item: any, rowFg: any) {

    rowFg.controls.warehouseId.setValue(item?.id);

  }
  onDelete(form: any) {

    this.Service.DeleteRow(form, this.form);
    this.child.tableRows = this.form;
  }
  add(param?: any): any {

  }

  close()
    :
    any {
  }

  delete(param?: any)
    :
    any {
  }

  get(param?: any)
    :
    any {
  }

  initialize(params?: any)
    :
    any {
  }


  update(param?: any)
    :
    any {
  }

}
