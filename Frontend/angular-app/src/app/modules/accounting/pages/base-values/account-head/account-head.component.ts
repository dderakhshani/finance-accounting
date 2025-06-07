import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {AccountHead} from "../../../entities/account-head";
import {Component, OnInit} from "@angular/core";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {FormAction} from "../../../../../core/models/form-action";
import {AccountManagerService} from "../../../services/account-manager.service";
import {AccountHeadModalComponent} from "./account-head-modal/account-head-modal.component";
import {PageModes} from "../../../../../core/enums/page-modes";
import {IdentityService} from "src/app/modules/identity/repositories/identity.service";
import {Action, PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {GetSSRSReportURLQuery} from "../../../repositories/reporting/queries/get-ssrs-report-url-query";
import {SSRSReportDialogComponent} from "../../../components/ssrsreport-dialog/ssrsreport-dialog.component";

@Component({
  selector: 'app-account-head',
  templateUrl: './account-head.component.html',
  styleUrls: ['./account-head.component.scss']
})
export class AccountHeadComponent implements OnInit {
  public readonly PageModes = PageModes;
  entities: AccountHead[] = [];
  formActions: Action[] = [
    PreDefinedActions.print()
  ];

  selectedAccountHead!: AccountHead;
  showAccountHeadModalInReadMode: boolean = false;

  constructor(
    private accountManagerService: AccountManagerService,
    private mediator: Mediator,
    public dialog: MatDialog, public identityService: IdentityService
  ) {


  }

  async ngOnInit() {
    this.accountManagerService.accountHeads.subscribe((val) => {
      // @ts-ignore
      let toggledAccountHeads = this.entities.filter(x => x._expanded)

      this.entities = JSON.parse(JSON.stringify(val))
      if (this.selectedAccountHead) this.selectedAccountHead = <AccountHead>this.entities.find(x => x.id === this.selectedAccountHead.id)

      if (toggledAccountHeads.length > 0) {
        toggledAccountHeads.forEach((toggleAccountHead) => {
          let accountHeadToToggle = this.entities.find(x => x.id === toggleAccountHead.id)
          // @ts-ignore
          accountHeadToToggle._expanded = true
        })
      }
    })
  }


  add(parentAccountHead: AccountHead) {
    let dialogConfig = new MatDialogConfig()
    dialogConfig.data = {
      parentId: parentAccountHead?.id,
      parentCode: parentAccountHead?.code,
      parentTitle: parentAccountHead?.title,
    }
    let dialogRef = this.dialog.open(AccountHeadModalComponent, dialogConfig)
    dialogRef.afterClosed().subscribe((accountHead: AccountHead) => {
      if (accountHead?.id) this.accountManagerService.updateAccountHeads()
    })
  }

  update(accountHead: AccountHead) {
    let dialogConfig = new MatDialogConfig()
    dialogConfig.data = accountHead
    let dialogRef = this.dialog.open(AccountHeadModalComponent, dialogConfig)
    dialogRef.afterClosed().subscribe((accountHead: AccountHead) => {
      if (accountHead?.id) this.accountManagerService.updateAccountHeads()
    })
  }

  showAccountHeadInReadMode(accountHead: AccountHead) {
    this.selectedAccountHead = accountHead;
    this.showAccountHeadModalInReadMode = true
  }

  async handlePrint() {
    await this.mediator.send(new GetSSRSReportURLQuery('acc_accounthead')).then(res => {
      let dialogConfig = new MatDialogConfig();
      dialogConfig.data = {
        sourceURL: 'Acc_AccountHead'
      }
      this.dialog.open(SSRSReportDialogComponent, dialogConfig)
    })
  }
}
