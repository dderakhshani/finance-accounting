import {Component} from '@angular/core';
import {ListControl} from "../../../../../../core/components/custom/tree/list-control/list-control";
import {Node} from "../../../../../../core/components/custom/tree/list-control/models/node";
import {ActivatedRoute, Router} from "@angular/router";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {PreDefinedActions} from "../../../../../../core/components/custom/action-bar/action-bar.component";
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {forkJoin} from "rxjs";
import {AccountReferencesGroup} from "../../../../entities/account-references-group";
import {
  GetAccountReferencesGroupsQuery
} from "../../../../repositories/account-reference-group/queries/get-account-references-groups-query";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../../../admin/repositories/base-value/queries/get-base-values-by-unique-name-query";
import {BaseValue} from "../../../../../admin/entities/base-value";
import {PageModes} from "../../../../../../core/enums/page-modes";
import {
  UpdateAccountReferenceCommand
} from "../../../../repositories/account-reference/commands/update-account-reference-command";
import {
  CreateAccountReferenceCommand
} from "../../../../repositories/account-reference/commands/create-account-reference-command";
import {AccountReference} from "../../../../entities/account-reference";
import {GetAccountReferenceQuery} from "../../../../repositories/account-reference/queries/get-account-reference-query";
import {
  ListControlActionTypes
} from "../../../../../../core/components/custom/tree/list-control/models/list-control-action-types";
import {FormControl} from "@angular/forms";
import {AccountManagerService} from "../../../../services/account-manager.service";
import {AccountHead} from "../../../../entities/account-head";

@Component({
  selector: 'app-add-account-reference',
  templateUrl: './add-account-reference.component.html',
  styleUrls: ['./add-account-reference.component.scss']
})
export class AddAccountReferenceComponent extends BaseComponent {

  pageModes = PageModes;

  accountReferencesGroups: AccountReferencesGroup[] = [];
  accountHeads: AccountHead[] = [];
  accountReferenceTypes: BaseValue[] = [];

  constructor(
    private _mediator: Mediator,
    private router: Router,
    private route: ActivatedRoute,
    private accountManagerService: AccountManagerService
  ) {
    super(route, router)
    this.request = new CreateAccountReferenceCommand()
  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.save(),
      PreDefinedActions.add(),
      PreDefinedActions.list()
    ]
  }


  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    this.isLoading = true;

    forkJoin([
      this._mediator.send(new GetBaseValuesByUniqueNameQuery('personAccountReferenceTypeBaseId'))
    ]).subscribe(async ([
                          accountReferenceTypes
                        ]) => {
      this.accountReferenceTypes = accountReferenceTypes;
      this.accountManagerService.accountReferenceGroups.subscribe((value) => {
        let filteredGroups = value.filter(x => x.isVisible)
        // @ts-ignore
        let alreadySelectedIds = this.accountReferencesGroups.filter(x => x._selected).map(x => x.id)

        this.accountReferencesGroups = JSON.parse(JSON.stringify(filteredGroups)).map((x: AccountReferencesGroup) => {
          if (alreadySelectedIds.includes(x.id)) {
            // @ts-ignore
            x._selected = true;
          }
          return x;
        });
      })

      this.accountManagerService.accountHeads.subscribe((value) => {
        // @ts-ignore
        let alreadySelectedIds = this.accountHeads.filter(x => x._selected).map(x => x.id)

        this.accountHeads = JSON.parse(JSON.stringify(value)).map((x: AccountHead) => {
          if (alreadySelectedIds.includes(x.id)) {
            // @ts-ignore
            x._selected = true;
          }
          return x;
        });
      })
      await this.initialize()

    })
  }

  async initialize(entity?: AccountReference) {
    if (entity || this.getQueryParam('id')) {
      this.pageMode = PageModes.Update;
      if (!entity) entity = await this.get(this.getQueryParam('id'));
      this.request = new UpdateAccountReferenceCommand().mapFrom(entity);
      entity.accountReferencesGroupsIdList.forEach(x => {
        let group: any = this.accountReferencesGroups.find(y => y.id === x);
        if (group) group._selected = true;
      })

      entity.personalGroupAccountHeadIds?.forEach(x => {
        let accountHead: any = this.accountHeads.find(y => y.id === x)
        if (accountHead) accountHead._selected = true;
      })
    } else {
      this.pageMode = PageModes.Add;
      let request = new CreateAccountReferenceCommand();
      request.isActive = true;
      this.request = request;

    }
    this.isLoading = false;
  }

  async add() {
    let response = await this._mediator.send(<CreateAccountReferenceCommand>this.request)
    return await this.initialize(response);
  }

  async get(id: number) {
    return await this._mediator.send(new GetAccountReferenceQuery(id))
  }

  async update() {
    let response = await this._mediator.send(<UpdateAccountReferenceCommand>this.request);
    return await this.initialize(response);
  }

  async navigateToAccountReferencesList() {
    await this.router.navigateByUrl('/accounting/accountReferences/list')

  }

  handleGroupSelection(group: any) {
    let groupControl = this.form.controls['referenceGroupsId'].controls.find((x: FormControl) => x.value === group.id)
    if (group._selected === true && !groupControl) {
      this.form.controls['referenceGroupsId'].controls.push(new FormControl(group.id))
    } else {
      this.form.controls['referenceGroupsId'].controls.splice(
        this.form.controls['referenceGroupsId'].controls.indexOf(
          this.form.controls['referenceGroupsId'].controls.find((x: FormControl) => x.value === group.id)
        ), 1
      )
    }
    this.form.patchValue(this.form.getRawValue())
  }

  handleAccountHeadSelection(accountHead: any) {
    if (accountHead.lastLevel === true) {

      let accountHeadControl = this.form.controls['accountHeadIds'].controls.find((x: FormControl) => x.value === accountHead.id)
      if (accountHead._selected === true && !accountHeadControl) {
        this.form.controls['accountHeadIds'].controls.push(new FormControl(accountHead.id))
      } else {
        this.form.controls['accountHeadIds'].controls.splice(
          this.form.controls['accountHeadIds'].controls.indexOf(
            this.form.controls['accountHeadIds'].controls.find((x: FormControl) => x.value === accountHead.id)
          ), 1
        )
      }
      this.form.patchValue(this.form.getRawValue())
    } else {
      accountHead._selected = false;
    }
  }

  async reset() {
    await this.deleteQueryParam('id');
    (<any[]>this.accountReferencesGroups).filter(x => x._selected === true).forEach(x => {
      x._selected = false
    })
    return super.reset();
  }

  close(): any {
  }

  delete(param?: any): any {
  }

}
