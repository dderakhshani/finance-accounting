import {Component, OnDestroy, Renderer2} from '@angular/core';
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {CreateVouchersHeadCommand} from "../../../../repositories/voucher-head/commands/create-vouchers-head-command";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {PageModes} from "../../../../../../core/enums/page-modes";
import {CodeVoucherGroup} from "../../../../entities/code-voucher-group";
import {AccountHead} from "../../../../entities/account-head";
import {AccountReference} from "../../../../entities/account-reference";
import {ActivatedRoute, Router} from "@angular/router";
import {IdentityService} from "../../../../../identity/repositories/identity.service";
import {UpdateVouchersHeadCommand} from "../../../../repositories/voucher-head/commands/update-vouchers-head-command";
import {GetVoucherHeadQuery} from "../../../../repositories/voucher-head/queries/get-voucher-head-query";
import {VoucherDetail} from "../../../../entities/voucher-detail";

import {FormArray, FormControl, FormGroup} from "@angular/forms";
import {AccountReferencesGroup} from "../../../../entities/account-references-group";
import {
  Action,
  ActionTypes,
  PreDefinedActions
} from "../../../../../../core/components/custom/action-bar/action-bar.component";
import {
  CreateVoucherDetailCommand
} from "../../../../repositories/voucher-detail/commands/create-voucher-detail-command";
import {
  UpdateVoucherDetailCommand
} from "../../../../repositories/voucher-detail/commands/update-voucher-detail-command";
import {GetVoucherHeadsQuery} from "../../../../repositories/voucher-head/queries/get-voucher-heads-query";
import {SearchQuery} from "../../../../../../shared/services/search/models/search-query";
import {TabManagerService} from "../../../../../../layouts/main-container/tab-manager.service";
import {VoucherHead} from "../../../../entities/voucher-head";
import {AccountManagerService} from "../../../../services/account-manager.service";
import {LocalStorageRepository} from "../../../../../../core/services/storage/local-storage-repository.service";
import {PagesCommonService} from 'src/app/shared/services/pages/pages-common.service';
import {AccountingHubService} from "../../../../services/accounting-hub.service";
import {MoneyPipe} from 'src/app/core/pipes/money.pipe';
import {
  AccountHeadPrintModel,
  level3Model,
  level4Model
} from 'src/app/modules/accounting/entities/account-head-print-model';
import {ToPersianDatePipe} from 'src/app/core/pipes/to-persian-date.pipe';
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {
  VoucherHeadHistoryDialogComponent
} from "../../../../components/voucher-head-history-dialog/voucher-head-history-dialog.component";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {
  ConfirmDialogComponent,
  ConfirmDialogConfig, ConfirmDialogIcons
} from "../../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component";
import {environment} from "../../../../../../../environments/environment";
import {SalaryExcelImportDialogComponent} from "./salary-excel-import-dialog/salary-excel-import-dialog.component";
import {DeleteVouchersHeadCommand} from "../../../../repositories/voucher-head/commands/delete-voucher-head-command";
import {Tab} from "../../../../../../layouts/main-container/models/tab";

@Component({
  selector: 'app-add-voucher-head',
  templateUrl: './add-voucher-head.component.html',
  styleUrls: ['./add-voucher-head.component.scss']
})
export class AddVoucherHeadComponent extends BaseComponent implements OnDestroy {

  accountHeads: AccountHead[] = [];
  accountReferences: AccountReference[] = [];
  accountReferencesGroups: AccountReferencesGroup[] = [];
  codeVoucherGroups: CodeVoucherGroup[] = [];
  voucherStates: any[] = [];

  selectedVoucherDetail: VoucherDetail = new VoucherDetail();
  // selectedVoucherDetailIds: number[] = [];
  credit: number | undefined = undefined;
  debit: number | undefined = undefined;
  remain: number | undefined = undefined;


  voucherHeadPanelState: boolean = false;
  voucherDetailSumPanelState: boolean = false;

  createdVoucherDetails: CreateVoucherDetailCommand[] = [];
  updatedVoucherDetails: UpdateVoucherDetailCommand[] = [];
  deletedVoucherDetails: UpdateVoucherDetailCommand[] = [];

  previousVoucherHeadId!: number;
  nextVoucherHeadId!: number;
  applicationUserFullName!: string;

  goToVoucherByNumberFormControl = new FormControl()

  constructor(
    private _mediator: Mediator,
    private _route: ActivatedRoute,
    private _router: Router,
    private _identityService: IdentityService,
    private tabService: TabManagerService,
    private accountManagerService: AccountManagerService,
    private localStorageRepository: LocalStorageRepository,
    public Service: PagesCommonService,
    private accountingHubService: AccountingHubService,
    private matDialog: MatDialog,
    private notificationService: NotificationService,
    public identityService: IdentityService,
    public renderer: Renderer2
  ) {
    super();
    this.request = new CreateVouchersHeadCommand();
    this.isLoading = true
    _identityService._applicationUser.subscribe(res => {
      if (res.isAuthenticated) {

        this.applicationUserFullName = res.fullName;
      }
    });
  }

