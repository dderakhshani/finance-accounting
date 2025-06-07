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
import { TableComponent } from '../../../../../../core/components/custom/table/table.component';
import { CreateBomItemCommand } from '../../../../repositories/bom-item/commands/create-bom-item-command';

@Component({
  selector: 'app-bom-items',
  templateUrl: './bom-items.component.html',
  styleUrls: ['./bom-items.component.scss']
})
export class BomItemsComponent extends BaseComponent {
  
  @ViewChild('dropDowncommodities', { read: TemplateRef }) dropDowncommodities!: TemplateRef<any>;
  @ViewChild('dropDownCommodityCategoryId', { read: TemplateRef }) dropDownCommodityCategoryId!: TemplateRef<any>;
  @ViewChild('btnDelete', { read: TemplateRef }) btnDelete!: TemplateRef<any>;
 

  tableConfigurations!: TableConfigurations;

  isLoadingPage: number = 0;
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
  get form1(): FormArray {
    return this.form as FormArray
  }
  constructor(private Service: PagesCommonService, public _notificationService: NotificationService,) {
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
      'commodityId',
      'عنوان کالا',
      TableColumnDataType.Template,
      '60%',
    );
   
    let colSubCategoryId = new TableColumn(
      'subCategoryId',
      'گروه محصول',
      TableColumnDataType.Template,
      '25%',
    );
   
    let colDelete = new TableColumn(
      'delete',
      'حذف',
      TableColumnDataType.Template,
      '5%',
      false
    );
    colCommodities.template = this.dropDowncommodities;
    colDelete.template = this.btnDelete;
    colSubCategoryId.template = this.dropDownCommodityCategoryId;
    let columns = [
      <TableColumn>{
        name: 'index',
        title: 'ردیف',
        type: TableColumnDataType.Index,
        width: '2.5%',
      },
      //colSubCategoryId,
      colCommodities,
      <TableColumn>{
        name: 'commodityCode',
        title: 'کد کالا',
        type: TableColumnDataType.Text,
        width: '30%',
      },
      colDelete
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, false, undefined, true))
    
   
  }

  add(param?: any): any {
    this.AddItems();
   
  }
  async AddItems() {
    
    var values = new CreateBomItemCommand();
    var list: FormArray = <FormArray>this.form;
    list.push(this.createForm(values, true));
    this.child.tableRows = this.form;
  }
  
  getCommodityById(item: any, rowFg: any) {
    
    
    rowFg.controls.commodityId.setValue(item?.id);
    rowFg.controls.commodityCode.setValue(item?.code);
    
    

  }
  CommodityCategoryIdSelect(id: any, rowFg: any) {
    rowFg.controls.subCategoryId.setValue(id);
  }
 
  onDelete(item: any) {


    var i: number = 0;
    var j: number = 0;

    this.form.controls.forEach((control: any) => {

      if ((control as FormGroup).controls.commodityId.value == item.controls.commodityId.value) {
        j = i;

      }

      i = i + 1;
    })
    if (i > 0) {
      this.form.removeAt(j);

    }
    
    this.child.tableRows = this.form.controls;

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
