import { Component, OnInit } from '@angular/core';
import {TypeFilterOptions} from "../../../../../../core/components/custom/table/models/column";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {ActivatedRoute, Router} from "@angular/router";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilter} from "../../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {TablePaginationOptions} from "../../../../../../core/components/custom/table/models/table-pagination-options";
import {SearchQuery} from "../../../../../../shared/services/search/models/search-query";
import {GetWarehouseCountReportQuery} from "../../../../repositories/warehouse-count-form/quereis/get-warehouse-count-report-query";
import {TableScrollingConfigurations} from "../../../../../../core/components/custom/table/models/table-scrolling-configurations";
import {PrintOptions} from "../../../../../../core/components/custom/table/models/print_options";
import {BaseTable} from "../../../../../../core/abstraction/base-table";
import {DecimalFormat} from "../../../../../../core/components/custom/table/models/decimal-format";
import {GetChildWarehouseCountFormHeadQuery} from "../../../../repositories/warehouse-count-form/quereis/get-child-warehouse-count-form-head-query";
import {AddReceiptCommand} from "../../../../repositories/receipt/commands/temporary-receipt/add-receipt-command";
import { GetALLCodeVoucherGroupsQuery} from "../../../../repositories/receipt/queries/receipt/get-code-voucher-groups-query";
import {GetWarehouseLayoutQuery} from "../../../../repositories/warehouse-layout/queries/get-warehouse-layout-query";
import {AddItemsCommand} from "../../../../repositories/receipt/commands/receipt-items/add-receipt-items-command";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {UpdateStateWarehouseCountFormCommand} from "../../../../repositories/warehouse-count-form/commands/update-state-warehouse-count-form";
import {ConfirmDialogComponent, ConfirmDialogIcons} from "../../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component";
import {MatDialog} from "@angular/material/dialog";

@Component({
  selector: 'app-warehouse-count-report',
  templateUrl: './warehouse-count-report.component.html',
  styleUrls: ['./warehouse-count-report.component.scss']
})
export class WarehouseCountReportComponent  extends BaseTable {
  rowData: any[] = [];
  warehouseLayoutTitle!:string;
  warehouseLayoutId!:number;
  lastCountFormHeaderId!:number;
  formNo!:string;
  errorMessage = '';
  stateCount!:Number;
  warehouseId:number=0;
  codeVoucheGroups=[]=[];
  allRows=[]=[];
  stepIndex = 0;
  showStepList=false;
  isCompleted = false;
  stepList = [
    { label: 'شروع', description: 'Start...' },
    { label: 'انتخاب مغایرت های بزرگتر از 0.4 و کوچکتر از -0.4', description: 'Getting rows...' },
    { label: 'ثبت سند ورود کالا به انبار', description: 'Creating inventory enter request...' },
    { label: 'ثبت سند خروج کالا از انبار', description: 'Creating inventory exit request...' },
    { label: 'تایید نهایی', description: 'Unlocking inventory and change formState...' }
  ];

