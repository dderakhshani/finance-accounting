import {EventEmitter, Injectable} from '@angular/core';
import {Mediator} from "../../../core/services/mediator/mediator.service";
import {AccountHead} from "../entities/account-head";
import {BehaviorSubject, forkJoin} from "rxjs";
import {AccountReferencesGroup} from "../entities/account-references-group";
import {AccountReference} from "../entities/account-reference";

import {
  GetAccountReferencesGroupsQuery
} from "../repositories/account-reference-group/queries/get-account-references-groups-query";
import {GetAccountReferencesQuery} from "../repositories/account-reference/queries/get-account-references-query";
import {GetAccountHeadsQuery} from "../repositories/account-head/queries/get-account-heads-query";
import {GetCodeVoucherGroupsQuery} from "../repositories/code-voucher-group/queries/get-code-voucher-groups-query";
import {CodeVoucherGroup} from "../entities/code-voucher-group";
import {SearchQuery} from "../../../shared/services/search/models/search-query";
import {AccountCacheManagerService} from "./account-cache-manager.service";
import {Toastr_Service} from "../../../shared/services/toastrService/toastr_.service";

@Injectable({
  providedIn: 'root'
})
export class AccountManagerService {
  private dataRefreshTimeInSeconds = 1000;
  private isInitialized = false;
  private isInitializing = false;
  private initialized = new EventEmitter<boolean>();
  accountHeads: BehaviorSubject<AccountHead[]> = new BehaviorSubject<AccountHead[]>([]);
  accountReferenceGroups: BehaviorSubject<AccountReferencesGroup[]> = new BehaviorSubject<AccountReferencesGroup[]>([]);
  accountReferences: BehaviorSubject<AccountReference[]> = new BehaviorSubject<AccountReference[]>([]);
  codeVoucherGroups: BehaviorSubject<CodeVoucherGroup[]> = new BehaviorSubject<CodeVoucherGroup[]>([]);
  private groupsCache: { [key: string]: any[] } = {};
  private validationCache: { [key: string]: boolean } = {};
  get accountHeadLastLevels() {
    return this.accountHeads.value.filter(x => x.lastLevel)
  }
  constructor(
    private mediator: Mediator,
    private toastr: Toastr_Service,

  ) {

  }

  public async init(): Promise<void> {
    if (!this.isInitialized && !this.isInitializing) {
      this.isInitializing = true;

      return forkJoin([
        this.getAccountReferencesGroups(),
        this.getAccountHeads(),
        this.getAccountReferences(),
        this.getCodeVoucherGroups()
      ]).toPromise().then(() => {
        this.isInitialized = true;
        this.initialized.emit();
        let that = this;
        setTimeout(() => {
          that.updateData(true)
        }, this.dataRefreshTimeInSeconds * 1000)

      })
    }
  }


  private async getAccountHeads() {



    return await this.mediator.send(new GetAccountHeadsQuery(0, 0, [], 'fullCode')).then(res => {
      this.accountHeads.next(res);
    });
  }

  private async getAccountReferencesGroups() {

    return await this.mediator.send(new GetAccountReferencesGroupsQuery(0, 0, [], 'code')).then(res => {
      this.accountReferenceGroups.next(res.data);
    });
  }

  private async getAccountReferences() {
    return await this.mediator.send(new GetAccountReferencesQuery()).then(res => {
      this.accountReferences.next(res.data);

    });
  }

  private async getCodeVoucherGroups() {
    return await this.mediator.send(new GetCodeVoucherGroupsQuery(0, 0, [], 'id ASC')).then(res => {
      this.codeVoucherGroups.next(res.data)

    })
  }


  public async updateData(recursiveCall: boolean = false) {
    const updateFromServer = async () => {
      await this.updateAccountHeads();
      await this.updateAccountReferencesGroups();
      await this.updateAccountReferences();
    };
      await updateFromServer();

    if (recursiveCall) {
      setTimeout(() => this.updateData(true), this.dataRefreshTimeInSeconds * 1000);
    }
  }