  ngAfterViewInit() {

   this.loadingSubscription = this.loadingFinished.subscribe(() => {
     this.actionBar.actions = [
       PreDefinedActions.save().setDisable(!this.identityService.doesHavePermission('VoucherHeadsList-Add') || !this.identityService.doesHavePermission('VoucherHeadsList-Edit')),
       PreDefinedActions.add().setDisable(!this.identityService.doesHavePermission('VoucherHeadsList-Add')),
       PreDefinedActions.refresh(),
       PreDefinedActions.list(),
       <Action>{
         title: 'بعدی',
         color: 'primary',
         type: ActionTypes.custom,
         uniqueName: 'nextVoucher',
         disabled: this.nextVoucherHeadId === 0,
         icon: 'arrow_back'
       },
       <Action>{
         title: 'قبلی',
         color: 'primary',
         type: ActionTypes.custom,
         uniqueName: 'previousVoucher',
         disabled: this.previousVoucherHeadId === 0,
         icon: 'arrow_forward'
       },
       <Action>{
         title: 'کپی',
         color: 'primary',
         type: ActionTypes.custom,
         uniqueName: 'copyVoucher',
         disabled: !this.form.controls['id'].value,
         icon: 'content_copy'
       },
       // <Action>{
       //   title: 'تاریخچه',
       //   color: 'primary',
       //   type: ActionTypes.custom,
       //   uniqueName: 'showHistory',
       //   disabled: !this.form.value.id,
       //   icon: 'history'
       // }
       <Action>{
         title: 'ثبت اکسل حقوق و دستمزد',
         color: 'primary',
         type: ActionTypes.custom,
         uniqueName: 'salaryExcel',
         disabled: this.form.value.id,
         show: this.identityService.doesHavePermission('Excel-Salaries'),
         icon: 'publish'
       },
     ]
   })
  }

  async ngOnInit() {

    await this.resolve();
  }


  ngOnDestroy() {
    if (this.pageMode === PageModes.Add && this.form.controls['vouchersDetailsList'].length === 0) localStorage.removeItem('tempVoucher');
    this.form?.valueChanges?.unsubscribe();
    super.ngOnDestroy();
  }

  async resolve() {
    this.isLoading = true;

    this.addShortkeys()
    this.voucherStates = [
      {
        title: 'موقت',
        id: 1
      },
      {
        title: 'مرور',
        id: 2
      },
      {
        title: 'دائم',
        id: 3
      },
    ];
    this.accountManagerService.accountHeads.subscribe(res => {
      this.accountHeads = res;
    });
    this.accountManagerService.accountReferences.subscribe(res => {
      this.accountReferences = res
    });
    this.accountManagerService.accountReferenceGroups.subscribe(res => {
      this.accountReferencesGroups = res
    });
    this.accountManagerService.codeVoucherGroups.subscribe(res => {
      this.codeVoucherGroups = res
      if (this.pageMode === PageModes.Add) {
        this.form.controls['codeVoucherGroupId'].setValue(this.codeVoucherGroups.find(x => x.uniqueName === 'ManualDoucument')?.id);
        this.form.controls['voucherDescription'].setValue(this.codeVoucherGroups.find(x => x.uniqueName === 'ManualDoucument')?.title);
      }
    });


    await this.initialize();
  }

  async initialize(entity?: any, silent: boolean = false) {
    this.createdVoucherDetails = []
    this.updatedVoucherDetails = []
    this.deletedVoucherDetails = []
    this.pageMode = PageModes.Add;
    if (entity || this.getQueryParam('id')) {
      this.pageMode = PageModes.Update;
      if (!entity) entity = await this.get(this.getQueryParam('id'));
      else await this.addQueryParam('id', entity.id);

      let myTab = this.tabManagerService.tabs.find(x => x.guid == this.componentId)
      if (myTab) myTab.title = 'سند حسابداری ' + entity.voucherNo;
      this.request = new UpdateVouchersHeadCommand().mapFrom(entity);
      let nextAndPreviousVoucherSearchQueries = [];
      nextAndPreviousVoucherSearchQueries.push(new SearchQuery({
        propertyName: 'voucherNo',
        comparison: 'equals',
        values: [+entity.voucherNo + 1],
        nextOperand: 'or'
      }))
      if (entity.voucherNo > 1)

        nextAndPreviousVoucherSearchQueries.push(
          new SearchQuery({
            propertyName: 'voucherNo',
            comparison: 'equals',
            values: [+entity.voucherNo - 1],
            nextOperand: 'or'
          }))
      await this._mediator.send(new GetVoucherHeadsQuery(0, 2, nextAndPreviousVoucherSearchQueries, "voucherNo DESC")).then(res => {
        this.nextVoucherHeadId = res.data.find((x: VoucherHead) => x.voucherNo > entity.voucherNo)?.id ?? 0
        this.previousVoucherHeadId = res.data.find((x: VoucherHead) => x.voucherNo < entity.voucherNo)?.id ?? 0
      })
      this.calculateCreditDebitAndRemain()
      if (entity.hasCorrectionRequest) {
        this.disableForm()
        if (!silent) {
          let dialogConfig = new MatDialogConfig();
          let confirmDialogData = new ConfirmDialogConfig()
          confirmDialogData.color = 'red'
          confirmDialogData.title = 'اخطار'
          confirmDialogData.message = `سندی که قصد ویرایش آن را دارید درخواست تغییرات فعال دارد و قابل ویرایش نمیباشد.`;
          confirmDialogData.actions = {
            confirm: {title: 'تایید', show: true},
            cancel: {title: '', show: false},
          }
          dialogConfig.data = confirmDialogData
          this.matDialog.open(ConfirmDialogComponent, dialogConfig)
        }
      }
    } else this.createNewVoucher();

    this.form.controls['creator'].disable();
    this.form.controls['voucherStateId'].disable();

    this.form.valueChanges.subscribe((newValue: any) => {
      this.calculateCreditDebitAndRemain(newValue)
      // this.selectedVoucherDetail = this.selectedVoucherDetail
    });

    if(!(this.identityService.doesHavePermission('VoucherHeadsList-Add') || this.identityService.doesHavePermission('VoucherHeadsList-Add'))) {
      this.disableForm()
    }
    this.isLoading = false;
  }
  disableForm(){
    this.form.disable({emitEvent: false});
    this.form.controls['vouchersDetailsList'].controls.forEach((x:FormGroup) => {
      x.controls['selected'].enable({ emitEvent: false})
    })

  }