   constructor(
    private _mediator: Mediator,
    public dialog: MatDialog,
    private router: Router,
    private route: ActivatedRoute,
    private notificationService: NotificationService,)
  {
    super(route, router);
  }
  async resolve(params?: any) {
    await this.generateColumn();

  }
async generateColumn() {
  this.columns = [
    {
      ...this.defaultColumnSettings,
      index: 0,
      field: 'rowIndex',
      title: 'ردیف',
      width: 1,
      type: TableColumnDataType.Index,
      isDisableDrop: true,
      lineStyle: 'onlyShowFirstLine',
    },
    {
      ...this.defaultColumnSettings,
      index: 1,
      field: 'id',
      title: 'شماره',
      width: 2,
      type: TableColumnDataType.Number,
      digitsInfo: DecimalFormat.None ,
      filter: new TableColumnFilter('id', TableColumnFilterTypes.Number),
      typeFilterOptions: TypeFilterOptions.NumberInputSearch,
      lineStyle: 'onlyShowFirstLine',
      display:false
    },
    {
      ...this.defaultColumnSettings,
      index: 2,
      field: 'commodityCompactCode',
      title: 'کد کوتاه کالا',
      width: 3,
      type: TableColumnDataType.Text,
      filter: new TableColumnFilter('commodityCompactCode', TableColumnFilterTypes.Text),
      lineStyle: 'onlyShowFirstLine',
      typeFilterOptions: TypeFilterOptions.TextInputSearch,
      style:{
        'text-align': 'left !important'
      }
    },
    {
      ...this.defaultColumnSettings,
      index: 3,
      field: 'commodityCode',
      title: 'کد کالا',
      width: 3,
      type: TableColumnDataType.Text,
      filter: new TableColumnFilter('commodityCode', TableColumnFilterTypes.Text),
      lineStyle: 'onlyShowFirstLine',
      typeFilterOptions: TypeFilterOptions.TextInputSearch,
      style:{
        'text-align': 'left !important'
      }
    },
    {
      ...this.defaultColumnSettings,
      index: 4,
      field: 'commodityName',
      title: 'نام کالا',
      width: 3.5,
      type: TableColumnDataType.Text,
      filter: new TableColumnFilter('commodityName', TableColumnFilterTypes.Text),
      lineStyle: 'onlyShowFirstLine',
      typeFilterOptions: TypeFilterOptions.TextInputSearch
    },
    {
      ...this.defaultColumnSettings,
      index: 5,
      field: 'measureTitle',
      title: 'واحد',
      width: 2,
      type: TableColumnDataType.Text,
      digitsInfo: DecimalFormat.TwoDecimals,
      filter: new TableColumnFilter('measureTitle', TableColumnFilterTypes.Text),
      lineStyle: 'onlyShowFirstLine',
      typeFilterOptions: TypeFilterOptions.TextInputSearch,
    },
    {
      ...this.defaultColumnSettings,
      index: 6,
      field: 'warehouseLayoutTitle',
      title: 'نام انبار',
      width: 3.5,
      type: TableColumnDataType.Text,
      filter: new TableColumnFilter('warehouseLayoutTitle', TableColumnFilterTypes.Text),
      lineStyle: 'onlyShowFirstLine',
      typeFilterOptions: TypeFilterOptions.TextInputSearch,
    },
    {
      ...this.defaultColumnSettings,
      index: 7,
      field: 'systemQuantity',
      title: 'موجودی سیستمی ',
      width: 2,
      type: TableColumnDataType.Number,
      digitsInfo: DecimalFormat.Default,
      filter: new TableColumnFilter('systemQuantity', TableColumnFilterTypes.Number),
      lineStyle: 'onlyShowFirstLine',
      typeFilterOptions: TypeFilterOptions.NumberInputSearch
    },
    {
      ...this.defaultColumnSettings,
      index: 8,
      field: 'countedQuantity1',
      title: 'شمارش 1',
      width: 2,
      type: TableColumnDataType.Number,
      digitsInfo: DecimalFormat.Default,
      filter: new TableColumnFilter('countedQuantity1', TableColumnFilterTypes.Number),
      lineStyle: 'onlyShowFirstLine',
     // display : false,
      typeFilterOptions: TypeFilterOptions.NumberInputSearch
    },
    {
      ...this.defaultColumnSettings,
      index: 9,
      field: 'countedQuantity2',
      title: 'شمارش 2',
      width: 2,
      type: TableColumnDataType.Number,
      digitsInfo: DecimalFormat.Default,
      filter: new TableColumnFilter('countedQuantity2', TableColumnFilterTypes.Number),
      lineStyle: 'onlyShowFirstLine',
      display : false,
      typeFilterOptions: TypeFilterOptions.NumberInputSearch
    },
    {
      ...this.defaultColumnSettings,
      index: 10,
      field: 'countedQuantity3',
      title: 'شمارش 3',
      width: 2,
      type: TableColumnDataType.Number,
      digitsInfo: DecimalFormat.Default,
      filter: new TableColumnFilter('countedQuantity3', TableColumnFilterTypes.Number),
      lineStyle: 'onlyShowFirstLine',
      display : false,
      typeFilterOptions: TypeFilterOptions.NumberInputSearch
    },
    {
      ...this.defaultColumnSettings,
      index: 11,
      field: 'countedQuantity4',
      title: 'شمارش 4',
      width: 2,
      type: TableColumnDataType.Number,
      digitsInfo: DecimalFormat.Default,
      filter: new TableColumnFilter('countedQuantity4', TableColumnFilterTypes.Number),
      lineStyle: 'onlyShowFirstLine',
      display : false,
      typeFilterOptions: TypeFilterOptions.NumberInputSearch
    },
    {
      ...this.defaultColumnSettings,
      index: 12,
      field: 'countedQuantity5',
      title: 'شمارش 5',
      width: 2,
      type: TableColumnDataType.Number,
      digitsInfo: DecimalFormat.Default,
      filter: new TableColumnFilter('countedQuantity5', TableColumnFilterTypes.Number),
      lineStyle: 'onlyShowFirstLine',
      typeFilterOptions: TypeFilterOptions.NumberInputSearch,
      display : false,
    },
    {
      ...this.defaultColumnSettings,
      index: 13,
      field: 'finalQuantity',
      title: 'موجودی نهایی',
      width: 2,
      type: TableColumnDataType.Number,
      digitsInfo: DecimalFormat.Default,
      filter: new TableColumnFilter('finalQuantity', TableColumnFilterTypes.Number),
      lineStyle: 'onlyShowFirstLine',
      typeFilterOptions: TypeFilterOptions.NumberInputSearch
    },
    {
      ...this.defaultColumnSettings,
      index: 14,
      field: 'discrepancyQuantity',
      title: 'مغایرت موجودی',
      width: 2,
      type: TableColumnDataType.Number,
      digitsInfo: DecimalFormat.Default,
      filter: new TableColumnFilter('discrepancyQuantity', TableColumnFilterTypes.Number),
      lineStyle: 'onlyShowFirstLine',
      typeFilterOptions: TypeFilterOptions.NumberInputSearch
    }
  ];
  this.tableConfigurations = new TableScrollingConfigurations(this.columns, new TableOptions(false, true), new TablePaginationOptions(), this.toolBar, new PrintOptions('گزارش شمارش انبار '));
  this.tableConfigurations.options.usePagination = true;
  this.tableConfigurations.options.showTopSettingMenu = true;

  let warehouseCountFormHead = new GetChildWarehouseCountFormHeadQuery()
  warehouseCountFormHead.parentId = this.getQueryParam('headerId');
  await this._mediator.send(warehouseCountFormHead).then(async response => {
    if (response.length > 0) {
      this.warehouseLayoutTitle=response[0].warehouseLayoutTitle;
      this.warehouseLayoutId=response[0].warehouseLayoutId;
      this.tableConfigurations.printOptions.reportTitle = 'گزارش شمارش انبار '+this.warehouseLayoutTitle;
      this.lastCountFormHeaderId=response[response.length-1].id;
      this.formNo=response[response.length-1].formNo;
      this.stateCount=response[response.length-1].formState;
      this.tableConfigurations.columns.forEach(column => {
        if (column.index >= 9 && column.index < 13) {
          const maxActiveIndex = 8 + response.length;
          const isActive = column.index < maxActiveIndex;
          column.display = isActive;
          if (!isActive) {
            this.tableConfigurations.columns = this.tableConfigurations.columns.filter(c => c.index !== column.index);
          }
        }
      });
      this.buildPrintHeader();
      this.buildPrintFooter();
      setTimeout(async () => {
        await this.get()
      },0)
    }
  });
}
  buildPrintFooter() {
    this.tableConfigurations.printOptions.customizeHtmlFooter = `
    <table   style="border: solid gray 1px;" class=" pdf-table">
     <tr style="height: 60px;">
        <td style="width: 50%;border:solid gray 1px"> <p style="text-align: right; padding-right: 20px">توضیحات:</p></td>
        <td style="width: 50%;border:solid gray 1px"><p style="text-align: right; padding-right: 20px">نام و امضاء شمارش کننده</p></td>
      </tr>
       </table>
       <table style="border: solid gray 1px" class=" pdf-table">
      <tr style="height: 60px">
       <td style="width: 50%;border:solid gray 1px;text-align: right;"><p>امضاء رئیس انبار</p></td>
        <td style="width: 50%;border:solid gray 1px;text-align: right;"><p>امضاء نماینده حسابداری</p></td>
</tr>
    </table>
 `
  }