  public async updateAccountHeads() {
    let latestCreated = this.accountHeads.value.map(x => x.createdAt).reduce(function (a, b) {
      return a > b ? a : b;
    })
    let latestModified = this.accountHeads.value.map(x => x.modifiedAt).reduce(function (a, b) {
      return a > b ? a : b;
    })

    let searchQueries: SearchQuery[] = [
      new SearchQuery({
        propertyName: 'createdAt',
        comparison: 'greaterThan',
        values: [latestCreated],
        nextOperand: 'or'
      }),
      new SearchQuery({
        propertyName: 'modifiedAt',
        comparison: 'greaterThan',
        values: [latestModified],
        nextOperand: 'or'
      }),
    ]
    await this.mediator.send(new GetAccountHeadsQuery(0, 0, searchQueries, 'fullCode')).then(res => {
      if (res?.length > 0) {
        let accountHeads = <AccountHead[]>JSON.parse(JSON.stringify(this.accountHeads.value));
        let newAccountHeads = res.filter(x => !accountHeads.map(x => x.id).includes(x.id))
        newAccountHeads.forEach(x => {
          accountHeads.push(x)
        })
        let updatedAccountHeads = res.filter(x => accountHeads.map(x => x.id).includes(x.id))

        accountHeads = accountHeads.map(old => updatedAccountHeads.find(modified => modified.id === old.id) || old);
        this.accountHeads.next(accountHeads)
      }
    })
  }

  public async updateAccountReferencesGroups() {
    let latestCreated = this.accountReferenceGroups.value.map(x => x.createdAt).reduce(function (a, b) {
      return a > b ? a : b;
    })
    let latestModified = this.accountReferenceGroups.value.map(x => x.modifiedAt).reduce(function (a, b) {
      return a > b ? a : b;
    })

    let searchQueries: SearchQuery[] = [
      new SearchQuery({
        propertyName: 'createdAt',
        comparison: 'greaterThan',
        values: [latestCreated],
        nextOperand: 'or'
      }),
      new SearchQuery({
        propertyName: 'modifiedAt',
        comparison: 'greaterThan',
        values: [latestModified],
        nextOperand: 'or'
      }),
    ]
    await this.mediator.send(new GetAccountReferencesGroupsQuery(0, 0, searchQueries, 'code')).then(res => {
      if (res?.data?.length > 0) {
        let accountReferenceGroups = <AccountReferencesGroup[]>JSON.parse(JSON.stringify(this.accountReferenceGroups.value));
        let newAccountReferenceGroups = res.data.filter(x => !accountReferenceGroups.map(x => x.id).includes(x.id))
        newAccountReferenceGroups.forEach(x => {
          accountReferenceGroups.push(x)
        })
        let updatedaccountReferenceGroups = res.data.filter(x => accountReferenceGroups.map(x => x.id).includes(x.id))

        accountReferenceGroups = accountReferenceGroups.map(old => updatedaccountReferenceGroups.find(modified => modified.id === old.id) || old);
        this.accountReferenceGroups.next(accountReferenceGroups)
      }
    })
  }

  public async updateAccountReferences() {
    let latestCreated = this.accountReferences.value.map(x => x.createdAt).reduce(function (a, b) {
      return a > b ? a : b;
    })
    let latestModified = this.accountReferences.value.map(x => x.modifiedAt).reduce(function (a, b) {
      return a > b ? a : b;
    })

    let searchQueries: SearchQuery[] = [
      new SearchQuery({
        propertyName: 'createdAt',
        comparison: 'greaterThan',
        values: [latestCreated],
        nextOperand: 'or'
      }),
      new SearchQuery({
        propertyName: 'modifiedAt',
        comparison: 'greaterThan',
        values: [latestModified],
        nextOperand: 'or'
      }),
    ]
    await this.mediator.send(new GetAccountReferencesQuery(0, 0, searchQueries, '')).then(res => {
      if (res?.data?.length > 0) {
        let accountReferences = <AccountReference[]>JSON.parse(JSON.stringify(this.accountReferences.value));
        let newAccountReferences = res.data.filter(x => !accountReferences.map(x => x.id).includes(x.id))
        newAccountReferences.forEach(x => {
          accountReferences.push(x)
        })
        let updatedAccountReferences = res.data.filter(x => accountReferences.map(x => x.id).includes(x.id))

        accountReferences = accountReferences.map(old => updatedAccountReferences.find(modified => modified.id === old.id) || old);
        this.accountReferences.next(accountReferences)
      }
    })
  }

