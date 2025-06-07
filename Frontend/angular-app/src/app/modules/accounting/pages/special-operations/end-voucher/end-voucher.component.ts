import {Component} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {EndVoucherHeadCommand} from "../../../repositories/voucher-head/commands/end-voucher-head-command";
import {CloseVoucherHeadCommand} from "../../../repositories/voucher-head/commands/close-voucher-head-command";
import {
  VoucherHeadsAdjustmentCommand
} from "../../../repositories/voucher-head/commands/voucher-heads-adjustment-command";
import { tr } from 'date-fns/locale';

@Component({
  selector: 'app-end-voucher',
  templateUrl: './end-voucher.component.html',
  styleUrls: ['./end-voucher.component.scss']
})
export class EndVoucherComponent extends BaseComponent{

  isTempDoc !: boolean;
  isEndDoc!:boolean;

  voucherDate!:Date;
  constructor(private  mediator:Mediator) {
    super();
  }

  ngOnInit(): void {
  }

  async endVoucherHead(isEndDoc:boolean) {

   this.isLoading = true;
try{

  var me = this;
  var request = new EndVoucherHeadCommand();
  request.replaceEndVoucherFlag = isEndDoc;
  let response = await this.mediator.send(request);
  this.isLoading = false;

}catch{
  this.isLoading = false;

}
  }
  async voucherHeadsAdjustment() {

    await this.mediator.send(new VoucherHeadsAdjustmentCommand());
  }
  async closeVoucherHeads(isTempDoc:boolean) {

    this.isLoading = true;
try{

  
  var me = this;
  var request = new CloseVoucherHeadCommand();
  request.ReplaceCloseVoucherFlag = isTempDoc;
  let response = await this.mediator.send(request);
  this.isLoading = false;

}
catch{
  this.isLoading = false;

}
  }

  async add() {
  }

  close(): any {
  }

  delete(): any {
  }

  get(id: number): any {
  }

  initialize(): any {
  }

  resolve(): any {
  }

  update(): any {
  }

}