  addShortkeys() {
    let windowRenderer = this.renderer.listen('window', 'keydown', (event) => {
      if (this.tabManagerService.activeTab.guid !== this.componentId) return;
      // Check if the Alt key is pressed
      if (event.altKey) {

        switch (event.code.toLowerCase()) {
          case 'keys':
            // Alt + S
            this.submitCheck();
            event.preventDefault();
            break;
          case 'keyr':
            // Alt + S
            this.initialize();
            event.preventDefault();
            break;
          default:
            break;
        }
      }
    });
    this.windowEventListeners.push(windowRenderer)
  }

  async createNewVoucher() {
    if (localStorage.getItem('tempVoucherHeads')) localStorage.removeItem('tempVoucherHeads')
    if (localStorage.getItem('tempVoucher')) localStorage.removeItem('tempVoucher')
    let command = new CreateVouchersHeadCommand();
    // if (localStorage.getItem('tempVoucher')) {
    //   command.mapFromItSelf(JSON.parse(<string>localStorage.getItem('tempVoucher')))
    //   this.request = command;
    //   this.calculateCreditDebitAndRemain()
    //   let dialogConfig = new MatDialogConfig();
    //   let confirmDialogData = new ConfirmDialogConfig()
    //   confirmDialogData.title = 'اخطار'
    //   confirmDialogData.message = `آیا مایل به ادامه ویرایش سند معلق خود هستید؟ `;
    //   confirmDialogData.description = '(در صورت انتخاب \"ثبت سند جدید\"، سند معلق شما پاک خواهد شد)';
    //   confirmDialogData.color = 'amber';
    //   confirmDialogData.icon = ConfirmDialogIcons.warning;
    //   confirmDialogData.actions = {
    //     confirm: {title: 'ثبت سند جدید', show: true},
    //     cancel: {title: 'ادامه و ویرایش سند معلق', show: true},
    //   }
    //   dialogConfig.data = confirmDialogData
    //   let dialogRef = this.matDialog.open(ConfirmDialogComponent, dialogConfig)
    //   dialogRef.afterClosed().subscribe((confirmed) => {
    //     if (confirmed) {
    //       localStorage.removeItem('tempVoucher')
    //       this.initialize()
    //     }
    //   })
    // } else {
    if (this.getQueryParam('cfId')) {
      let voucherHeadToCopyFrom = await this.get(this.getQueryParam('cfId'))
      let command = new CreateVouchersHeadCommand().mapFrom(voucherHeadToCopyFrom);
      command.creatorFirstName = this._identityService.applicationUser.fullName;
      command.creatorLastName = this._identityService.applicationUser.lastName;
      command.voucherNo = undefined;
      command.voucherDailyId = undefined;
      command.voucherStateId = this.voucherStates[0].id;
      this.request = command;
      this.credit = 0;
      this.debit = 0;
      this.remain = this.credit - this.debit
      this.deleteQueryParam('cfId')
    } else {
      command.voucherDate = new Date(new Date().setHours(0, 0, 0, 0))
      command.voucherStateId = this.voucherStates[0].id;
      command.codeVoucherGroupId = this.codeVoucherGroups.find(x => x.uniqueName === 'ManualDoucument')?.id;
      command.voucherDescription = this.codeVoucherGroups.find(x => x.uniqueName === 'ManualDoucument')?.title;
      command.creatorFirstName = this._identityService.applicationUser.fullName;
      command.creatorLastName = this._identityService.applicationUser.lastName;
      this.request = command;
      this.credit = 0;
      this.debit = 0;
      this.remain = this.credit - this.debit
    }

    // }


    this.form.valueChanges.subscribe(() => {
      localStorage.setItem('tempVoucher', JSON.stringify(this.request))
    })
  }

  calculateCreditDebitAndRemain(voucherHead?: VoucherHead) {
    if (!voucherHead) voucherHead = this.form.getRawValue();
    // @ts-ignore
    this.credit = voucherHead.vouchersDetailsList.map((voucherDetail: any) => voucherDetail.credit ?? 0).reduce((prev: any, next: any) => {
      return (+prev ?? 0) + (+next ?? 0)
    }, 0);
    // @ts-ignore
    this.debit = voucherHead.vouchersDetailsList.map((voucherDetail: any) => voucherDetail.debit ?? 0).reduce((prev: any, next: any) => {
      return (+prev ?? 0) + (+next ?? 0)
    }, 0);
    //@ts-ignore
    this.remain = this.credit - this.debit

  }

  async submitCheck() {
    if (this.remain !== 0) {
      let dialogConfig = new MatDialogConfig();
      let confirmDialogData = new ConfirmDialogConfig()
      confirmDialogData.title = 'اخطار'
      confirmDialogData.message = 'سند غیر موازنه است، آیا میخواهید ثبت کنید؟'
      dialogConfig.data = confirmDialogData
      let dialogRef = this.matDialog.open(ConfirmDialogComponent, dialogConfig)
      return dialogRef.afterClosed().subscribe(async (isUserAgreed: boolean) => {
        if (isUserAgreed === true) return await this.submit()
      })
    } else {
      return await this.submit();
    }

  }

  async add() {

    this.isLoading = true;
    await this._mediator.send(<CreateVouchersHeadCommand>this.request)
      .then(async (response) => {
        localStorage.removeItem('tempVoucher');
        await this.initialize(response);
      })

      .catch(() => {
        this.isLoading = false;
      });
  }