  getGroupsRelatedToBoth(accountHeadId: number, accountReferenceId: number) {
    const cacheKey = `${accountHeadId}-${accountReferenceId}`;
    const accountHead = this.accountHeads.value.find(x => x.id === accountHeadId);
    const accountReference = this.accountReferences.value.find(x => x.id === accountReferenceId);
    let groupsRelatedToAccountHead: any[] = [];
    if (accountHead) {
      const groupIds = accountHead.accountHeadRelReferenceGroups.map(x => x.referenceGroupId);
      groupsRelatedToAccountHead = this.accountReferenceGroups.value.filter(x => groupIds.includes(x.id));
    }

    let groupsRelatedToAccountReference: any[] = [];
    if (accountReference) {
      const groupIds = accountReference.accountReferencesGroupsIdList;
      groupsRelatedToAccountReference = this.accountReferenceGroups.value.filter(x => groupIds.includes(x.id));
    }

    const commonGroups = this.findCommonGroups(
      groupsRelatedToAccountHead,
      groupsRelatedToAccountReference
    );

    if (commonGroups.length > 0) {
      this.groupsCache[cacheKey] = commonGroups;
      return commonGroups;
    }

    if (groupsRelatedToAccountHead.length > 0 && groupsRelatedToAccountReference.length > 0) {
      console.warn('گروه‌های مرتبط وجود دارد اما مشترکی بین آنها نیست!');
      this.toastr.showToast({title: "خطا", message: `گروه‌های مرتبط وجود دارد اما مشترکی بین آنها نیست!`, type: "error"});
      return [];
    }

    if (groupsRelatedToAccountHead.length === 0 && groupsRelatedToAccountReference.length === 0) {
      console.warn('هیچ گروه مرتبطی در هیچ یک وجود ندارد!');
      this.toastr.showToast({title: "خطا", message: `هیچ گروه مرتبطی در هیچ یک وجود ندارد!`, type: "error"});
      return [];
    }

    const availableGroups = [
      ...groupsRelatedToAccountHead,
      ...groupsRelatedToAccountReference
    ];

    return availableGroups;
  }

  private findCommonGroups(list1: any[], list2: any[]): any[] {
    const ids = new Set(list1.map(g => g.id));
    return list2.filter(g => ids.has(g.id));
  }

  validateGroupRelations(
    accountHeadId: number,
    accountReferenceId: number,
    groupId: number
  ): boolean {
    const cacheKey = `${accountHeadId}-${accountReferenceId}-${groupId}`;

    const group = this.accountReferenceGroups.value.find(g => g.id === groupId);

    if (!group) {
      console.error(`Group with ID ${groupId} not found`);
      return false;
    }

    const isValidHead = this.accountHeads.value.some(head =>
      head.id === accountHeadId &&
      head.accountHeadRelReferenceGroups?.some(rel => rel.referenceGroupId === groupId)
    );
    if (!isValidHead) {
      console.error(`NOT ValidHead`);
      return false;
    }

    const isValidReference = this.accountReferences.value.some(ref =>
      ref.id === accountReferenceId &&
      ref.accountReferencesGroupsIdList?.includes(groupId)
    );
    if (!isValidReference) {
      console.error(`NOT Valid Reference`);
      return false;
    }

    const checkParentHierarchy = (currentGroup: AccountReferencesGroup): boolean => {
      if (!currentGroup.parentId) return true;
      const parentGroup = this.accountReferenceGroups.value.find(g => g.id === currentGroup.parentId);
      return parentGroup ? checkParentHierarchy(parentGroup) : false;
    };

    const result = isValidHead &&
      isValidReference &&
      group.isVisible &&
      checkParentHierarchy(group)
    this.validationCache[cacheKey] = result;
    return result;
  }
  public getGroupsRelatedToAccountHead(accountHeadId: number):AccountReferencesGroup[] {
    let accountHead = this.accountHeads.value.find(x => x.id === accountHeadId);
    if (accountHead) {
      let groupIds = accountHead.accountHeadRelReferenceGroups.map(x => x.referenceGroupId);
      const result = [... this.accountReferenceGroups.value.filter(x => groupIds.includes(x.id))]
      return result
    }
    return []
  }

