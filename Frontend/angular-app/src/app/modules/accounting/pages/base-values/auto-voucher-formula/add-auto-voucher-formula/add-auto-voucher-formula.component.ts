import {Component, OnInit, ViewChild} from '@angular/core';
import { Router } from "@angular/router";
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { FormControl } from "@angular/forms";
import { AccountManagerService } from "../../../../services/account-manager.service";
import { AccountHead } from "../../../../entities/account-head";
import { CodeVoucherGroup } from 'src/app/modules/accounting/entities/code-voucher-group';
import { AutoVoucherFormula } from 'src/app/modules/accounting/entities/AutoVoucherFormula';
import { PageModes } from 'src/app/core/enums/page-modes';
import { CreateAutoVoucherFormulaCommand } from 'src/app/modules/accounting/repositories/auto-voucher-formula/commands/create-auto-voucher-formula-command';
import { UpdateAutoVoucherFormulaCommand } from 'src/app/modules/accounting/repositories/auto-voucher-formula/commands/update-auto-voucher-formula-command';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { DeleteAutoVoucherFormulaCommand } from 'src/app/modules/accounting/repositories/auto-voucher-formula/commands/delete-auto-voucher-formula-command';
import { Action, ActionTypes, PreDefinedActions } from 'src/app/core/components/custom/action-bar/action-bar.component';
import {CreateFormulaComponent} from "./create-formula/create-formula.component";
import {CreateConditionComponent} from "./create-condition/create-condition.component";
import {GetAutoVoucherFormulaQuery} from "../../../../repositories/auto-voucher-formula/queries/get-auto-voucher-formula-query";
import {JsonEditorOptions} from "@maaxgr/ang-jsoneditor";


@Component({
  selector: 'app-add-auto-voucher-formula',
  templateUrl: './add-auto-voucher-formula.component.html',
  styleUrls: ['./add-auto-voucher-formula.component.scss']
})

export class AddAutoVoucherFormulaComponent extends BaseComponent {

  jsonFormulaFormControl = new FormControl('');
  jsonConditionFormControl=new FormControl('');
  entity!: AutoVoucherFormula;
  accountHeads: AccountHead[] = [];
  voucherTypes: CodeVoucherGroup[] = [];
  sourceVoucherTypes: CodeVoucherGroup[] = [];
  pageModes = PageModes;
  debitCreditStatuses = new Array<{ id: number, title: string }>()
  fillDebitCreditStatuses() {
    this.debitCreditStatuses.push({ id: 1, title: "بدهکار" });
    this.debitCreditStatuses.push({ id: 2, title: "بستانکار" });
  }
  public editorOptions: JsonEditorOptions;
  public formulaJson: any;
 public visibleData:any;
  @ViewChild(CreateFormulaComponent) createFormulaComponent!: CreateFormulaComponent;
  @ViewChild(CreateConditionComponent) CreateConditionComponent!: CreateConditionComponent;
  constructor(
    private mediator: Mediator,
    private router: Router,
    public accountManagerService: AccountManagerService,

  ) {
    super();
    this.editorOptions=new JsonEditorOptions();
    this.editorOptions.modes = ['code', 'text', 'tree', 'view'];
    this.visibleData=this.formulaJson;
  }
  ngAfterViewInit() {
    this.actionBar.actions=[
      PreDefinedActions.save(),
      new Action('مرتب سازی','add',ActionTypes.custom,'beautify', 'primary')

    ]
  }
  async ngOnInit() {
    await this.resolve();
  }

  async navigateToAutoVoucherFormulaList() {
    await this.router.navigateByUrl('/accounting/autoVoucherFormula/list');
  }

  async add(param?: any) {
    // debugger
    this.createFormulaComponent.generateJson();
    this.CreateConditionComponent.generateJson();
     let response = await this.mediator.send(<CreateAutoVoucherFormulaCommand>this.request);

  }

  async get(Id: number) {
    // debugger
    return await this.mediator.send(new GetAutoVoucherFormulaQuery(Id));
  }
  async update(param?: any) {
    // debugger
    this.createFormulaComponent.generateJson();
    this.CreateConditionComponent.generateJson();
    let response = await this.mediator.send(<UpdateAutoVoucherFormulaCommand>this.request);
  }
  async delete(param?: any) {
    let response = await this.mediator.send(new DeleteAutoVoucherFormulaCommand((<UpdateAutoVoucherFormulaCommand>this.request).id ?? 0));
  }
  close() {
    throw new Error('Method not implemented.');
  }

  async resolve() {
    this.isLoading = true;
    this.accountManagerService.accountHeads.subscribe((value) => {
      this.accountHeads = value;
    })

    this.accountManagerService.codeVoucherGroups.subscribe((value) => {
      this.voucherTypes = value;
      this.sourceVoucherTypes = value
    })
    this.fillDebitCreditStatuses();


    this.isLoading = false;
    return await this.initialize()
  }

  async initialize(entity?: AutoVoucherFormula) {
    // debugger
    if (entity || this.getQueryParam('id')) {
      this.pageMode = PageModes.Update;
      if (!entity) entity = await this.get(this.getQueryParam('id'));
      this.request = new UpdateAutoVoucherFormulaCommand().mapFrom(entity);
      this.jsonFormulaFormControl.setValue(JSON.parse(entity.formula));
      this.jsonConditionFormControl.setValue(JSON.parse(entity.conditions));

    } else {
      this.pageMode = PageModes.Add;
      let request = new CreateAutoVoucherFormulaCommand();

   //request.formulaJson = "[{\"Property\":\"VoucherRowDescription\",\"Value\":{\"Text\":\"بابت سند شماره [1] مورخه [2] \",\"Properties\":[{\"Name\":\"DocumentNo\"},{\"Name\":\"DocumentDate\"}]}},{\"Property\":\"Credit\",\"Value\":{\"Text\":\"[1]\",\"Properties\":[{\"Name\":\"PriceMinusDiscount\"}]}}]"
    //request.conditionsJson="[{\"Expression\":\"[startf] [1] = 28410 [endf]\",\"Properties\":[{\"Name\":\"DocumentStateBaseId\"}]}]"
      this.request = request;
    }
    this.isLoading = false;
  }

  handleCustomClick(action:Action) {
    if(action.uniqueName == 'beautify') {

    }
  }
  showJson(d: Event) {
    this.visibleData = d;
  }
}

