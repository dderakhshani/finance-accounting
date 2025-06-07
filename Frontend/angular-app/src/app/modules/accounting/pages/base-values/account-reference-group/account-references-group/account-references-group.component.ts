import { Component, OnInit } from '@angular/core';
import { AccountManagerService } from "../../../../services/account-manager.service";
import { FormAction } from "../../../../../../core/models/form-action";
import { FormActionTypes } from "../../../../../../core/constants/form-action-types";
import { AccountReferencesGroup } from "../../../../entities/account-references-group";
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import {
  AccountReferencesGroupModalComponent
} from "../account-references-group-modal/account-references-group-modal.component";
import { IdentityService } from 'src/app/modules/identity/repositories/identity.service';

@Component({
  selector: 'app-account-references-group',
  templateUrl: './account-references-group.component.html',
  styleUrls: ['./account-references-group.component.scss']
})
export class AccountReferencesGroupComponent implements OnInit {

  entities: AccountReferencesGroup[] = [];
  formActions: FormAction[] = [
    FormActionTypes.add,
    FormActionTypes.refresh,
  ];

  selectedGroupId!: number;
  constructor(
    private accountManagerService: AccountManagerService,
    public dialog: MatDialog, public identityService: IdentityService) {
  }

  async ngOnInit() {
    this.accountManagerService.accountReferenceGroups.subscribe((val) => {
      this.entities = JSON.parse(JSON.stringify(val.filter(x => x.isVisible)))
    })
  }

  add(parentAccountReferencesGroup: AccountReferencesGroup) {
    let dialogConfig = new MatDialogConfig()
    dialogConfig.data = {
      parentId: parentAccountReferencesGroup?.id
    }
    let dialogRef = this.dialog.open(AccountReferencesGroupModalComponent, dialogConfig)
    dialogRef.afterClosed().subscribe((accountReferencesGroup: AccountReferencesGroup) => {
      if (accountReferencesGroup.id) this.accountManagerService.updateAccountReferencesGroups()
    })
  }

  update(accountReferencesGroup: AccountReferencesGroup) {
    let dialogConfig = new MatDialogConfig()
    dialogConfig.data = accountReferencesGroup
    let dialogRef = this.dialog.open(AccountReferencesGroupModalComponent, dialogConfig)
    dialogRef.afterClosed().subscribe((accountReferencesGroup: AccountReferencesGroup) => {
      if (accountReferencesGroup.id) this.accountManagerService.updateAccountReferencesGroups()
    })
  }

}
