import {Component, EventEmitter, Input, OnChanges, Output, SimpleChanges, TemplateRef, ViewChild} from '@angular/core';
import {TableConfigurations} from "../../../../../core/components/custom/table/models/table-configurations";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {GetCommoditiesQuery} from "../../../repositories/commodity/queries/get-commodities-query";
import {Commodity} from "../../../entities/commodity";
import {FormArray, FormControl, FormGroup} from "@angular/forms";
import {
  ConfirmDialogComponent,
  ConfirmDialogIcons
} from "../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component";
import {DeleteCommodityCommand} from "../../../repositories/commodity/commands/delete-commodity-command";


@Component({
  selector: 'app-commodity-table',
  templateUrl: './commodity-table.component.html',
  styleUrls: ['./commodity-table.component.scss']
})
export class CommodityTableComponent extends BaseComponent implements OnChanges{
  commodities: any[] = []
  tableConfigurations!: TableConfigurations;
  PageMode: string = 'search';

  //ViewChild
  @ViewChild('txtCode', { read: TemplateRef }) txtCode!: TemplateRef<any>;
  @ViewChild('txtTitle', { read: TemplateRef }) txtTitle!: TemplateRef<any>;
  @ViewChild('buttonBoms', { read: TemplateRef }) buttonBoms!: TemplateRef<any>;
  @ViewChild('buttonCopy', { read: TemplateRef }) buttonCopy!: TemplateRef<any>;
  @ViewChild('buttonEdit', { read: TemplateRef }) buttonEdit!: TemplateRef<any>;
  @ViewChild('buttonDelete', { read: TemplateRef }) buttonDelete!: TemplateRef<any>;
  // Input
  @Input()  commodityCategoryId!: number;
  @Input()   SearchForm = new FormGroup({
    commodityCode: new FormControl(),
    commodityCategoryId: new FormControl(),
    commodityNationalId: new FormControl(),
    isWrongMeasure: new FormControl(),
  });
  // output

  @Output() commodityList : EventEmitter<any | FormArray> = new EventEmitter<any | FormArray>();

  constructor(
    private router: Router,
    public dialog: MatDialog,
    private _mediator: Mediator,
    public _notificationService: NotificationService,
  ) {
    super();

  }

   async ngOnInit() {

   }
  async ngAfterViewInit() {

     await this.resolve();
   }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.commodityCategoryId && changes.commodityCategoryId.currentValue != undefined) {
      this.SearchForm.controls.commodityCategoryId.setValue(changes.commodityCategoryId.currentValue)
       this.get();
    }
  }
  async resolve() {
    let colBoms= new TableColumn(
      'bomsCount',
      'تعداد فرمول',
      TableColumnDataType.Number,
      '10%',
      true,
      new TableColumnFilter('bomsCount', TableColumnFilterTypes.Number)
    )
    let colTitle = new TableColumn(
      'title',
      'عنوان',
      TableColumnDataType.Text,
      '20%',
      true,
      new TableColumnFilter('title', TableColumnFilterTypes.Text)
    )
    let colCode =new TableColumn(
      'code',
      'کد',
      TableColumnDataType.Text,
      '15%',
      true,
      new TableColumnFilter('code', TableColumnFilterTypes.Text)
    )
    let colCopy = new TableColumn(
      'colCopy',
      'رونوشت',
      TableColumnDataType.Template,
      '5%',
      false
    );
    let colEdit = new TableColumn(
      'colEdit',
      'ویرایش',
      TableColumnDataType.Template,
      '5%',
      false
    );
    let colDelete = new TableColumn(
      'colDelete',
      'حذف',
      TableColumnDataType.Template,
      '5%',
      false
    );
    colCode.template = this.txtCode;
    colTitle.template = this.txtTitle;
    colBoms.template = this.buttonBoms;
    colCopy.template = this.buttonCopy;
    colEdit.template = this.buttonEdit;
    colDelete.template = this.buttonDelete;

    let columns: TableColumn[] = [
      new TableColumn(
       'select',
       '5%',
       TableColumnDataType.Select,
       '5%',
      ),

      colCode,
      new TableColumn(
        'compactCode',
        'کد کوتاه',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('compactCode', TableColumnFilterTypes.Text)
      ),
      colTitle,

      new TableColumn(
        'commodityCategoryTitle',
        'گروه کالا',
        TableColumnDataType.Text,
        '15%',
        true,
        new TableColumnFilter('commodityCategoryTitle',TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'isHaveWast',
        'داغی دارد؟',
        TableColumnDataType.CheckBox,
        '7%',
        true,
        new TableColumnFilter('isHaveWast', TableColumnFilterTypes.CheckBox)
      ),
      new TableColumn(
        'isHaveForceWast',
        'داغی اجباری؟',
        TableColumnDataType.CheckBox,
        '7%',
        true,
        new TableColumnFilter('isHaveForceWast', TableColumnFilterTypes.CheckBox)
      ),
      new TableColumn(
        'isActive',
        'فعال؟',
        TableColumnDataType.CheckBox,
        '7%',
        true,
        new TableColumnFilter('isActive', TableColumnFilterTypes.CheckBox)
      ),
      new TableColumn(
        'commodityNationalId',
        'شناسه ملی',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('commodityNationalId',TableColumnFilterTypes.Text)
      ),


      new TableColumn(
        'measureTitle',
        'واحد',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('measureTitle', TableColumnFilterTypes.Text)
      ),
     
      colBoms,
      colEdit,
      colCopy,
      colDelete

    ];
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    await this.get();
  }


  initialize(params?: any): any {
  }

  async get(param?: any) {
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
    if (this.SearchForm.controls.commodityCode.value != undefined && this.SearchForm.controls.commodityCode.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "code",
        values: [this.SearchForm.controls.commodityCode.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }

    if (this.SearchForm.controls.commodityNationalId.value != undefined && this.SearchForm.controls.commodityNationalId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "commodityNationalId",
        values: [this.SearchForm.controls.commodityNationalId.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.isWrongMeasure.value != undefined && this.SearchForm.controls.isWrongMeasure.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "isWrongMeasure",
        values: [this.SearchForm.controls.isWrongMeasure.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }

    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key

      })
    }
    let commodityCategoryId = this.SearchForm.controls.commodityCategoryId.value == undefined ? 0 : this.SearchForm.controls.commodityCategoryId.value;
    let request = new GetCommoditiesQuery(commodityCategoryId,this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.commodities = response.data;

    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);
  }
  async navigateToCommodity(commodity: Commodity, pageMode:string) {
    await this.router.navigateByUrl(`commodity/add?id=${commodity.id}&pageMode=${pageMode}`)
  }

  deleteFilter(){
    this.onDeleteFilter(this.SearchForm,this.tableConfigurations)
  }
  add(param?: any): any {
  }

  close(): any {
  }
  async delete(commodity: Commodity) {

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'تایید حذف / بایگانی',
        message: `آیا مطمئن به حذف  کالا  ` + commodity.title + ` می باشید؟`,
        icon: ConfirmDialogIcons.warning,
        actions: {
          confirm: { title: 'بله', show: true }, cancel: { title: 'خیر', show: true }
        }
      }
    });
    dialogRef.afterClosed().subscribe(async result => {
      if (result == true) {

        await this._mediator.send(new DeleteCommodityCommand(commodity.id)).then(res => {
          this.get();
        });
      }
    });


  }








  updateCommodityList() {
    this.commodityList.emit(this.commodities);
  }

  update(param?: any): any {
  }
}
