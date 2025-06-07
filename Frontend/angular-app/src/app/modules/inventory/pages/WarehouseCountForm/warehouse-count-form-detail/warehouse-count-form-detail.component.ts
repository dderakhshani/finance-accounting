import {AfterViewInit, Component, OnInit, TemplateRef, ViewChild} from '@angular/core';
import {Column, TypeFilterOptions} from "../../../../../core/components/custom/table/models/column";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {DecimalFormat} from "../../../../../core/components/custom/table/models/decimal-format";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableScrollingConfigurations} from "../../../../../core/components/custom/table/models/table-scrolling-configurations";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {ActivatedRoute, Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {WarehouseCountFormDetail} from "../../../entities/warehouse-count-form-detail";
import {GetWarehouseCountFormDetailQuery} from "../../../repositories/warehouse-count-form/quereis/get-warehouse-count-form-detail-query";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TablePaginationOptions} from "../../../../../core/components/custom/table/models/table-pagination-options";
import {GetWarehouseCountFormHeadQuery} from "../../../repositories/warehouse-count-form/quereis/get-warehouse-count-form-head-query";
import {UpdateBulkWarehouseCountQuantityCommand} from "../../../repositories/warehouse-count-form/commands/update-balk-warehouse-count-quantity";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {UpdateStateWarehouseCountFormCommand} from "../../../repositories/warehouse-count-form/commands/update-state-warehouse-count-form";
import {Toastr_Service} from "../../../../../shared/services/toastrService/toastr_.service";
import {GetWarehouseCountFormConflictQuery} from "../../../repositories/warehouse-count-form/quereis/get-warehouse-count-form-conflict-query";
import { ConfirmDialogComponent, ConfirmDialogIcons} from "../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component";
import {IdentityService} from "../../../../identity/repositories/identity.service";
import {ExcelImportDialogComponent} from "../excel-import-dialog/excel-import-dialog.component";
import {BaseTable} from 'src/app/core/abstraction/base-table';
import {PrintOptions} from "../../../../../core/components/custom/table/models/print_options";

@Component({
  selector: 'app-warehouse-count-form-detail',
  templateUrl: './warehouse-count-form-detail.component.html',
  styleUrls: ['./warehouse-count-form-detail.component.scss']
})
export class WarehouseCountFormDetailComponent extends BaseTable {
  @ViewChild('txtCountQuantity', {read: TemplateRef}) txtCountQuantity!: TemplateRef<any>;
  @ViewChild('txtDescription', {read: TemplateRef}) txtDescription!: TemplateRef<any>;
  changedQuantities: WarehouseCountFormDetail[] = [];
  WarehouseCountFormDetails: WarehouseCountFormDetail[] = [];
  headerId!: number;
  formNo!: string;
  formDate!: string;
  confirmerUser!: string;
  confirmUserId!: number;
  countUser!: string;
  warehouseLayoutTitle!: string;
  warehouseCountFormStatus!: string;
  formState!: number;
  isEditable: boolean = false;
  formStateMessage: string = "";
  countChangedQuantities: number = 0;
  currentUserId!:number;
  constructor(
    private _mediator: Mediator,
    private router: Router,
    public dialog: MatDialog,
    private route: ActivatedRoute,
    private notificationService: NotificationService,
    private toast: Toastr_Service,
    private identityService: IdentityService,
  ) {
    super(route, router);
   this.currentUserId= this.identityService.applicationUser.id;
  }
  ngOnInit(params?: any): void {
    this.resolve()
     this.headerId = this.getQueryParam('headerId')
    if ( this.headerId) {
      this.get(+ this.headerId)
    }else {
      this.get()
    }

    this.getHeader();
  }
  ngAfterViewInit() {
    const txtCountQuantityColumn = this.tableConfigurations.columns.find((col: any) => col.field === 'countedQuantity');
    if (txtCountQuantityColumn) {
      txtCountQuantityColumn.template = this.txtCountQuantity
    }
    const txtDescriptionColumn = this.tableConfigurations.columns.find((col: any) => col.field === 'description');
    if (txtDescriptionColumn) {
      txtDescriptionColumn.template = this.txtDescription
    }
    let currentUserId= this.identityService.applicationUser.id;
   setTimeout(()=>{
     this.actionBar.actions = [
       PreDefinedActions.add().setTitle('ثبت گروهی موجودی شمارش شده').setDisable(!this.isEditable).setIcon('save'),
       PreDefinedActions.save().setTitle('اتمام شمارش و ارسال برای تایید کننده').setIcon("local_post_office").setDisable(this.formState>0),
       PreDefinedActions.saveAndExit().setTitle('تایید فرم شمارش').setDisable(this.formState==0 && currentUserId!=this.confirmUserId).setIcon('save').setIcon('checked'),
       PreDefinedActions.list().setTitle('ثبت موجودی اقلام شمارش شده با فایل اکسل').setDisable(this.formState>=1).setIcon('publish')];
   })
  }

