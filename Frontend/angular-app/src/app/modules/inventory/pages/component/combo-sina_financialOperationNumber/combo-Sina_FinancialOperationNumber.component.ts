
import { ElementRef, EventEmitter, OnChanges, Output, ViewChild } from '@angular/core';
import { Component, Input } from '@angular/core';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { GetPrehensionQuery } from '../../../../logistics/repositories/prehension/queries/get-all-prehension-query';
import { MatMenuTrigger } from '@angular/material/menu';
import { GeView_Sina_FinancialOperationNumberQuery } from '../../../repositories/receipt/queries/receipt/get-view_sina_financialOperationNumber';
import { FormControl } from '@angular/forms';


@Component({
  selector: 'app-combo-Sina_FinancialOperationNumber',
  templateUrl: './combo-Sina_FinancialOperationNumber.component.html',
  styleUrls: ['./combo-Sina_FinancialOperationNumber.component.scss']
})
export class ComboSina_FinancialOperationNumberComponent implements OnChanges {

  @ViewChild('input')
  input!: ElementRef<HTMLInputElement>;
  title = new FormControl('');

  public nodes: any[] = [];
  @Input() isDisable: boolean = false;
  @Input() DefaultId: any = undefined
  @Input() lablelTitleCombo: string = "";
  @Input() isRequired: boolean = false;
  @Input() tabindex: number = 1;
  @Output() SelectId = new EventEmitter<any>();

  searchTerm: string = "";

  
  constructor(private _mediator: Mediator,
    private Service: PagesCommonService,
    public _notificationService: NotificationService,
  ) {

  }

  async initialize() {

  }
  async ngOnChanges() {


    //--------get data flat and convert to tree by component
    await this.initialize();


    //-----------------------نمایش مقدار اولیه----------------------
    if (this.DefaultId != undefined) {

    

      
      this.title.setValue(this.DefaultId);
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



  onSelectNode(item: any) {

  
    this.SelectId.emit(item);
    this.title.setValue(item?.paymentNumber);

    this.DefaultId = item?.paymentNumber;


    if (this.title.value == undefined) {
      this.title.setValue(undefined);
      this.DefaultId = undefined
    }

  }

  async onSearchTerm() {
    const filterValue = this.input.nativeElement.value.toLowerCase();
    this.ReferenceFilter(filterValue, this.DefaultId)

  }
  async ReferenceFilter(searchTerm: string, paymentNumber: any) {

    var filter: SearchQuery[] = [];
    //--------------در هنگام ویرایش فرم اطلاعات کاربری که قبلا انتخاب شده آورده شود

    if (paymentNumber != undefined) {
      filter = [

        new SearchQuery({
          propertyName: 'paymentNumber',
          comparison: 'equal',
          values: [paymentNumber],
          nextOperand: 'and'
        }),

      ]

    }
    else {

      filter = [

        new SearchQuery({
          propertyName: 'paymentNumber',
          comparison: 'contains',
          values: [searchTerm],
          nextOperand: 'and'
        }),

      ]
    }

    if (filter.length > 0) {

      await this._mediator.send(new GeView_Sina_FinancialOperationNumberQuery(0, 50, filter, '')).then(res => {

        this.nodes = res.data;
        this.nodes.forEach(a => a.title = a.productTitle);
      })
    }
    if (this.nodes.length == 1) {

      this.onSelectNode(this.nodes[0])
    }
  }

  onSelectionChange(event: any) {

    this.onSelectNode(event.option.value);
  }
}



