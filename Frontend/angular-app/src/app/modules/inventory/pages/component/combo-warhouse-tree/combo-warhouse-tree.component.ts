import { style } from '@angular/animations';
import { FlatTreeControl } from '@angular/cdk/tree';
import { ElementRef, EventEmitter, OnChanges, Output, ViewChild } from '@angular/core';
import { Component, Input } from '@angular/core';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { GetWarehousesQuery } from '../../../repositories/warehouse/queries/get-warehouses-query';
import { GetWarehousesLastLevelQuery } from '../../../repositories/warehouse/queries/get-warehouses-recursives-query';
import { GetWarehousesLastLevelByCodeVoucherGroupIdQuery } from '../../../repositories/warehouse/queries/get-warehousesLastLevel-by-codeVoucherGroupId-query ';
import { MatMenuTrigger } from '@angular/material/menu';
import { AsyncPipe } from '@angular/common';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormControl } from '@angular/forms';
interface FlatNode {
  expandable: boolean;
  name: string;
  level: number;
  id: number;
  parentId: number
}

export class source {
  public id!: any;
  public parentId!: number;
  public children: source[] = [];
  public title!: string;
  public hasChild: boolean = false;
  public expandable: boolean = false;
}

@Component({
  selector: 'app-combo-warhouse-tree',
  templateUrl: './combo-warhouse-tree.component.html',
  styleUrls: ['./combo-warhouse-tree.component.scss'],

})
export class ComboWarhouseTreeComponent implements OnChanges {

  @ViewChild('input')
  input!: ElementRef<HTMLInputElement>;
  title = new FormControl('');

  public nestedNodes: source[] = [];
  public nestedNodes_Filter: source[] = [];
  public nodes: any[] = [];
  @Input() isDisable: boolean = false;
  @Input() DefaultId: any = undefined
  @Input() isRequired: boolean = false;
  @Output() SelectId = new EventEmitter<any>();
  @Input() lablelTitleCombo: string = "";
  @Input() isLastLevel: boolean = false;
  @Input() isFilterByCodeVoucher: boolean = false;
  @Input() codeVoucherGroupId!: number;
  @Input() tabindex!: number;
  @Input( )warehouseCountShow:boolean = false;
  parentNames: string[] = []
  nodes_filter: any[] = [];

  constructor(
    private _mediator: Mediator,
    private Service: PagesCommonService,
    public _notificationService: NotificationService,

  ) {

  }

  async initialize() {

    var filter: SearchQuery[] = [
      new SearchQuery({
        propertyName: 'IsActive',
        comparison: 'equal',
        values: [true]
      })

    ];

    if (this.isFilterByCodeVoucher && this.codeVoucherGroupId != undefined) {
      await this._mediator.send(new GetWarehousesLastLevelByCodeVoucherGroupIdQuery(this.codeVoucherGroupId, 0, 0, filter)).then(async (res) => {
        this.nodes = res.data
        this.nodes_filter = res.data;


      })

    }
    else if (this.isLastLevel) {
      await this._mediator.send(new GetWarehousesLastLevelQuery(0, 0, filter)).then(async (res) => {

        this.nodes = res.data
        this.nodes_filter = res.data;


      })

    }
    else if (!this.isFilterByCodeVoucher && !this.isLastLevel) {
      await this._mediator.send(new GetWarehousesQuery(0, 0, filter)).then(async (res) => {
        if (this.warehouseCountShow)
                this.nodes = res.data.filter(x=>x.countable);
        else
          this.nodes=res.data;

      })

    }

    if (this.nodes.length == 1) {

      this.onSelectNode(this.nodes[0].id)
    }

  }
  async ngOnChanges() {


    //--------get data flat and convert to tree by component
    await this.initialize();


    //-----------------------نمایش مقدار اولیه----------------------
    if (this.nodes.length > 0 && this.DefaultId != undefined) {

      var item = this.nodes.find(a => a.id == Number(this.DefaultId));

      var value = item?.title;
      this.title.setValue(value);
      if (this.isDisable)
        this.title.disable()

    }
    if (this.DefaultId == undefined) {
      this.title.setValue(undefined);
    }

  }

  async ngOnInit() {

    await this.resolve();
  }

  async resolve() {

    await this.initialize()
  }



  onSelectNode(id: any) {

    var item = this.nodes.find(a => a.id == id);
    this.SelectId.emit(item);
    this.title.setValue(item?.title);

    this.DefaultId = id;


    if (this.title.value == undefined) {
      this.title.setValue(undefined);
      this.DefaultId = undefined
    }

  }

  onSearchTerm() {
    const filterValue = this.input.nativeElement.value.toLowerCase();
    this.nodes = this.nodes_filter.filter(o => o.title.toLowerCase().includes(filterValue));

  }

  onSelectionChange(event: any) {

    this.onSelectNode(event.option.value);
  }

}