  async update() {
    if ((<UpdateVouchersHeadCommand>this.request).hasCorrectionRequest) {
      let dialogConfig = new MatDialogConfig();
      let confirmDialogData = new ConfirmDialogConfig()
      confirmDialogData.color = 'red'
      confirmDialogData.title = 'اخطار'
      confirmDialogData.message = `سندی که قصد ویرایش آن را دارید درخواست تغییرات فعال دارد و قابل ویرایش نمیباشد.`;
      confirmDialogData.actions = {
        confirm: {title: 'تایید', show: true},
        cancel: {title: '', show: false},
      }
      dialogConfig.data = confirmDialogData
      this.matDialog.open(ConfirmDialogComponent, dialogConfig)
    } else {
      this.isLoading = true;
      (<UpdateVouchersHeadCommand>this.request).vouchersDetailsCreatedList = this.createdVoucherDetails;
      (<UpdateVouchersHeadCommand>this.request).vouchersDetailsUpdatedList = this.updatedVoucherDetails;
      (<UpdateVouchersHeadCommand>this.request).vouchersDetailsDeletedList = this.deletedVoucherDetails;
      await this._mediator.send(<UpdateVouchersHeadCommand>this.request)
        .then((response) => {
          this.initialize(response, true);
        }).catch(() => {
          this.isLoading = false
        });
    }

  }

  async get(id: number) {
    return await this._mediator.send(new GetVoucherHeadQuery(id, true))
  }

  async delete() {
    if (!(this.identityService.applicationUser.permissions.filter(x => x.uniqueName === 'VoucherHead-Delete')?.length > 0))
      this.notificationService.showWarningMessage('شما دسترسی مورد نیاز برای حذف سند را ندارید.')
    await this._mediator.send(new DeleteVouchersHeadCommand(this.form.getRawValue()?.id)).then(() => {
      this.tabManagerService.closeTab(<Tab>this.tabManagerService.tabs.find(x => x.guid === this.componentId))
    })
  }

  close(): any {
  }

  async reset() {
    await this.deleteQueryParam('id');
    await this.deleteQueryParam('cfId');
    if (localStorage.getItem('tempVoucher')) {
      this.form?.valueChanges?.unsubscribe();
      localStorage.removeItem('tempVoucher')
    }
    this.initialize()
  }

  handleVoucherGroupChange(groupId: number) {
    this.form.patchValue({
      voucherDescription: this.codeVoucherGroups.find(x => x.id === groupId)?.title
    })
  }

  handleVoucherDetailSelection(voucherDetail: FormGroup) {

    this.selectedVoucherDetail = voucherDetail.getRawValue();
    this.selectedVoucherDetail.levelName1 = this.accountHeads.find(x => x.id === this.selectedVoucherDetail.level1)?.title ?? ' ';
    this.selectedVoucherDetail.levelName2 = this.accountHeads.find(x => x.id === this.selectedVoucherDetail.level2)?.title ?? ' ';
    this.selectedVoucherDetail.levelName3 = this.accountHeads.find(x => x.id === this.selectedVoucherDetail.level3)?.title ?? ' ';
    this.selectedVoucherDetail.referenceTitle1 = this.accountReferences.find(x => x.id === this.selectedVoucherDetail.referenceId1)?.title ?? ' ';
    this.selectedVoucherDetail.accountReferencesGroupTitle = this.accountReferencesGroups.find(x => x.id === this.selectedVoucherDetail.accountReferencesGroupId)?.title ?? ' ';
    // //@ts-ignore
    // if (this.selectedVoucherDetail.selected == false) {
    //   this.selectedVoucherDetailIds.push(this.selectedVoucherDetail.id);
    // }
    // else {
    //   this.selectedVoucherDetailIds = this.selectedVoucherDetailIds.filter(a => a != this.selectedVoucherDetail.id);
    // }

    if(!(this.identityService.doesHavePermission('VoucherHeadsList-Add') || this.identityService.doesHavePermission('VoucherHeadsList-Add'))) {
      this.disableForm()
    }
  }


  async navigateToVoucherHeadsList() {
    await this._router.navigateByUrl('/accounting/voucherHead/list')
  }


  getCodeVoucherGroupTitle(id: number) {
    return this.codeVoucherGroups.find(x => x.id === id)?.title
  }

  getVoucherStateTitle(id: number) {
    return this.voucherStates.find(x => x.id === id)?.title
  }

  addVoucherDetail(payload: any) {
    let vouchersDetailsListForm: FormArray = this.form.controls['vouchersDetailsList'];
    vouchersDetailsListForm.push(payload.form);
    if (this.pageMode === PageModes.Update) this.createdVoucherDetails.push(payload.command)
    this.form.controls['vouchersDetailsList'].controls = [...this.form.controls['vouchersDetailsList'].controls]
    this.form.updateValueAndValidity({onlySelf: false, emitEvent: true})

  }

  updateVoucherDetail(form: FormGroup) {
    if (form.value.id) {
      let updatedVoucherDetail = this.updatedVoucherDetails.find(x => x.id === form.value.id);
      if (updatedVoucherDetail) this.updatedVoucherDetails.splice(this.updatedVoucherDetails.indexOf(updatedVoucherDetail), 1)
      this.updatedVoucherDetails.push(form.getRawValue())
    }
    this.form.controls['vouchersDetailsList'].controls = [...this.form.controls['vouchersDetailsList'].controls]
    this.form.updateValueAndValidity({onlySelf: false, emitEvent: true})

  }

  deleteVoucherDetail(vouchersToDelete: FormGroup[]) {
    let voucherDetailsListForm: FormArray = this.form.controls['vouchersDetailsList'];

    vouchersToDelete.forEach((voucherToDeleteForm: FormGroup) => {
      if (voucherToDeleteForm.getRawValue().id) {
        this.deletedVoucherDetails.push(voucherToDeleteForm.getRawValue())
        this.updatedVoucherDetails = this.updatedVoucherDetails.filter(x => x.id !== voucherToDeleteForm.getRawValue().id)
      } else {

        this.createdVoucherDetails = this.createdVoucherDetails.filter(x => x.requestId !== voucherToDeleteForm.getRawValue()?.requestId)
      }
      voucherDetailsListForm.removeAt(this.form.controls['vouchersDetailsList'].controls.indexOf(voucherToDeleteForm))
    })

    this.form.controls['vouchersDetailsList'].controls = [...this.form.controls['vouchersDetailsList'].controls]
    this.form.updateValueAndValidity({onlySelf: false, emitEvent: true})
  }


