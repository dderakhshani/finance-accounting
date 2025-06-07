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
import { AddCommoditySerialDialog } from '../../../../component/commodity-serial-dialog/add-commodity-serial-dialog.component';
import { Assets, AssetsSerial } from '../../../../../entities/Assets';
import { GetAssetsByDocumentIdQuery } from '../../../../../repositories/assets/queries/get-assets-by-documentId';
import { TableColumnDataType } from "../../../../../../../core/components/custom/table/models/table-column-data-type";
import { TableOptions } from "../../../../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../../../../core/components/custom/table/models/table-column";
import { TableComponent } from '../../../../../../../core/components/custom/table/table.component';
import { PageModes } from '../../../../../../../core/enums/page-modes';
import { UpdateQuantityDialogComponent } from '../../../../component/update-quantity-dialog/update-quantity-dialog.component';
import { TableColumnFilter } from '../../../../../../../core/components/custom/table/models/table-column-filter';
import { TableColumnFilterTypes } from '../../../../../../../core/components/custom/table/models/table-column-filter-types';


@Component({
  selector: 'app-temporary-receipt-items-single-row',
  templateUrl: './temporary-receipt-items-single-row.component.html',
  styleUrls: ['./temporary-receipt-items-single-row.component.scss']
})
export class TemporaryReceiptItemsSingleRowComponent extends BaseComponent {
  child: any;
  @ViewChild(TableComponent)
  set appShark(child: TableComponent) {
    this.child = child
  };

  @ViewChild('btnDelete', { read: TemplateRef }) btnDelete!: TemplateRef<any>;
  @ViewChild('btnSerial', { read: TemplateRef }) btnSerial!: TemplateRef<any>;

  @ViewChild('txtquantity', { read: TemplateRef }) txtquantity!: TemplateRef<any>;
  @ViewChild('checkboxWrong', { read: TemplateRef }) checkboxWrong!: TemplateRef<any>;

  @ViewChild('txtdescription', { read: TemplateRef }) txtdescription!: TemplateRef<any>;
  @ViewChild('txtcommodityCode', { read: TemplateRef }) txtcommodityCode!: TemplateRef<any>;
  @ViewChild('dropDowncommodities', { read: TemplateRef }) dropDowncommodities!: TemplateRef<any>;

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
  @Input() codeVoucherGroup: string | undefined = undefined;
  @Input() pageModeTypeUpdate: boolean = false;
  @Input() EditType: Number | undefined = undefined;
  get form1(): FormArray {
    return this.form as FormArray
  }

  async ngOnInit() {

  }
  async ngOnChanges() {

    this.resolve()

  }


  async resolve(params?: any) {
    let colCommodities: any;
    if (this.pageModeTypeUpdate) {
      colCommodities = new TableColumn(
        'commodityId',
        'عنوان کالا',
        TableColumnDataType.Template,
        '25%',


      );
    } else {
      colCommodities = new TableColumn(
        'commodityTitle',
        'نام کالا',
        TableColumnDataType.Text,
        '18%',
        true,
        new TableColumnFilter('commodityTitle', TableColumnFilterTypes.Text),
      );

    }
    let colcommodityCode = new TableColumn(
      'commodityCode',
      'کد کالا',
      TableColumnDataType.Template,
      '15%',
      true,
      new TableColumnFilter('commodityCode', TableColumnFilterTypes.Text),
    )


    let quantity = new TableColumn(
      'quantity',
      'مقدار (واحد اصلی)',
      TableColumnDataType.Template,
      '7%',
      true,
      new TableColumnFilter('quantity', TableColumnFilterTypes.Text),
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
    let colSerial = new TableColumn(
      'serial',
      'کد سریال',
      TableColumnDataType.Template,
      '10%',
      false
    );
    let colWrongMeasure = new TableColumn(
      'isWrongMeasure',
      'اشتباه در واحد کالا',
      TableColumnDataType.Template,
      '7%',
    );


    colDelete.template = this.btnDelete;
    colSerial.template = this.btnSerial;
    quantity.template = this.txtquantity;
    description.template = this.txtdescription;
    colWrongMeasure.template = this.checkboxWrong;
    colcommodityCode.template = this.txtcommodityCode;
    colCommodities.template = this.dropDowncommodities;
    let columns = [
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '3%'),
      colCommodities,
      colcommodityCode,

      new TableColumn(
        'mainMeasureTitle',
        'واحد  کالا',
        TableColumnDataType.Text,
        '10%',

      ),
      quantity,
      description,
      colSerial,
      colDelete,
      colWrongMeasure,

    ];
    let tableOptions = new TableOptions(false, true, undefined, true,true,true);

    this.tableConfigurations = new TableConfigurations(columns, tableOptions)

  }


  onDelete(form: any) {

    this.Service.DeleteRow(form, this.form);
    this.child.tableRows = this.form;
  }

  //===========================================================================
  //----------------------در سندهای مربوط به اموال شناسه های اموال دریافت شود
  async addCommoditySerials(rowFg: any) {

    let dialogConfig = new MatDialogConfig();
    let assetsList: any = undefined;

    //اگر در حالت ویرایش باشیم.
    if (rowFg.controls['assets'].value == undefined) {
      await this._mediator.send(new GetAssetsByDocumentIdQuery(rowFg.controls.id.value, rowFg.controls.commodityId.value)).then(res => {

        assetsList = res
      });
    }

    dialogConfig.data = {
      commodityCode: rowFg.controls.commodityCode.value,
      commodityId: rowFg.controls.commodityId.value,
      commodityTitle: rowFg.controls.commodityTitle.value,
      quantity: rowFg.controls.quantity.value,
      documentItemId: rowFg.controls.id.value,
      assets: assetsList != undefined ? assetsList : rowFg.controls['assets'].value

    };

    let dialogReference = this.dialog.open(AddCommoditySerialDialog, dialogConfig);
    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      var Assets: Assets = response;

      if (Assets.assetsSerials != undefined) {
        Assets?.assetsSerials.forEach(item => {

          rowFg.controls['assets'].setValue(Assets);

        })
        rowFg.controls.commoditySerial.setValue(response.length);
        rowFg.controls['quantity'].disable();
      }


    })
  }
  onWrongMeasure(rowFg: any) {
    var _value = rowFg.controls.isWrongMeasure.value;
    rowFg.controls.isWrongMeasure.setValue(!_value);
  }
  CommoditySelect(item: any, rowFg: any) {

    rowFg.controls.commodityId.setValue(item?.id);
    rowFg.controls.mainMeasureId.setValue(item?.measureId);
    rowFg.controls.mainMeasureTitle.setValue(item?.measureTitle);
    rowFg.controls.commodityCode.setValue(item?.code);
    rowFg.controls.documentMeasureId.setValue(item?.measureId);
  }
  async updateQuantity(item: any, rowFg: any) {

    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      DocumentItem: item,

      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(UpdateQuantityDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      if (response) {
        rowFg.controls.quantity.setValue(response.quantity);
      }
    })

  }
  async navigateToHistory(id: number) {

    var url = `inventory/commodityReceiptReports?commodityId=${id}&warehouseId=${this.receipt?.warehouseId}`
    this.router.navigateByUrl(url)

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
