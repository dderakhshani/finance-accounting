import {Component, Inject} from '@angular/core';
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {User} from "../../../../admin/entities/user";
import {GetUsersQuery} from "../../../../admin/repositories/user/queries/get-users-query";
import {WarehouseCountFormHead} from "../../../entities/warehouse-count-form-head";
import {CreateWarehouseCountFormCommand} from "../../../repositories/warehouse/commands/create-warehouse-count-Form-command";
import {PageModes} from "../../../../../core/enums/page-modes";
import {ActivatedRoute, Router} from "@angular/router";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {PagesCommonService} from "../../../../../shared/services/pages/pages-common.service";

@Component({
  selector: 'app-warehouse-layout-count-form-issuance-dialog',
  templateUrl: './warehouse-layout-count-form-issuance-dialog.component.html',
  styleUrls: ['./warehouse-layout-count-form-issuance-dialog.component.scss']
})
export class WarehouseLayoutCountFormIssuanceDialogComponent extends  BaseComponent {
  entity!:WarehouseCountFormHead;
  confirmerUsers: User[]=[];
  counterUsers:User[]=[];
  warehouseLayoutId:number;
  parentId!:number;
  formStatus!:number;
  selectedCommodity:any[]=[];
  basedOnLocation:boolean=true;
  warehouseId:number;
  constructor(private _mediator: Mediator,
              private dialogRef: MatDialogRef<WarehouseLayoutCountFormIssuanceDialogComponent>,
              @Inject(MAT_DIALOG_DATA) data: any,
              private route: ActivatedRoute,
              private router: Router,
              public Service: PagesCommonService,
  ) {
    super(route,router);
    this.request =new CreateWarehouseCountFormCommand();
    this.selectedCommodity=data.selectedCommodity
    this.warehouseLayoutId=data.warehouseLayoutId;
    this.pageMode = PageModes.Add;
    this.parentId=data.parentId;
    this.formStatus=data.formStatus;
    this.basedOnLocation=data.basedOnLocation;
    this.warehouseId=data.warehouseId;
  }
  ngOnInit(params?: any): void {
   this.resolve();
  }
  resolve(params?: any) {
    this.getConfirmerUsers();
    this.getCounterUsers();
  }
  initialize(params?: any) {

  }
  add(param?: any) {
   let req=<CreateWarehouseCountFormCommand>this.request;
   this.isLoading = true;
    req.warehouseLayoutId=this.warehouseLayoutId;
    req.parentId=this.parentId;
    req.basedOnLocation=this.basedOnLocation;
    req.warehouseId=this.warehouseId;
    if (this.selectedCommodity!=undefined && this.selectedCommodity.length>0 && this.formStatus==0)
      req.countFormSelectedCommodity=this.selectedCommodity;

     this._mediator.send(<CreateWarehouseCountFormCommand>this.request).then(res => {
       this.isLoading = false;
      this.dialogRef.close(res)
      return this.router.navigateByUrl(`/inventory/WarehouseCountFormList`);

     }).catch(err => {
       this.isLoading = false;
     })
  }
  get(param?: any) {
    throw new Error('Method not implemented.');
  }
  update(param?: any) {
    throw new Error('Method not implemented.');
  }
  delete(param?: any) {
    throw new Error('Method not implemented.');
  }
  close() {
    throw new Error('Method not implemented.');
  }
   getConfirmerUsers(value?:any){
    let conditions : SearchQuery[] = []
    if(value){
      conditions.push(new SearchQuery({
        propertyName: 'fullName',
        values:  [value],
        comparison: 'contain',
        nextOperand: 'and'
      }))
    }
     this._mediator.send(new GetUsersQuery(0,0,conditions)).then(res => {
     this.confirmerUsers=res.data;
    })
  }
   getCounterUsers(value?:any){
    let conditions : SearchQuery[] = []
    if(value){
      conditions.push(new SearchQuery({
        propertyName: 'fullName',
        values:  [value],
        comparison: 'contain',
        nextOperand: 'and'
      }))
    }
     this._mediator.send(new GetUsersQuery(0,0,conditions)).then(res => {
      this.counterUsers=res.data;
    })
  }
  confirmUserDisplayFn(userId: number) {
    return  this.confirmerUsers.find(x => x.id === userId) ?.fullName?? '';
  }
  counterUserDisplayFn(userId: number) {
    return  this.counterUsers.find(x => x.id === userId) ?.fullName?? '';
  }
}
