import {Component, Input, TemplateRef, ViewChild} from '@angular/core';
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {FormArray, FormGroup} from "@angular/forms";
import {
  CreateCommodityCategoryPropertyItemCommand
} from "../../../../repositories/commodity-category-property-item/commands/create-commodity-category-property-item-command";
import {
  TableConfigurations
} from "../../../../../../core/components/custom/table/models/table-configurations";
import {PreDefinedActions} from "../../../../../../core/components/custom/action-bar/action-bar.component";

import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../core/components/custom/table/models/table-column";
import { TableComponent } from '../../../../../../core/components/custom/table/table.component';
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';

@Component({
  selector: 'app-commodity-category-property-item',
  templateUrl: './commodity-category-property-item.component.html',
  styleUrls: ['./commodity-category-property-item.component.scss']
})
export class CommodityCategoryPropertyItemComponent extends BaseComponent {

  public items: any[] = [];

  @ViewChild('txtTitle', { read: TemplateRef }) txtTitle!: TemplateRef<any>;
  @ViewChild('txtUniqueName', { read: TemplateRef }) txtUniqueName!: TemplateRef<any>;
  @ViewChild('txtCode', { read: TemplateRef }) txtCode!: TemplateRef<any>;
  @ViewChild('txtOrderIndex', { read: TemplateRef }) txtOrderIndex!: TemplateRef<any>;
  @ViewChild('txtActive', { read: TemplateRef }) txtActive!: TemplateRef<any>;
  @ViewChild('txtParentId', { read: TemplateRef }) txtParentId!: TemplateRef<any>;
  @ViewChild('btnDelete', { read: TemplateRef }) btnDelete!: TemplateRef<any>;

  

  tableConfigurations!: TableConfigurations;
  child: any;
  @ViewChild(TableComponent)
  set appShark(child: TableComponent) {
    this.child = child
  };
  @Input() set formSetter(form: FormArray) {

   
    this.form = form;
    this.form.controls.forEach((control: FormGroup) => {

      this.items.push(
        {
        id: control.controls.id.value,
        title: control.controls.title.value
        }
      )
     
    })
  }

  constructor(public Service: PagesCommonService) {
  
    super()
  }

  async ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.add().setTitle('افزودن مقادیر'),
      /*PreDefinedActions.delete(),*/
    ]
    await this.resolve();
  }


  async ngOnInit() {
    

  }

  async resolve(params?: any) {
    
    let colTitle = new TableColumn(
      'title',
      'عنوان',
      TableColumnDataType.Template,
      '20%',
    );
    let colUniqueName = new TableColumn(
      'uniqueName',
      'نام یکتا',
      TableColumnDataType.Template,
      '20%',
    );
    let colCode = new TableColumn(
      'code',
      'کد',
      TableColumnDataType.Template,
      '10%',
    );
    let colOrderIndex = new TableColumn(
      'orderIndex',
      'ترتیب نمایش',
      TableColumnDataType.Template,
      '10%',
    );
    let colIsActive = new TableColumn(
      'isActive',
      'فعال است',
      TableColumnDataType.Template,
      '10%',
    );
    let colParentId = new TableColumn(
      'parentId',
      'سرگروه',
      TableColumnDataType.Template,
      '20%',
    );
    let colDelete = new TableColumn(
      'delete',
      'حذف',
      TableColumnDataType.Template,
      '10%',
    );
   
    colTitle.template = this.txtTitle;
    colUniqueName.template = this.txtUniqueName;
    colCode.template = this.txtCode;
    colOrderIndex.template = this.txtOrderIndex;
    colIsActive.template = this.txtActive;
    colParentId.template = this.txtParentId;
    colDelete.template = this.btnDelete;
    let columns = <TableColumn[]>[
     
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      colTitle,
      colUniqueName,
      colCode,
      colOrderIndex,
      colIsActive,
      colParentId,
      colDelete
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(true, true, '100%', true))

  }


  initialize(params?: any): any {
  }
  

  add() {

    var list: FormArray = <FormArray>this.form;
    let newCommand = new CreateCommodityCategoryPropertyItemCommand()
    let newForm = this.createForm(newCommand, true);
    list.insert(0,newForm);
    
    this.child.tableRows = list;
 
  }
  selectParentId(sorce: any, frng: any) {

  
    frng.controls.parentId.setValue(sorce.value);
    
  }
  

  delete(param?: any): any {
    this.form.controls.filter((x:FormGroup) => x.controls['selected'].value === true).forEach((fg:FormGroup) => {
      this.form.controls.removeAt(this.form.controls.indexOf(fg))
    })
    this.form.controls = [...this.form.controls];

  }
  onDelete(item: any) {

    
    this.form.removeAt(this.form.controls.indexOf(item));
    this.child.tableRows = this.form.controls;

  }
  update(param?: any): any {
  }
  handleRowChanges(item: FormGroup) {

  }

  close(): any {
  }

  get(param?: any): any {
  }

}