  getHeader(){
    let warehouseCountFormHead = new GetWarehouseCountFormHeadQuery();
    if(this.headerId)
      warehouseCountFormHead.id = this.headerId;
    this.tableConfigurations.options.isLoadingTable = true;
    this._mediator.send(warehouseCountFormHead).then(response => {
      this.formNo = response.formNo;
      this.confirmerUser=response.confirmerUserName;
      this.confirmUserId=response.confirmerUserId;
      this.countUser=response.counterUserName;
      this.formDate=response.formDate;
      this.warehouseLayoutTitle=response.warehouseLayoutTitle;
      this.warehouseCountFormStatus=response.formStateMessage;
      this.formState=response.formState;
      this.isEditable = response.formState == 0;
      this.formStateMessage = response.formStateMessage;
      this.tableConfigurations.printOptions.reportTitle = 'گزارش اقلام ' + this.warehouseLayoutTitle;
      this.buildPrintHeader();
      this.buildPrintFooter();
      this.ngAfterViewInit();
    });
  }
  resolve(params?: any) {
    this.columns = [
      {
        ...this.defaultColumnSettings,
        index: 1,
        field: 'id',
        title: 'شماره',
        width: 2,
        type: TableColumnDataType.Number,
        digitsInfo: DecimalFormat.None,
        filter: new TableColumnFilter('id', TableColumnFilterTypes.Number),
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,
        lineStyle: 'onlyShowFirstLine',
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
        style: {
          'text-align': 'left !important'
        }
      },
      {
        ...this.defaultColumnSettings,
        index: 3,
        field: 'commodityCode',
        title: 'کد کالا',
        width: 4,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('commodityCode', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
        style: {
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
        field: 'warehouseLayoutTitle',
        title: 'محل انبار',
        width: 3.5,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('warehouseLayoutTitle', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      },
      {
        ...this.defaultColumnSettings,
        index: 6,
        field: 'systemQuantity',
        title: 'موجودی سیستمی ',
        width: 3.5,
        type: TableColumnDataType.Number,
        digitsInfo: DecimalFormat.Default,
        filter: new TableColumnFilter('systemQuantity', TableColumnFilterTypes.Number),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,
        print: false
      },
      {
        ...this.defaultColumnSettings,
        index: 7,
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
        index: 8,
        field: 'countedQuantity',
        title: 'موجودی شمارش شده',
        width: 2,
        type: TableColumnDataType.Template,
        digitsInfo: DecimalFormat.Default,
        filter: new TableColumnFilter('countedQuantity', TableColumnFilterTypes.Number),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,
        template: this.txtCountQuantity,
      },
      {
        ...this.defaultColumnSettings,
        index: 9,
        field: 'conflictQuantity',
        title: 'مغایرت',
        width: 2,
        type: TableColumnDataType.Number,
        digitsInfo: DecimalFormat.Default,
        filter: new TableColumnFilter('conflictQuantity', TableColumnFilterTypes.Number),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,
        print: false
      },
      {
        ...this.defaultColumnSettings,
        index:10,
        field: 'description',
        title: 'توضیحات',
        width: 5,
        type: TableColumnDataType.Template,
        filter: new TableColumnFilter('description', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
        template: this.txtDescription,
        print: false
      },
    ];

    this.tableConfigurations = new TableScrollingConfigurations(this.columns, new TableOptions(false, true), new TablePaginationOptions(), this.toolBar, new PrintOptions('گزارش اقلام '))
    this.tableConfigurations.options.usePagination = true;
    this.tableConfigurations.options.showFilterRow = true;
    this.tableConfigurations.options.showTopSettingMenu = true;
    this.tableConfigurations.options.exportOptions.showExportButton = true;
    this.tableConfigurations.printOptions.hasCustomizeHeaderPage = true;
    this.selectedItemsFilterForPrint.hasCustomizeHeaderPage = true;
    this.tableConfigurations.pagination.pageSize = 5000;      }

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

  initialize(params?: any) {
    throw new Error('Method not implemented.');
  }

  add(param?: any) {
    throw new Error('Method not implemented.');
  }

  get(param?: any) {
    this.isLoading=true;
    let searchQueries: SearchQuery[] = JSON.parse(JSON.stringify(this.excludedRows));
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
    let request = new GetWarehouseCountFormDetailQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    if (this.headerId)
      request.warehouseCountFormHeadId = this.headerId;
    this.tableConfigurations.options.isLoadingTable = true;
    this._mediator.send(request).then(response => {
      this.WarehouseCountFormDetails = response.data.map((x: any) => {
        x.conflictQuantity = x.countedQuantity - x.systemQuantity
        return x;
      })
      this.checkAndHideColumnToPrint()
      response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);
      this.tableConfigurations.options.isLoadingTable = false;
      this.isLoading=false;
    }).catch(() => {
      this.tableConfigurations.options.isLoadingTable = false;
    });

  }

  checkAndHideColumnToPrint() {
    if (!this.WarehouseCountFormDetails || this.WarehouseCountFormDetails.length === 0) return;

    const shouldHide = this.WarehouseCountFormDetails.every(
      x => x.warehouseLayoutTitle && x.warehouseLayoutTitle == '.'
    );

    const column = this.tableConfigurations.columns?.find(
      col => col.field === 'warehouseLayoutTitle'
    );
    if (column) {
      column.print = !shouldHide;
    }
  }

  updateInputSearchFilters(filterConditions: { [key: string]: any }) {
    this.filterConditionsInputSearch = {...this.filterConditionsInputSearch, ...filterConditions};
    this.get()

  }

  handleOptionSelected(event: { typeFilterOptions: any, query: any }) {
    this.tableConfigurations.pagination.pageIndex = 0;
    if (event.typeFilterOptions == TypeFilterOptions.NgSelect) {

    }
    if (event.typeFilterOptions == TypeFilterOptions.NumberInputSearch || event.typeFilterOptions == TypeFilterOptions.TextInputSearch) {
      this.updateInputSearchFilters(event.query)
    }
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

  editRows($event: any) {
    if (this.isEditable) {
      return;
    }
    this.toast.showInfo('فرم در وضعیت ' + this.formStateMessage + ' قرار دارد. امکان تغییر وجود ندارد.');
  }

  async handleRemoveAllFiltersAndSorts(config: TableScrollingConfigurations) {
    this.tableConfigurations.columns = config.columns;
    this.tableConfigurations.options = config.options;
    this.selectedItemsFilterForPrint = [];
    this.excludedRows = []
    this.requestsList = this.requestsList.slice(0, 1);
    this.requestsIndex = -1;
    this.filterConditionsInputSearch = {};
    this.get()

  }

  parseFloat(val: any): any {
    if (typeof val == 'number') {
      return val;
    }
    if (val != null)
      return parseFloat(val.replace(/,/g, ''));
    else
      return null;
  }

  async changeQuantity(event: Event, row: any) {
    this.changedQuantities = this.changedQuantities.filter(x => x.id != row.id);
    row.conflictQuantity = row.countedQuantity - row.systemQuantity;
    this.changedQuantities.push(row);
    this.countChangedQuantities++
    if (this.countChangedQuantities == 50) {
      await this.updateQuantities();
      this.countChangedQuantities = 0
    }
  }

  changeDescription(event: Event, row: any) {
    this.changedQuantities = this.changedQuantities.filter(x => x.id != row.id);
    this.changedQuantities.push(row);

  }

  getQuantityStyle(row: any): { [key: string]: string } {
    const isDifferent = (a: number, b: number, tolerance = 0.0001): boolean => {
      return Math.abs(a - b) > tolerance;
    };

    return row.countedQuantity !== undefined && isDifferent(row.countedQuantity, row.systemQuantity)
      ? { 'border': 'solid 1px red' }
      : {};
  }

  async updateQuantities() {
    let request = new UpdateBulkWarehouseCountQuantityCommand();
    request.warehouseCountedQuantities = this.changedQuantities.map((x: any) => {
      return {
        id: x.id,
        countedQuantity: x.countedQuantity || 0,
        description: x.description || ""
      }
    });
    await this._mediator.send(request).then(response => {
      this.notificationService.showSuccessMessage("تعداد " + ` ${this.changedQuantities.length} ` + "موجودی شمارش شده ثبت شد");
    })
    this.changedQuantities = this.changedQuantities.splice(0, this.changedQuantities.length);
  }

  async saveQuantities() {
    await this.updateQuantities();
    this.get();
  }

  async sendForConfirm() {
    let request = new GetWarehouseCountFormConflictQuery()
    if (this.headerId)
      request.warehouseCountFormHeadId = this.headerId;
    this._mediator.send(request).then(response => {
      let conflictRows = response.data.length;
      if (conflictRows > 0 && conflictRows != undefined) {
        const dialogRef = this.dialog.open(ConfirmDialogComponent, {
          data: {
            title: 'تایید ارسال',
            message: 'تعدادی مغایرت در موجودی ها وجود دارد. آیا برای تایید کننده ارسال می کنید؟!',
            icon: ConfirmDialogIcons.warning,
            actions: {
              confirm: {title: 'بله', show: true}, cancel: {title: 'خیر', show: true}
            }
          }
        });
        dialogRef.afterClosed().subscribe(async result => {
          if (result == true) {
            await this.saveQuantities()
            this.updateStateForm(1);
            this.actionBar.actions[0].setDisable(true);
            this.actionBar.actions[1].setDisable(true);
            this.actionBar.actions[2].setDisable(false);
          }
        });
      }
      else {
        this.saveQuantities()
        this.updateStateForm(1);
        this.actionBar.actions[0].setDisable(true);
        this.actionBar.actions[1].setDisable(true);
        this.actionBar.actions[2].setDisable(false);
      }
    });
  }
  getExcelFile() {
    const dialogRef = this.dialog.open(ExcelImportDialogComponent, {
      data: {
        headerId: this.headerId
      }
    }).afterClosed().subscribe(async result => {
      this.get()
    });
  }

  updateStateForm(stateForm: number) {
    let request = new UpdateStateWarehouseCountFormCommand();
    request.id = this.headerId;
    request.warehouseStateForm = stateForm;
    this._mediator.send(request).then(response => {
      this.notificationService.showSuccessMessage("عملیات با موفقیت انجام شد.");
      this.isEditable = false;
    });
  }

  confirmForm() {
    this.updateStateForm(2);
  }
}
