import {Component, EventEmitter, Input, OnInit, Output, TemplateRef, ViewChild} from '@angular/core';
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {
  TableConfigurations
} from "../../../../../../core/components/custom/table/models/table-configurations";
import {PreDefinedActions} from "../../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../core/components/custom/table/models/table-column";
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';
import { FormArray, FormGroup } from '@angular/forms';
import { NotificationService } from '../../../../../../shared/services/notification/notification.service';
import { Mediator } from '../../../../../../core/services/mediator/mediator.service';
import { Warehouse } from '../../../../../inventory/entities/warehouse';
import { TableComponent } from '../../../../../../core/components/custom/table/table.component';
import { CreateBomHeaderItemCommand } from '../../../../repositories/bom-item-headers/commands/create-bom-hearder-item-command';

@Component({
  selector: 'app-bom-headers-items',
  templateUrl: './bom-headers-items.component.html',
  styleUrls: ['./bom-headers-items.component.scss']
})
export class BomHeadersItemsComponent extends BaseComponent {
  
  @ViewChild('txtValue', { read: TemplateRef }) txtValue!: TemplateRef<any>;
  @ViewChild('btnDelete', { read: TemplateRef }) btnDelete!: TemplateRef<any>;
  @ViewChild('dropWarehouse', { read: TemplateRef }) dropWarehouse!: TemplateRef<any>;
  @ViewChild('dropDowncommodities', { read: TemplateRef }) dropDowncommodities!: TemplateRef<any>;
  
  
  tableConfigurations!: TableConfigurations;
  @Output() Additems = new EventEmitter<any>();
  @Input() set formSetter(form: FormArray) {
    if (form) {
      this.form = form;
    }

  }
  child: any;
  @ViewChild(TableComponent)
  set appShark(child: TableComponent) {
    this.child = child
  };
  constructor(private Service: PagesCommonService,
    public _notificationService: NotificationService,
    private _mediator: Mediator,) {
    super();
  }
  
  ngAfterViewInit() {

    this.resolve();

    this.actionBar.actions = [
      PreDefinedActions.add(),     
    ]
    
  }

  async resolve(params?: any) {
    
    let colCommodities = new TableColumn(
      'usedCommodityId',
      'عنوان کالا',
      TableColumnDataType.Template,
      '20%',
    );
    let colWarehouse = new TableColumn(
      'bomWarehouseId',
      'انبار',
      TableColumnDataType.Template,
      '20%',
    );
    let colvalue = new TableColumn(
      'value',
      'مقدار مصرف',
      TableColumnDataType.Template,
      '20%',
    );
   
    let colDelete = new TableColumn(
      'delete',
      'حذف',
      TableColumnDataType.Template,
      '10%',
      false
    );
    colCommodities.template = this.dropDowncommodities;
    colDelete.template = this.btnDelete;
    colvalue.template = this.txtValue;
    colWarehouse.template = this.dropWarehouse;
    let columns = [
      <TableColumn>{
        name: 'index',
        title: 'ردیف',
        type: TableColumnDataType.Index,
        width: '2.5%',
      },
      
      colCommodities,
     
      new TableColumn(
        'commodityCode',
        'کد کالا',
        TableColumnDataType.Text,
        '10%',

      ),
      new TableColumn(
        'mainMeasureTitle',
        'واحد  کالا',
        TableColumnDataType.Text,
        '10%',

      ),
      colWarehouse,
      colvalue,
      colDelete
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, false, undefined, true))
    
  }

  add(param?: any): any {
   
    this.AddItems();
   
  }
  async AddItems() {
    var values = new CreateBomHeaderItemCommand();
    var list: FormArray = <FormArray>this.form;
    list.insert(0,this.createForm(values));
    this.child.tableRows = list;
  }
  onDelete(item: any) {

    var i: number = 0;
    var j: number = 0;

    this.form.controls.forEach((control: any) => {

      if ((control as FormGroup).controls.usedCommodityId.value == item.controls.usedCommodityId.value) {
        j = i;

      }

      i = i + 1;
    })
    if (i > 0) {
      this.form.removeAt(j);

    }
    this.child.tableRows = this.form.controls;

  }
  getCommodityById(item: any, rowFg: any) {
    
    rowFg.controls.usedCommodityId.setValue(item?.id);
    rowFg.controls.mainMeasureTitle.setValue(item?.measureTitle);
    rowFg.controls.commodityCode.setValue(item?.code);

  }
  WarehouseIdSelect(item: Warehouse, rowFg: any) {

    rowFg.controls.bomWarehouseId.setValue(item?.id);

  }
  async ngOnInit() {
  }


  async ngOnChanges() {


  }
  close(): any {
  }

  delete(param?: any): any {
  }

  get(param?: any): any {
  }

  initialize(params?: any): any {
  }


  update(param?: any): any {
  }

}