  async handleCustomAction(action: Action) {

    if (action.uniqueName === 'nextVoucher' && this.nextVoucherHeadId) {
      let currentPageUrl = this._router.url;
      let nextPageUrl = currentPageUrl.replace((<number>(<UpdateVouchersHeadCommand>this.request).id).toString(), this.nextVoucherHeadId.toString())
      if (!this.tabService.tabs.find(x => x.actualRoute === nextPageUrl)) {
        await this.addQueryParam("id", this.nextVoucherHeadId)
        await this.initialize()
      } else {
        await this._router.navigateByUrl(nextPageUrl)
      }
    }
    if (action.uniqueName === 'previousVoucher' && this.previousVoucherHeadId) {
      let currentPageUrl = this._router.url;
      let previousPageUrl = currentPageUrl.replace((<number>(<UpdateVouchersHeadCommand>this.request).id).toString(), this.previousVoucherHeadId.toString())
      if (!this.tabService.tabs.find(x => x.instanceRoute === previousPageUrl)) {
        await this.addQueryParam("id", this.previousVoucherHeadId)
        await this.initialize()
      } else {
        await this._router.navigateByUrl(previousPageUrl)
      }
    }
    if (action.uniqueName === 'copyVoucher' && this.form.value.id) {
      await this._router.navigateByUrl(`/accounting/voucherHead/add?cfId=${this.form.value.id}`)

    }
    if (action.uniqueName === 'showHistory') {
      let dialogConfig = new MatDialogConfig();
      dialogConfig.data = {
        id: this.form.value.id,
      }
      this.matDialog.open(VoucherHeadHistoryDialogComponent, dialogConfig)
    }

    if (action.uniqueName === 'salaryExcel') {
      let dialogConfig = new MatDialogConfig();
      let dialogRef = this.matDialog.open(SalaryExcelImportDialogComponent, dialogConfig)

      dialogRef.afterClosed().subscribe((res: VoucherDetail[]) => {

        res.map(x => {
          return new CreateVoucherDetailCommand().mapFrom(x);
        }).forEach((command: any) => {
          let voucherDetailForm = <FormGroup>this.createForm(command, true)
          this.addVoucherDetail({form: voucherDetailForm, command})
        })

        this.form.controls['vouchersDetailsList'].controls = [...this.form.controls['vouchersDetailsList'].controls]

      })
    }
  }


  async goToVoucherByNumber() {
    let voucherNo = this.goToVoucherByNumberFormControl.value;
    if (!voucherNo) this.notificationService.showWarningMessage('سندی با این شماره یافت نشد')

    await this._mediator.send(new GetVoucherHeadsQuery(0, 1, [new SearchQuery({
      propertyName: 'voucherNo',
      comparison: 'equals',
      values: [voucherNo]
    })])).then(res => {
      if (res.data[0]) {
        this.reset()
        this.addQueryParam('id', res.data[0].id)
        this.initialize()
        this.goToVoucherByNumberFormControl.setValue(null)
      } else this.notificationService.showWarningMessage('سندی با این شماره یافت نشد')
    })
  }

  async printSimpleVoucherHead() {

    //@ts-ignore
    if (this.getQueryParam('id')) {

      let token = this.identityService.getToken();

      //@ts-ignore
      window.open(`${environment.apiURL}/accountingreports/VouchersHeadprint/index?access_token=${token}&id=${this.getQueryParam('id')}&printType=2&selectedVoucherDetailIds=${this.form.controls['vouchersDetailsList'].controls.filter((x: any) => x.value.selected === true).map((x: any) => x.value.id)}`);
    }
  }

