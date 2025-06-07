
import { ElementRef, EventEmitter, OnChanges, Output, ViewChild } from '@angular/core';
import { Component, Input } from '@angular/core';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { GetAccountReferenceProviderQuery } from '../../../repositories/personal/get-accounting-refrences-provider-query';
import { GetAccountReferenceInventoryQuery} from '../../../repositories/personal/get-accounting-refrences-query';
import { GetAccountReferencesPersonQuery } from '../../../repositories/personal/get-persons-query';
import { MatMenuTrigger } from '@angular/material/menu';
import { FormControl } from '@angular/forms';


@Component({
  selector: 'app-combo-account-refrence',
  templateUrl: './combo-account-refrence.component.html',
  styleUrls: ['./combo-account-refrence.component.scss']
})
export class ComboAccountRefrenceComponent implements OnChanges {

  @ViewChild('input')
  input!: ElementRef<HTMLInputElement>;
  title = new FormControl('');
  public nodes: any[] = [];
  @Input() isDisable: boolean = false;
  @Input() accountHeadId: number | undefined = undefined;
  
  @Input() DefaultId: any = undefined
  @Input() lablelTitleCombo: string = "";
  @Input() isRequired: boolean = false;
  @Input() accountReferencesGroupsCode: string = ''
  @Input() accountReferencesGroupsId: string = ''
  @Output() SelectId = new EventEmitter<any>();
  @Input() tabindex: number = 1;

  

  onSelectNode(item: any) {

    this.SelectId.emit(item);
    
    this.title.setValue(item?.title);
    this.DefaultId = item != undefined ? item.id : undefined;
    this.accountReferencesGroupsId = item?.accountReferenceGroupId;
    


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
    if (this.title.value == undefined && this.DefaultId != undefined) {

      await this.ReferenceFilter('', this.DefaultId).then(a => {
        var value = this.nodes.find(a => a.id == Number(this.DefaultId))?.title;

        this.title.setValue(value);
        
      })

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
      this.ReferenceFilter('', this.DefaultId).then(a => {
        if (this.isDisable)
          this.title.disable();
      });
     
    }
  }

  async onSearchTerm() {
    const filterValue = this.input.nativeElement.value.toLowerCase();
    this.ReferenceFilter(filterValue, this.DefaultId)

  }
  async ReferenceFilter(searchTerm: string, id: any) {
    var filter: SearchQuery[] = [];

    if (searchTerm != '') {

      var propertyNames = ['searchTerm']

      filter = this.Service.filterWord(filter, searchTerm, propertyNames);
    }

    //--------------در هنگام ویرایش فرم اطلاعات کاربری که قبلا انتخاب شده آورده شود
   
    if (id != undefined && id>0) {
      filter = [

        new SearchQuery({
          propertyName: 'id',
          comparison: 'equal',
          values: [id],
          nextOperand: 'and'
        }),
      ]
     
      if (this.accountReferencesGroupsId != ''  && this.accountReferencesGroupsId != undefined) {
        filter.push(new SearchQuery({
          propertyName: 'AccountReferenceGroupId',
          comparison: 'equal',
          values: [this.accountReferencesGroupsId],
          nextOperand: 'and'
        }))
      }
    }
    //filter.push(new SearchQuery({
    //  propertyName: 'accountHeadId',
    //  comparison: 'equal',
    //  values: [this.accountHeadId],
    //  nextOperand: 'and'
    //}))

    if (filter.length > 0) {

      this._notificationService.isLoaderDropdown = true;
      if (this.accountReferencesGroupsCode == this.Service.ProviderCodeGroup) {
        //----------------لیست تامین کنندگان
        await this._mediator.send(new GetAccountReferenceProviderQuery(0, 25, filter)).then(res => {

          this.nodes = res.data
        })
      }
      else if (this.accountReferencesGroupsCode == this.Service.PersonalCodeGroup) {
        //--------------------لیست پرسنل ایفا
        await this._mediator.send(new GetAccountReferencesPersonQuery(0, 25, filter)).then(res => {

          this.nodes = res.data
        })
      }
      else {
        await this._mediator.send(new GetAccountReferenceInventoryQuery(this.accountHeadId,0, 25, filter)).then(res => {

          this.nodes = res.data
        })
      }
    }
    if (this.nodes.length == 1) {

      this.onSelectNode(this.nodes[0])
    }
  }
  onSelectionChange(event: any) {

    var item = this.nodes.find(a => a.id == event.option.value);
    this.onSelectNode(item);
  }
}



