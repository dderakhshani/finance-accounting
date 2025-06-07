
import {Component, Input} from '@angular/core';
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {BaseComponent} from "../../../../core/abstraction/base.component";
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import {AddWarehouseLayoutDialogComponent} from "./add-warehouse-layout-dialog/add-warehouse-layout-dialog.component";
import {PageModes} from "../../../../core/enums/page-modes";
import {WarehouseLayout, WarehouseLayoutTree} from "../../entities/warehouse-layout";
import { UpdateWarehouseLayoutDialogComponent } from './update-warehouse-layout-dialog/update-warehouse-layout-dialog.component';
import { Warehouse } from '../../entities/warehouse';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GetWarehouseLayoutQuery } from '../../repositories/warehouse-layout/queries/get-warehouse-layout-query';
import { GetWarehouseLayoutTreesQuery } from '../../repositories/warehouse-layout/queries/get-warehouse-layouts-tree-query';
import {SearchQuery} from "../../../../shared/services/search/models/search-query";
import { GetParentIdAllChildByCapacityAvailabeQuery } from '../../repositories/warehouse-layout/queries/get-warehouse-layouts-parentId-all-child-query';
import { NotificationService } from '../../../../shared/services/notification/notification.service';
import { PagesCommonService } from '../../../../shared/services/pages/pages-common.service';
import {  WarehouseLayoutCountFormIssuanceDialogComponent} from "./warehouse-layout-count-form-issuance-dialog/warehouse-layout-count-form-issuance-dialog.component";

@Component({
  selector: 'app-warehouse-layout',
  templateUrl: './warehouse-layout.component.html',
  styleUrls: ['./warehouse-layout.component.scss']
})
export class WarehouseLayoutComponent extends BaseComponent {

  WarehouseLayouts: WarehouseLayout[] = [];
  WarehouseLayouts_Filter: WarehouseLayout[] = [];
  WarehouseLayoutTree:WarehouseLayoutTree[] = [];
  @Input( )warehouseCountShow:boolean = false;
  viewType:string="grid"

  showFiller = false;
  ParentId:number |undefined=undefined;
  isToggled:boolean=false;
  searchTerm:string="";
warehouseId!:number;
  lablelTitleCombo: string="انبار مورد نظر را انتخاب کنید";

  ownerList=new WarehouseLayout() ;


  constructor(
    private _mediator: Mediator,
    public dialog: MatDialog,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,
     ) {
    super();
    this.form = new FormControl('', [Validators.required,])

    }

  async ngOnInit() {
    await this.resolve();
    let urlParams =window.location.pathname;
    //this.warehouseCountShow = urlParams.includes('warehouseCountFormIssuance');

    }

  async resolve() {
    await this.initialize()
  }
  //--------------جستجو چیدمان انبار--------------------------------------
  async getRequestDetails() {
    await this.get()
  }
  async get() {
    let searchQuery = [
      new SearchQuery({
        propertyName: 'warehouseId',
        comparison: 'equal',
        values: [this.form?.value],
      })
    ]

    this.WarehouseLayoutTree=[];
    this.WarehouseLayouts=[];
    await this._mediator.send(new GetWarehouseLayoutTreesQuery(0, 0, searchQuery, "id ASC")).then(res => {
      this.WarehouseLayoutTree = res.data;
      this.warehouseId=res.data[0].warehouseId;
    })

  }
  //--------------------نمایش جزئیات اطلاعات انبار---------------------------
  async onShowDetails(id:number){
    if(id!=undefined)
    {
      this.ParentId=id;
      //var ownerList=this.WarehouseLayoutTree.find(a=>a.id==id);

      await this._mediator.send(new GetParentIdAllChildByCapacityAvailabeQuery(id)).then(res => {

        this.WarehouseLayouts=[];


        this.WarehouseLayouts = res.data;
        this.WarehouseLayouts_Filter=res.data;
      })
    }


  }
  WarehouseIdSelect(item: Warehouse) {


    this.form.setValue(item?.id);
    this.get();
  }

  //-----------------------------افزودن جدید-------------------------------
  addMainParent()
  {
    this.ParentId=0;
    this.add();
  }

  add() {
   let warehouseLayout=new WarehouseLayout() ;
   warehouseLayout.parentId=this.ParentId;

    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {

      WarehouseLayout: warehouseLayout,
      warehouseId: this.form?.value,
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(AddWarehouseLayoutDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {

      if (response && pageMode === PageModes.Add) {
        this.WarehouseLayouts=[];
        this.WarehouseLayouts=response?.data

        this.onShowDetails(Number(this.WarehouseLayouts[0]?.parentId))
        this.get();

      }
    })

  }


  //----------------------------------ویرایش------------------------------
  onClickDetaislCard(WarehouseLayout: WarehouseLayout)
  {

    if(WarehouseLayout.lastLevel==false)
    {
      this.update(WarehouseLayout);
    }
    else{
      this.onShowDetails(Number(WarehouseLayout.id))
    }
  }

 async update(WarehouseLayout: any) {
   let dialogConfig = new MatDialogConfig();

   var request = await this._mediator.send(new GetWarehouseLayoutQuery(WarehouseLayout.id))

    dialogConfig.data = {
      WarehouseLayout: request,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(UpdateWarehouseLayoutDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {

      if (response) {

        if (pageMode === PageModes.Update) {

          var WarehouseLayoutToUpdate = this.WarehouseLayouts.find(x => x.id === request.id)
          var id = this.WarehouseLayouts.find(x => x.id == request.id)?.parentId;
          if (id != undefined) {
            this.onShowDetails(Number(id))
          }

          if (WarehouseLayoutToUpdate) {
            Object.keys(response).forEach((key: string) => {
              // @ts-ignore
              WarehouseLayoutToUpdate[key] = response[key]
            })
          }

        }

        this.get();
      }

      if (pageMode === PageModes.Delete) {
        this.get();

        if(this.WarehouseLayouts[0]?.parentId!=undefined)
        {
          this.onShowDetails(Number(this.WarehouseLayouts[0]?.parentId))
        }
      }
    })
  }

  async warehouseCount(WarehouseLayout: any){
    let dialogConfig = new MatDialogConfig();
     dialogConfig.data = {
      warehouseLayoutId: WarehouseLayout.id,
      pageMode: PageModes.Update,
      parentId:null,
       warehouseId:this.warehouseId
    };
    let dialogReference = this.dialog.open(WarehouseLayoutCountFormIssuanceDialogComponent, dialogConfig);
  }
  onSearchTerm() {

      if (this.searchTerm) {
        this.WarehouseLayouts  = [...this.WarehouseLayouts_Filter.filter(x => x.title.toLowerCase().includes(this.searchTerm.toLowerCase()))]
      } else {
        this.WarehouseLayouts = [...this.WarehouseLayouts_Filter]
      }
    }

  delete() {

  }

  async initialize() {

  }


  close() {
  }
}
