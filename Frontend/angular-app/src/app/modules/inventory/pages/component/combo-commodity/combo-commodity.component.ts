
import { ElementRef, EventEmitter, OnChanges, Output, ViewChild } from '@angular/core';
import { Component, Input } from '@angular/core';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { GetReceiptsCommoditesQuery } from '../../../repositories/commodity/get-receipt-commodites-query';
import { GetReceiptsCommodityQuery } from '../../../repositories/commodity/get-receipt-commodity-query';
import { FormControl } from '@angular/forms';




@Component({
  selector: 'app-combo-commodity',
  templateUrl: './combo-commodity.component.html',
  styleUrls: ['./combo-commodity.component.scss']
})
export class ComboCommodityComponent implements OnChanges {

  @ViewChild('input')
  input!: ElementRef<HTMLInputElement>;
  title = new FormControl('');

  public nodes: any[] = [];
  @Input() isDisable: boolean = false;
  @Input() DefaultId: any = undefined;
  @Input() lablelTitleCombo: string = "";
  @Input() isRequired: boolean = false;
  @Input() warehouseId: any = undefined;
  @Input() isOnlyFilterByWarehouse: any = false;
  @Input() tabindex: number = 1;

  @Output() SelectId = new EventEmitter<any>();

 
  

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
  constructor(private _mediator: Mediator,
    private Service: PagesCommonService,
    public _notificationService: NotificationService,
  ) {

  }
  async ngOnChanges() {
    
    //-----------------------نمایش مقدار اولیه----------------------
    if ((this.title.value == undefined || this.title.value == '')  && this.DefaultId != undefined) {
      var item = this.nodes.find(a => a.id == Number(this.DefaultId));
      if (item == undefined) {
        await this.CommodityFilter('', this.DefaultId).then(res => {
          item = this.nodes.find(a => a.id == Number(this.DefaultId));

          var value = item?.title;
          this.title.setValue(value);

        });
      }
      
      var value = item?.title;
      this.title.setValue(value);
      
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
  async initialize() {

    if (this.DefaultId != undefined) {
      this.CommodityFilter('', this.DefaultId).then(a => {
        if (this.isDisable)
          this.title.disable();
      });
    }

  }


  onSearchTerm() {
    const filterValue = this.input.nativeElement.value.toLowerCase();
    if (filterValue.length / 3 >= 0) {

      this.CommodityFilter(filterValue, this.DefaultId)
    }


  }

  async CommodityFilter(searchTerm: string, id: any) {
    var filter: SearchQuery[] = [];
    
    if (searchTerm != '') {

      var propertyNames = ['SearchTerm']

      filter = this.Service.filterWord(filter, searchTerm, propertyNames);
    }
    //--------------در هنگام ویرایش فرم اطلاعات کاربری که قبلا انتخاب شده آورده شود
    if (id != undefined) {
      filter = [

        new SearchQuery({
          propertyName: 'id',
          comparison: 'equal',
          values: [id],
          nextOperand: 'or'
        }),
      ]
    }
    //--------------در هنگام ویرایش فرم اطلاعات کاربری که قبلا انتخاب شده آورده شود
    if (id != undefined) {

      await this._mediator.send(new GetReceiptsCommodityQuery(id, 0, 50, filter)).then(res => {
        this.nodes = res.data;
      });
    }

    else if (filter.length > 0 && id == undefined) {

      await this._mediator.send(new GetReceiptsCommoditesQuery(this.isOnlyFilterByWarehouse, this.warehouseId , searchTerm, 0, 100, filter)).then(res => {
        this.nodes = res.data
      });
    }
    
  }

  onSelectionChange(event: any) {

    
    this.onSelectNode(event.option.value);
  }
}



