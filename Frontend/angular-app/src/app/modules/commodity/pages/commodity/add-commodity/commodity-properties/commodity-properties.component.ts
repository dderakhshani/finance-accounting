import { Component, EventEmitter, Inject, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { BaseComponent } from "../../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../../core/services/mediator/mediator.service";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../../shared/services/notification/notification.service';
import { GetBaseValuesByUniqueNameQuery } from '../../../../../admin/repositories/base-value/queries/get-base-values-by-unique-name-query';
import { BaseValue } from '../../../../../admin/entities/base-value';
import { AddCommodityPropertyValueCommand } from '../../../../repositories/commodity-property-value/commands/add-commodity-property-value-command';

@Component({
  selector: 'app-commodity-properties',
  templateUrl: './commodity-properties.component.html',
  styleUrls: ['./commodity-properties.component.scss']
})
export class CommodityPropertiesComponent implements OnChanges {

  @Input() nodes_filter: any[] = [];
  @Input() nodes: any[] = [];

  @Input() property: AddCommodityPropertyValueCommand = new AddCommodityPropertyValueCommand();
  @Input() isDisable: boolean = false;
  @Input() DefaultId: any = undefined
  @Input() isRequired: boolean = false;

  @Output() SelectedItem = new EventEmitter<any>();

  title: string | undefined = "";;
  searchTerm: string = "";
  propertyValueTypes: BaseValue[] = []
  itemType: string |undefined= "";
  constructor(private mediator: Mediator,
    private Service: PagesCommonService,
    public _notificationService: NotificationService,
  ) {}

  
  async ngOnChanges() {

    //-----------------------نمایش مقدار اولیه----------------------
    if (this.title == "" && this.DefaultId != undefined) {
      var item = this.nodes.find(a => a.id == Number(this.DefaultId));
      var value = item?.title;
      this.title = value;
    }
    
    this.title = this.DefaultId == undefined ? '' : this.title;
    this.searchTerm = "";

  }

  ngOnInit(): void {

    
    this.mediator.send(new GetBaseValuesByUniqueNameQuery('TypeCommodityCategoryProperties')).then(responce => {
      this.propertyValueTypes = responce;
      this.itemType = this.propertyValueTypes.find(a => a.id == this.property.propertyTypeBaseId)?.uniqueName;

    })
    this.DefaultId = this.property.categoryPropertyId
    this.title = this.property.value;
    

  }
  onSelectNode(item: any) {
    
    this.property.value = item.title;
    this.property.valuePropertyItemId = Number(item.id);
    

    this.SelectedItem.emit(this.property);
    this.title = item?.title;
    this.DefaultId = item != undefined ? item.id : undefined;
    this.title = this.title == undefined ? '' : this.title
    this.searchTerm = "";
    this.onfocus();

  }
  onSelected() {

    this.property.value = this.title;
    this.SelectedItem.emit(this.property);
   
  }

  onfocus() {

    setTimeout(() => {
      document.getElementById('searchTerm')?.focus();
    }, 1);

  }
  onSearchTerm() {


   
      if (this.searchTerm) {

        this.nodes = [...this.nodes_filter.filter(x => x.title.toLowerCase().includes(this.searchTerm.toLowerCase()))]
      } else {
        this.nodes = [...this.nodes_filter]
      }
   
  }
}