  public getGroupsRelatedToAccountReference(accountReferenceId: number):AccountReferencesGroup[]  {

    let accountReference = this.accountReferences.value.find(x => x.id === accountReferenceId);
    if (accountReference) {
      let groupIds = accountReference.accountReferencesGroupsIdList;
      return [... this.accountReferenceGroups.value.filter(x => groupIds.includes(x.id))]
    }
    return []
  }

  public getAccountReferencesRelatedToGroup(groupId: number) {
    if(!groupId) return []
    return [... this.accountReferences.value.filter(x => x.accountReferencesGroupsIdList.includes(groupId))]
  }

  public getAccountHeadsRelatedToGroup(groupId: number) {
    if(!groupId) return []
    return [...this.accountHeads.value.filter(x => x.accountHeadRelReferenceGroups.map(x => x.referenceGroupId).includes(groupId))]
  }


  public accountHeadDisplayFn(id: number): string {
    if (!id) return '';

    const accountHead = this.accountHeads.value.find(x => x.id === id);
    if (!accountHead) return '';

    const codePart = accountHead.code ? `(${accountHead.code})` : '';
    return [codePart, accountHead.title]
      .filter(part => part)
      .join(' ')
      .trim();
  }

  public accountReferenceGroupDisplayFn(id: number) {
    if (!id) return '';

    const accountReferenceGroup = this.accountReferenceGroups.value.find(x => x.id === id);
    if (!accountReferenceGroup) return '';
    const codePart = accountReferenceGroup.code ? `(${accountReferenceGroup.code})` : '';
    return [codePart, accountReferenceGroup.title]
      .filter(part => part)
      .join(' ')
      .trim();
  }

  public accountReferenceDisplayFn(id: number): string {
    if (!id) return '';

    const accountReference = this.accountReferences.value.find(x => x.id === id);
    if (!accountReference) return '';

    const codePart = accountReference.code ? `(${accountReference.code})` : '';
    return [codePart, accountReference.title]
      .filter(part => part)
      .join(' ')
      .trim();
  }

  public codeVoucherGroupDisplayFn(id: number): string {
    if (!id) return '';

    const codeVoucherGroup = this.codeVoucherGroups.value.find(x => x.id === id);
    if (!codeVoucherGroup) return '';

    const codePart = codeVoucherGroup.code ? `(${codeVoucherGroup.code})` : '';
    return [codePart, codeVoucherGroup.title]
      .filter(part => part)
      .join(' ')
      .trim();
  }


  getAccountHeadById(id: number): AccountHead | null {
    if (!id) return null;
    return this.accountHeads.value.find(x => x.id === id) || null;
  }

  getReferenceGroupById(id: number): AccountReferencesGroup | null {
    if (!id) return null;
    return this.accountReferenceGroups.value.find(x => x.id === id) || null;
  }

  getReferenceById(id: number): AccountReference | null {
    if (!id) return null;
    return this.accountReferences.value.find(x => x.id === id) || null;
  }
  getCodeVoucherGroupById(id: number): CodeVoucherGroup | null {
    if (!id) return null;
    return this.codeVoucherGroups.value.find(x => x.id === id) || null;
  }
  getAccountHeadByIds(ids: number[]): AccountHead[] {
    if (!ids || ids.length === 0) return [];
    return this.accountHeads.value.filter(x => ids.includes(x.id));
  }
  getReferenceGroupByIds(ids: number[]): AccountReferencesGroup[] {
    if (!ids || ids.length === 0) return [];
    return [...this.accountReferenceGroups.value.filter(x => ids.includes(x.id))];
  }
  getReferenceByIds(ids: number[]): AccountReference[] {
    if (!ids || ids.length === 0) return [];
    return [...this.accountReferences.value.filter(x => ids.includes(x.id))];
  }
  getCodeVoucherGroupByIds(ids: number[]): CodeVoucherGroup[] {
    if (!ids || ids.length === 0) return [];
    return [...this.codeVoucherGroups.value.filter(x => ids.includes(x.id))];
  }
}