  async printSimpleVoucherHead1() {

    if (this.getQueryParam('id')) {
      let result = await this._mediator.send(new GetVoucherHeadQuery(this.getQueryParam('id'), true, true, this.form.controls['vouchersDetailsList'].controls.filter((x: any) => x.value.selected === true).map((x: any) => x.value.id), 2));


      let printContents =
        '';
      if (result.datas.length > 0) {


        printContents = `
        <div style="text-align: center;border-top: 1px solid #000000;padding-top: 20px;">
           <div style="width: 20%;float: left;font-size: 14px;">
                 تاریخ چاپ: ${this.Service.toPersianDate(new Date())}
                 <br/>
                 اپراتور: ${this.applicationUserFullName}
           </div>
           <div style="width: 60%;float: left;">
                   ${this.identityService.applicationUser.companies.find(x => x.id == this.identityService.applicationUser.companyId)?.title}
                  <br>
                  سند حسابداری
                  <br><br><br>
           </div>
              <div style="width: 23%;float: right;font-size: 14px;margin-bottom: 10px;padding: 6px;margin-right: 9px;">
                 شماره سند: ${result.datas[0].voucherNo}
              </div>
              <div style="width: 23%;float: right;font-size: 14px;margin-bottom: 10px;padding: 6px;margin-right: 2px;">
                     تاریخ سند: ${this.Service.toPersianDate(result.datas[0].voucherDate)}
              </div>
              <div style="width: 23%;float: right;font-size: 14px;margin-bottom: 10px;padding: 6px;margin-right: 2px;">
                     شماره روزانه سند: ${result.datas[0].voucherDailyId}
              </div>
              <div style="width: 23%;float: right;font-size: 14px;margin-bottom: 10px;padding: 6px;margin-right: 2px;">
                     نوع سند: ${result.datas[0].codeVocherGroupName}
              </div>
              <div style="width: 99%;float: right;font-size: 14px;margin-bottom: 10px;padding: 6px;margin-right: 2px; text-align:right;border-bottom: 1px solid #000000">
                     نوع سند: ${result.datas[0].voucherDescription}
              </div>
       </div>`

        printContents += `<table>
      <thead>
                 <tr>
                   <th style="width: 1%" scope="col">ردیف</th>
                   <th style="width: 8%" scope="col">معین</th>
                   <th style="width: 11%" scope="col">تفصیل</th>
                   <th style="width: 34%" scope="col">شرح</th>
                   <th style="width: 5%" scope="col">بدهکار</th>
                   <th style="width: 5%" scope="col">بستانکار</th>
                 </tr>
      </thead>
      <tbody>`;

        for (let i = 0; i < result.datas.length; i++) {
          let data = result.datas[i];
          printContents += `
          <tr style="direction: rtl;text-align: center;">
          <td style="border-top: none; border: 1px solid black;font-size: 9px;">${i + 1}</td>
          <td style="border-top: none;border: 1px solid black;font-size: 9px;">${data.accountHeadCode}</td>
          <td style="border-top: none;border: 1px solid black;font-size: 9px;">${data.accountReferenceCode}</td>
          <td style="border-top: none;border: 1px solid black;font-size: 9px;">${data.voucherRowDescription}</td>
          <td style="border-top: none;border-bottom: 1px solid black;font-size: 9px;">${data.debit?.toLocaleString()}</td>
          <td style="border-top: none;border-bottom: 1px solid black;font-size: 9px;">${data.credit?.toLocaleString()}</td>
          </tr>
          `;

          // printContents += `
          // <tr  style="direction: rtl;text-align: right;">

          // <td colspan='3' style="border-bottom: 1px solid black;padding: 12px;">${data.description}</td>

          // </tr>
          // `;
        }

        printContents += `
          <tr style="direction: rtl;text-align: center;">
          <td style="border-top: none; border: 1px solid black"></td>
          <td style="border-top: none;border: 1px solid black"></td>
          <td style="border-top: none;border: 1px solid black"></td>
          <td style="border-top: none;border: 1px solid black">جمع سند</td>
          <td style="border-top: none;border-bottom: 1px solid black;">${result.sumDebit?.toLocaleString()}</td>
          <td style="border-top: none;border-bottom: 1px solid black;">${result.sumCredit?.toLocaleString()}</td>
          </tr>
          </tbody></table>
          `;
        printContents += `
          <div style="text-align: center;padding: 30px;width:100%">
              <div style="width:25%;float: left;">
              مدیر مالی:
              </div>
              <div style="width:25%;float: left;">
              رئیس حسابداری:
              </div>
              <div style="width:25%;float: left;">
              رسیدگی کننده:
              </div>
              <div style="width:25%;float: left;">
              تنظیم کننده: ${this.form.get('creator')?.value}
              </div>
          </div>`;
      }

      this.Service.onPrint(printContents, "")
    }
  }