  buildPrintHeader() {
    this.tableConfigurations.printOptions.customizeHtmlHeader = `
    <div class="header-container" >
      <div class="header-right" style="align-items: center !important;">
        <div class="fig-logo">
          <img src="/assets/images/Reportlogo.jpg" class="logo" alt="لوگو"><br/>
        </div>
          <p style="text-align: center">شرکت ایفا سرام</p>
          <p style="text-align: center">سیستم مدیریت یکپارچه</p>
      </div>
      <div class="header-right" >
        <p ><b>گزارش شمارش اقلام</b></p>
      </div>
      <div class="header-left" >
        <p class="date-report">تاریخ چاپ گزارش: ${new Date().toLocaleDateString('fa-IR')}</p>
        <p class="current-user" >شماره : ${this.formNo}</p>
      </div>
    </div>`;
  }

   initialize(params?: any) {
     throw new Error('Method not implemented.');

  }
  add(param?: any) {
      throw new Error('Method not implemented.');
  }
  async get(param?: any) {
    let searchQueries: SearchQuery[]=JSON.parse(JSON.stringify(this.excludedRows));
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
    if (this.filterConditionsInputSearch) {
      Object.keys(this.filterConditionsInputSearch).forEach(key => {
        const filter = this.filterConditionsInputSearch[key];
        if (filter && filter.propertyNames && filter.searchValues && filter.searchValues[0]) {
          filter.propertyNames.forEach((propertyName: string) => {
            searchQueries.push(new SearchQuery({
              propertyName: propertyName,
              values: filter.searchValues,
              comparison: filter.searchCondition,
              nextOperand: filter.nextOperand
            }));
          });
        }
      });
    }
    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }
    let request = new GetWarehouseCountReportQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
      request.warehouseCountFormHeadId = this.getQueryParam('headerId');
     this.tableConfigurations.options.isLoadingTable = true;
     await  this._mediator.send(request).then(response => {
       this.rowData=response.data;
       response.totalCount ? this.tableConfigurations.pagination.totalItems = response.totalCount :this.rowData.length ;
       this.tableConfigurations.options.isLoadingTable = false;

    }).catch(() => {
      this.tableConfigurations.options.isLoadingTable = false;
    });
  }

  async getAllRow(): Promise<any> {
    let searchQueries: SearchQuery[] = JSON.parse(JSON.stringify(this.excludedRows));
    searchQueries.push(
      {
        propertyName: "discrepancyQuantity",
        values: ['0.4'],
        comparison: ">=",
        nextOperand: "or"
      },
      {
        propertyName: "discrepancyQuantity",
        values: ['-0.4'],
        comparison: "<=",
        nextOperand: "or"
      }
    );

    let request = new GetWarehouseCountReportQuery(0, 0, searchQueries);
    request.warehouseCountFormHeadId = this.getQueryParam('headerId');
    const response = await this._mediator.send(request);
    this.allRows=response.data;
    return response.data;
  }

  async getCodeVoucherGroups(): Promise<any> {
    let codeVoucherRequest = new GetALLCodeVoucherGroupsQuery();
    const response = await this._mediator.send(codeVoucherRequest);
    return response.data;
  }

  async getWarehouseId(): Promise<any> {
    let warehouseLayout = new GetWarehouseLayoutQuery(this.warehouseLayoutId);
    const response = await this._mediator.send(warehouseLayout);
    return response.warehouseId;
  }

  async addDeductionAndExtraForm() {
     this.showStepList=true
    this.warehouseId=await this.getWarehouseId();
    this.codeVoucheGroups=await  this.getCodeVoucherGroups();
  }
  async nextStep() {
    if (this.stepIndex >= this.stepList.length) return;
    this.isLoading = true;
    this.errorMessage = '';
    try {
      switch (this.stepIndex) {
        case 0: await this.addDeductionAndExtraForm(); break;
        case 1: await this.getAllRow(); break;
        case 2: await this.createInventoryEnterRequest(this.codeVoucheGroups,this.allRows,this.warehouseId); break;
        case 3: await this.createInventoryExitRequest(this.codeVoucheGroups,this.allRows,this.warehouseId); break;
        case 4:  await this.updateFormState(4, this.lastCountFormHeaderId);  break;
      }

      this.stepIndex++;
      if (this.stepIndex === this.stepList.length) {
        this.isCompleted = true;
      }
    } catch (err: any) {
      this.errorMessage = err.message || 'Something went wrong!';
    } finally {
      this.isLoading = false;
    }
  }

  // prevStep() {
  //   if (this.stepIndex > 0) {
  //     this.stepIndex--;
  //   }
  // }
  // async startProcess() {
  //   this.isLoading = true;
  //   this.errorMessage = '';
  //   try {
  //     this.currentStep = Step.CalculatingRows;
  //    let rows = await this.getAllRow();
  //
  //     this.currentStep = Step.GettingVouchers;
  //     let voucherGroups = await this.getCodeVoucherGroups();
  //
  //     // this.currentStep = Step.GettingWarehouseId;
  //     // let warehouseId = await this.getWarehouseId();
  //
  //     this.currentStep = Step.CreatingEnterRequest;
  //     await this.createInventoryEnterRequest(voucherGroups,rows, this.warehouseId);
  //
  //     this.currentStep = Step.CreatingExitRequest;
  //     await this.createInventoryExitRequest(voucherGroups, rows, this.warehouseId);
  //
  //     this.currentStep = Step.Done;
  //   } catch (error: any) {
  //     this.errorMessage = error?.message || 'خطایی رخ داده است.';
  //     this.currentStep = Step.Failed;
  //   } finally {
  //     this.isLoading = false;
  //   }
  // }

  async createInventoryExitRequest(voucherGroup:any,rows:any[],warehouseId:number) {
    let exitWarehouse = rows.filter(x => x.discrepancyQuantity < -0.4);
    if (exitWarehouse != undefined && exitWarehouse.length > 0) {
      let requestExit = new AddReceiptCommand();
      let voucherGroupExit=voucherGroup.filter((x:any)=>x.code=='5873')[0] //اصلاح موجودی خروج از انبار
      requestExit.codeVoucherGroupId = voucherGroupExit.id;
      requestExit.documentStauseBaseValue = voucherGroupExit.viewId; //اصلاح موجودی
      requestExit.menueId = 1282;
      requestExit.documentDate = new Date();
      requestExit.warehouseId = warehouseId;
      requestExit.receiptDocumentItems = exitWarehouse.map((x: any) => {
        return {
          commodityId: x.commodityId,
          quantity: Math.abs(x.discrepancyQuantity),
        } as AddItemsCommand
      });

     return await this._mediator.send(requestExit).then(resExit => {
        this.notificationService.showSuccessMessage("سند خروج انبار برای کالاها صادر شد.");
      });
    }
  }

  async createInventoryEnterRequest(voucherGroup:any,rows:any[],warehouseId:number) {
    let enterWarehouse = rows.filter(x => x.discrepancyQuantity > 0.4);
    if (enterWarehouse != undefined && enterWarehouse.length > 0) {
    let requestEnter = new AddReceiptCommand();
    let voucherGroupEnter=voucherGroup.filter((x:any)=>x.code=='5823')[0] //اصلاح موجودی ورود به انبار

    requestEnter.codeVoucherGroupId = voucherGroupEnter.id;
    requestEnter.viewId = voucherGroupEnter.viewId;
    requestEnter.menueId = 1282;
    requestEnter.documentStauseBaseValue = voucherGroupEnter.viewId;   //اصلاح موجودی
    requestEnter.documentDate = new Date();
    requestEnter.warehouseId = warehouseId;
    requestEnter.receiptDocumentItems = enterWarehouse.map((x: any) => {
        return {
          commodityId: x.commodityId,
          quantity: Math.abs(x.discrepancyQuantity),
        } as AddItemsCommand
      });
     return await this._mediator.send(requestEnter).then(resEnter => {
        this.notificationService.showSuccessMessage("سند ورود به انبار برای کالاها صادر شد.");
      })
    }
  }

  async updateFormState(formState: number,headerId:number) {
    let request = new UpdateStateWarehouseCountFormCommand();
    request.id =  headerId;
    request.warehouseStateForm = formState;
   await this._mediator.send(request).then(response => {
      this.stateCount=formState;
    });
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

  async ngOnInit(): Promise<void> {
   await this.resolve();
  }
  countCancellation() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'تایید ابطال شمارش',
        message: 'آیا از ابطال شمارش مطمئن هستید؟',
        icon: ConfirmDialogIcons.warning,
        actions: {
          confirm: {title: 'بله', show: true}, cancel: {title: 'خیر', show: true}
        }
      }
    });

    dialogRef.afterClosed().subscribe(async result => {
      if (result == true) {
        this.isLoading=true;
      await  this.updateFormState(5,this.getQueryParam('headerId')); //ابطال شمارش
        this.isLoading=false;
        return this.router.navigateByUrl(`/inventory/WarehouseCountFormList`);
      }
      else
        return
    });
  }

}