  async printVoucherHead() {

    if (this.getQueryParam('id')) {
      let result = await this._mediator.send(new GetVoucherHeadQuery(this.getQueryParam('id'), true, true, this.form.controls['vouchersDetailsList'].controls.filter((x: any) => x.value.selected === true).map((x: any) => x.value.id), 1));


      let printContents = `<style>td{
        word-break:none !important;
      }

      @media print
      {
        tr { page-break-inside:initial !important;}
        td { page-break-inside:initial !important;}

      }
      </style>`;
      if (result.creditDatas.length > 0 || result.debitDatas.length > 0) {


        printContents = `
        <div style="text-align: center;">

           <div style="width: 100%;float: left;text-align: center;">
                   ${this.identityService.applicationUser.companies.find(x => x.id == this.identityService.applicationUser.companyId)?.title}
                  <br>
                  سند حسابداری
           </div>
              <div style="width: 25%;float: right;font-size: 14px;margin-bottom: 10px;padding: 6px;margin-right: 9px;">
                 شماره سند: ${result.debitDatas[0].voucherNo}
              </div>
              <div style="width: 25%;float: left;font-size: 14px;margin-bottom: 10px;padding: 6px;margin-right: 2px;">
                     تاریخ سند: ${new ToPersianDatePipe().transform(result.debitDatas[0].voucherDate)}
              </div>
       </div>`

        printContents += `<table>
      <thead>
                 <tr>
                   <th style="width: 1%" scope="col">ردیف</th>
                   <th style="width: 4%" scope="col">کد حساب</th>
                   <th style="width: 31%" scope="col">شرح</th>
                   <th style="width: 10%" scope="col">مبلغ جزء</th>
                   <th style="width: 10%" scope="col">بدهکار</th>
                   <th style="width: 10%" scope="col">بستانکار</th>
                 </tr>
      </thead>
      <tbody>`;

        let allDebit = 0;
        let allCredit = 0;
        let index = 0;
        let model: AccountHeadPrintModel[] = [];
        for (let i = 0; i < result.debitOrders.length; i++) {
          let data = result.debitOrders[i];
          index++;
          ///////////////////////// کد حساب

          printContents += `
<tr style="direction: rtl;text-align: center;">
<td  style="direction: rtl;text-align: center;font-size:12px;">${index}</td>
<td  style="direction: rtl;text-align: center;">
<p style="font-size: 14px;max-height: 40px;height: 40px;"> ${data.split('-')[0]}</p>
`;
          //@ts-ignore
          let level2data = result.debitDatas.filter(a => a.level2AccountHead == data).sort(a => a.accountHeadCode);


          for (let j = 0; j < level2data.length;) {

            //@ts-ignore
            let level3data = level2data.filter(a => a.accountHeadCode == level2data[j]?.accountHeadCode);

            printContents += `
  <p style="font-size: 12;max-height: 40px;height: 40px;">${level2data[j].accountHeadCode.split('-')[0]}</p>`
            j += level3data.length;
            for (let k = 0; k < level3data.length; k++) {

              printContents += `
    <p style="font-size: 9px;max-height: 40px;height: 40px;">${level3data[k].accountReferenceCode.split('-')[0]}</p>`
            }

          }


          printContents += `</td>
          <td  style="direction: rtl;text-align: right;">
          <p style="font-size: 14px;max-height: 40px;height: 40px;"> ${data.split('-')[1]}</p>
          `;


          /////////////   شرح
          for (let j = 0; j < level2data.length;) {

            //@ts-ignore
            let level3data = level2data.filter(a => a.accountHeadCode == level2data[j]?.accountHeadCode);

            printContents += `
            <p style="font-size: 12;max-height: 40px;height: 40px;">${level2data[j].accountHeadCode.split('-')[1]}</p>
        `
            j += level3data.length;
            for (let k = 0; k < level3data.length; k++) {

              printContents += `
              <p style="font-size: 9px;max-height: 40px;height: 40px;">${level3data[k].accountReferenceCode.split('-')[1]}
              <br>${level3data[k].voucherRowDescription}
              </p>
              `
            }

          }
          printContents += `
          </td>
          <td  style="direction: rtl;text-align: center;">
          <p style="font-size: 14px;max-height: 40px;height: 40px;"> &nbsp; </p>
          `;

          //////////////////    مبلغ جزء
          let templevel2 = new AccountHeadPrintModel();
          for (let j = 0; j < level2data.length;) {
            let templevel3 = new level3Model();
            //@ts-ignore
            let level3data = level2data.filter(a => a.accountHeadCode == level2data[j]?.accountHeadCode);
            j += level3data.length;
            printContents += `
          <p style="font-size: 12;max-height: 40px;height: 40px;"> &nbsp; </p>
          `
            for (let k = 0; k < level3data.length; k++) {

              let templevel4: level4Model = new level4Model();
              if (level3data[k].debit != 0) {
                templevel4.level4debit += level3data[k].debit;
                templevel3.level3debit += level3data[k].debit;
                printContents += `
                <p style="font-size: 9px;max-height: 40px;height: 40px;">${new MoneyPipe().transform(level3data[k].debit)}</p>
                `
              } else {
                templevel4.level4credit += level3data[k].credit;
                templevel3.level3credit += level3data[k].credit;
                printContents += `
                <p style="font-size: 9px;max-height: 40px;height: 40px;">${new MoneyPipe().transform(level3data[k].credit)}</p>
                `
              }
              templevel3.level4.push(templevel4);
            }
            templevel2.level2credit += templevel3.level3credit;
            templevel2.level2debit += templevel3.level3debit;
            templevel2.level3.push(templevel3);
          }


          model.push(templevel2);


          /////////////////بدهکار
          printContents += `
          </td>
          <td  style="direction: rtl;text-align: center;">
          `;
          printContents += `
            <p style="font-size: 14px;max-height: 40px;height: 40px;">&nbsp;</p>

            `;
          for (let j = 0; j < templevel2.level3.length; j++) {
            printContents += `
            <p style="font-size: 12;max-height: 40px;height: 40px;">${templevel2.level3[j].level3debit != 0 ? new MoneyPipe().transform(templevel2.level3[j].level3debit) : '&nbsp;'}</p>

            `;

            for (let k = 0; k < templevel2.level3[j].level4.length; k++) {
              printContents += `
            <p style="font-size: 9px;max-height: 40px;height: 40px;">&nbsp;</p>
            `;
            }
          }


          ///////////////// بستانکار
          printContents += `
          </td>
          <td  style="direction: rtl;text-align: center;">
          `;
          printContents += `
            <p style="font-size: 14px;max-height: 40px;height: 40px;">&nbsp;</p>

            `;
          for (let j = 0; j < templevel2.level3.length; j++) {
            printContents += `
            <p style="font-size: 12;max-height: 40px;height: 40px;">${templevel2.level3[j].level3credit != 0 ? new MoneyPipe().transform(templevel2.level3[j].level3credit) : '&nbsp;'}</p>

            `;

            for (let k = 0; k < templevel2.level3[j].level4.length; k++) {
              printContents += `
            <p style="font-size: 9px;max-height: 40px;height: 40px;">&nbsp;</p>

            `;
            }
          }

          printContents += `
          </td>
          </tr>`;
        }


        for (let i = 0; i < result.creditOrders.length; i++) {
          let data = result.creditOrders[i];
          index++;
          ///////////////////////// کد حساب

          printContents += `
<tr style="direction: rtl;text-align: center;">
<td  style="direction: rtl;text-align: center;font-size:12px;">${index}</td>
<td  style="direction: rtl;text-align: center;">
<p style="font-size: 14px;max-height: 40px;height: 40px;"> ${data.split('-')[0]}</p>
`;
          //@ts-ignore
          let level2data = result.creditDatas.filter(a => a.level2AccountHead == data).sort(a => a.accountHeadCode);


          for (let j = 0; j < level2data.length;) {

            //@ts-ignore
            let level3data = level2data.filter(a => a.accountHeadCode == level2data[j]?.accountHeadCode);

            printContents += `
  <p style="font-size: 12;max-height: 40px;height: 40px;">${level2data[j].accountHeadCode.split('-')[0]}</p>`
            j += level3data.length;
            for (let k = 0; k < level3data.length; k++) {

              printContents += `
    <p style="font-size: 9px;max-height: 40px;height: 40px;">${level3data[k].accountReferenceCode.split('-')[0]}</p>`
            }

          }


          printContents += `</td>
          <td  style="direction: rtl;text-align: right;">
          <p style="font-size: 14px;max-height: 40px;height: 40px;"> ${data.split('-')[1]}</p>
          `;


          /////////////   شرح
          for (let j = 0; j < level2data.length;) {

            //@ts-ignore
            let level3data = level2data.filter(a => a.accountHeadCode == level2data[j]?.accountHeadCode);

            printContents += `
            <p style="font-size: 12;max-height: 40px;height: 40px;">${level2data[j].accountHeadCode.split('-')[1]}</p>
        `
            j += level3data.length;
            for (let k = 0; k < level3data.length; k++) {

              printContents += `
              <p style="font-size: 9px;max-height: 40px;height: 40px;">${level3data[k].accountReferenceCode.split('-')[1]}
              <br>${level3data[k].voucherRowDescription}
              </p>
              `
            }

          }
          printContents += `
          </td>
          <td  style="direction: rtl;text-align: center;">
          <p style="font-size: 14px;max-height: 40px;height: 40px;"> &nbsp; </p>
          `;

          //////////////////    مبلغ جزء
          let templevel2 = new AccountHeadPrintModel();
          for (let j = 0; j < level2data.length;) {
            let templevel3 = new level3Model();
            //@ts-ignore
            let level3data = level2data.filter(a => a.accountHeadCode == level2data[j]?.accountHeadCode);
            j += level3data.length;
            printContents += `
          <p style="font-size: 14;max-height: 40px;height: 40px;"> &nbsp; </p>
          `
            for (let k = 0; k < level3data.length; k++) {

              let templevel4: level4Model = new level4Model();
              if (level3data[k].debit != 0) {
                templevel4.level4debit += level3data[k].debit;
                templevel3.level3debit += level3data[k].debit;
                printContents += `
                <p style="font-size: 9px;max-height: 40px;height: 40px;">${new MoneyPipe().transform(level3data[k].debit)}</p>
                `
              } else {
                templevel4.level4credit += level3data[k].credit;
                templevel3.level3credit += level3data[k].credit;
                printContents += `
                <p style="font-size: 9px;max-height: 40px;height: 40px;">${new MoneyPipe().transform(level3data[k].credit)}</p>
                `
              }
              templevel3.level4.push(templevel4);
            }
            templevel2.level2credit += templevel3.level3credit;
            templevel2.level2debit += templevel3.level3debit;
            templevel2.level3.push(templevel3);
          }


          model.push(templevel2);


          /////////////////بدهکار
          printContents += `
          </td>
          <td  style="direction: rtl;text-align: center;">
          `;
          printContents += `
            <p style="font-size: 14px;max-height: 40px;height: 40px;">&nbsp;</p>

            `;
          for (let j = 0; j < templevel2.level3.length; j++) {
            printContents += `
            <p style="font-size: 12;max-height: 40px;height: 40px;">${templevel2.level3[j].level3debit != 0 ? new MoneyPipe().transform(templevel2.level3[j].level3debit) : '&nbsp;'}</p>

            `;

            for (let k = 0; k < templevel2.level3[j].level4.length; k++) {
              printContents += `
            <p style="font-size: 9px;max-height: 40px;height: 40px;">&nbsp;</p>
            `;
            }
          }


          ///////////////// بستانکار
          printContents += `
          </td>
          <td  style="direction: rtl;text-align: center;">
          `;
          printContents += `
            <p style="font-size: 14px;max-height: 40px;height: 40px;">&nbsp;</p>

            `;
          for (let j = 0; j < templevel2.level3.length; j++) {
            printContents += `
            <p style="font-size: 12;max-height: 40px;height: 40px;">${templevel2.level3[j].level3credit != 0 ? new MoneyPipe().transform(templevel2.level3[j].level3credit) : '&nbsp;'}</p>

            `;

            for (let k = 0; k < templevel2.level3[j].level4.length; k++) {
              printContents += `
            <p style="font-size: 9px;max-height: 40px;height: 40px;">&nbsp;</p>

            `;
            }
          }

          printContents += `
          </td>
          </tr>`;
        }

        for (let i = 0; i < model.length; i++) {
          allDebit += model[i].level2debit;
          allCredit += model[i].level2credit;
        }

        printContents += `
          <tr style="direction: rtl;text-align: center;background-color: #e7e7eb;">
          <td style="border-top: none; border: 1px solid black"></td>
          <td style="border-top: none; border: 1px solid black"></td>
          <td style="border-top: none;border: 1px solid black;font-size: 14px;">جمع کل</td>
          <td style="border-top: none;border: 1px solid black;font-size: 14px;"></td>
          <td style="border-top: none;border-bottom: 1px solid black;font-size: 14px;">${new MoneyPipe().transform(allDebit)}</td>
          <td style="border-top: none;border-bottom: 1px solid black;font-size: 14px;">${new MoneyPipe().transform(allCredit)}</td>
          </tr>
          </tbody></table>
          `;
        //@ts-ignore
        printContents += `<div style="width: 99%;float: right;font-size: 14px;margin-bottom: 10px;padding: 6px;margin-right: 2px; text-align:right;border-bottom: 1px solid #000000">شرح سند: ${result.creditDatas.find(oo => oo.voucherDescription != undefined)?.voucherDescription}
        </div>
          <div style="text-align: center;padding: 30px;width:100%">
              <div style="width:25%;float: left;">
              مدیر مالی:
              </div>
              <div style="width:25%;float: left;">
              رئیس حسابداری:
              </div>
              <div style="width:25%;float: left;">
              رسیدگی کننده:
              </div>
              <div style="width:25%;float: left;">
              تنظیم کننده: ${this.form.get('creator')?.value}
              </div>
          </div>`;
      }

      this.Service.onPrint(printContents, "")
    }
  }

}
