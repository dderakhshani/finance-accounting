import {
  AfterViewInit,
  Component,
  EventEmitter, HostListener,
  Input,
  NgModule,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  ViewChild
} from "@angular/core";
import {AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators} from "@angular/forms";
import {CommandBase} from "../services/mediator/command-base";
import {ValidationRule} from "../validation/validation-rule";
import {FormAction} from "../models/form-action";
import {PageModes} from "../enums/page-modes";
import {CustomValidators} from "../validation/custom-validators";
import {IRequest} from "../services/mediator/interfaces";
import {ActivatedRoute, Params, Router} from "@angular/router";
import {ApplicationError} from "../exceptions/application-error";
import {ActionBarComponent, Default_Actions} from "../components/custom/action-bar/action-bar.component";
import {TabManagerService} from "../../layouts/main-container/tab-manager.service";
import {ServiceLocator} from "../services/service-locator/service-locator";
import {PagesCommonService} from "../../shared/services/pages/pages-common.service";
import {tsCastToAny} from "@angular/compiler-cli/src/ngtsc/typecheck/src/ts_util";
import {Subscription} from "rxjs";

@Component({
  template: '',
  styleUrls: ['./base.component.scss']

})
export abstract class BaseComponent implements OnInit, AfterViewInit, OnDestroy {

  public componentId!: string;
  public tabManagerService!: TabManagerService
  private PagesCommonService!: PagesCommonService;

  public windowEventListeners: any[] = [];
  marginFromBottom_appDynamicTableHeight : number = 30;

  protected constructor(
    private _ROUTE?: ActivatedRoute,
    private _ROUTER?: Router
  ) {
    if (!this.pageMode) {
      this.pageMode = PageModes.Add;
    }
    this.tabManagerService = ServiceLocator.injector.get(TabManagerService)
    this.PagesCommonService = ServiceLocator.injector.get(PagesCommonService)
    this._ROUTE = ServiceLocator.injector.get(ActivatedRoute)
    this._ROUTER = ServiceLocator.injector.get(Router)
    this.componentId = this.tabManagerService.activeTab.guid
  }
  @ViewChild(ActionBarComponent) actionBar!: ActionBarComponent;



  @Input() pageMode!: PageModes.Add | PageModes.Update | PageModes.Read | PageModes.Delete;
  @Input() form!: any;
  @Input() entity!: any;
  @Input() entities: any[] = [];
  @Output() formReady: EventEmitter<any> = new EventEmitter<any>();

  protected _request!: IRequest<any, any>;
  @Input() shouldCreateFormFromRequest: boolean = true;

  @Input() set request(request: IRequest<any, any>) {
    this._request = request;
    if (this.shouldCreateFormFromRequest) this.form = this.createForm(this._request, true);
  };

  get request(): IRequest<any, any> {
    return this._request
  }

  formActions!: FormAction[];
  pageIndex: number = 1;
  pageSize: number = 10;
  totalItems: number = 0;

  private intervalId: any;
  counter: number = 0;

  private _isLoading: boolean = false;
  public loadingFinished:EventEmitter<boolean> = new EventEmitter<boolean>();
  public loadingSubscription!:Subscription;
  toggleIsLoading() {
    this.isLoading = !this.isLoading
  }

  set isLoading(value: boolean) {
    this._isLoading = value
    this.tabManagerService.toggleCurrentTabLoading(value)
    if (!this._isLoading) {
      let that = this;
      setTimeout(() => {
        that.loadingFinished.emit(true)
      })
    }
  }

  get isLoading() {
    return this._isLoading
  }

  isSaving: boolean = false;

  abstract ngOnInit(params?: any): void

  ngOnDestroy(): void {
    if(this.loadingSubscription) this.loadingSubscription.unsubscribe()

    this.windowEventListeners.forEach((x: any) => {
      x();
    })
  }

  ngAfterViewInit() {
    if (this.actionBar && this.actionBar.actions.length === 0)
      this.actionBar.actions = Default_Actions;

  }

  abstract resolve(params?: any): any;

  abstract initialize(params?: any): any;

  abstract add(param?: any): any;

  abstract get(param?: any): any;

  abstract update(param?: any): any;

  abstract delete(param?: any): any;

  async submit() {
    if (this.pageMode === PageModes.Add) {
      return await this.add().then(() => {
        return this.pageMode = PageModes.Update;
      });
    }
    if (this.pageMode === PageModes.Update) {
      return await this.update();
    }
  }

  //===============================================================
  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    // this.getTableTopViewport();

  }

  getTableTopViewport() {
    let html: any = document.getElementsByClassName('current-tab');

    let topViewport: any;
    let bottomViewport: any;
    let heightTable: any;

    for (let i = 0; i < html.length; i++) {
      const slide = html[i] as HTMLElement;
      let table = slide?.getElementsByTagName('div');
      for (let j = table.length - 1; j >= 0; j--) {

        const t = table[j] as HTMLElement;

        if (t.className == 'tableFixHead') {
          setTimeout(() => {

            console.log('t.getBoundingClientRect()top : ' + t.getBoundingClientRect().top)
            const windowHeight = window.innerHeight;
            const rect = t.getBoundingClientRect();
            topViewport = rect.top
            bottomViewport = rect.bottom
            heightTable = windowHeight - 30 - topViewport;
            heightTable = heightTable < 150 ? 300 : heightTable;
            t.style.maxHeight = `${heightTable}px`;
          }, 1);  // اجرای کد پس از یک تاخیر کوتاه برای اطمینان از رندر DOM
        }


      }
    }

  }

  //--------------------*******************************************
  //===============================================================

  reset(): any {
    this.resetForm(this.form);
    this.pageMode = PageModes.Add;
    this.initialize()
  };

  abstract close(): any;

  public createForm(entity: any | CommandBase, bindEntityToForm: boolean = false, validations?: ValidationRule): AbstractControl {
    if (Array.isArray(entity))//----------------------------If entity is Array
    {
      let controls: any[] = [];

      if (entity?.length > 0) {
        entity.forEach(e => {
          controls.push(this.createForm(e));
        });
      }
      let formArray = new FormArray(controls);
      if (bindEntityToForm) {
        this.bindEntitiesToFormArray(entity, formArray)
      }
      return formArray;
    } else if (entity && typeof entity === 'object' && typeof entity?.getMonth !== "function") {//--------------If entity is Object

      let controls: any = {};
      Object.keys(entity).forEach(key => {
        controls[key] = this.createForm(
          entity[key],
          false,
          // @ts-ignore
          entity?.validationRules?.find(x => x.title.toLowerCase() === key?.toLowerCase())
        );
      });

      let descriptors = Object.getOwnPropertyDescriptors(Object.getPrototypeOf(entity));
      Object.keys(descriptors).forEach(key => {
        if (typeof descriptors[key]?.get === "function" && key !== '__proto__' && !key.includes('_')) {
          if (key !== 'url' && key !== 'validationRules' && key !== 'mapFrom' && key !== 'mapTo') {
            controls[key] = this.createForm(entity[key]);
          }
        }
      });

      let formGroup = new FormGroup(controls);
      if (bindEntityToForm) {
        this.bindEntityToFormGroup(entity, formGroup)
      }
      return formGroup;

    } else {//----------------------------------------------Else entity is value
      return new FormControl(entity, CustomValidators.getValidators(validations));
    }
  }

  public updateForm(entity: any, form: AbstractControl) {
    if (Array.isArray(entity)) {//----------------------------If entity is Array
      // TODO update Form Array items Values instead of replacing them
      if (entity?.length > 0) {
        let formArr = form as FormArray;
        this.clearFormArray(formArr);
        entity.forEach(e => {
          formArr?.push(this.createForm(e));
        });
      }

    } else if (entity && typeof entity === 'object' && typeof entity?.getMonth !== "function") {//--------------If entity is Object
      Object.keys(entity).forEach(key => {
        this.updateForm(entity[key], <AbstractControl>(form).get(key));
      });
    } else {//----------------------------------------------Else entity is value
      (form as FormControl)?.setValue(entity);
    }
  }

  public bindEntitiesToFormArray(entity: any[], form: FormArray) {
    form.valueChanges.subscribe((newValue: any[]) => {
      entity.splice(0, entity.length);
      entity.push(...newValue);
    })
  }

  public bindEntityToFormGroup(entity: any, form: FormGroup) {
    form.valueChanges.subscribe(() => {
      let newFormValue = form.getRawValue();
      Object.keys(newFormValue).forEach(key => {

        try {
          entity[key] = newFormValue[key] ?? undefined
        } catch (e) {

        }

      });
    })


    // Object.keys(form.controls).forEach(key => {
    //   form.controls[key].valueChanges.subscribe(
    //     (newValue) => {
    //       // if its not a getter setter
    //       try {
    //         entity[key] = newValue ?? undefined
    //       } catch (e) {
    //
    //       }
    //     }
    //   )
    // });
  }

  public addEntityToFormArray(entity: any, form: FormArray) {
    form.push(this.createForm(entity));
  }

  public clearFormArray(form: FormArray): void {
    while (form?.controls.length > 0) {
      form.removeAt(0);
    }
  }

  public resetForm(form: AbstractControl) {
    // @ts-ignore
    if (typeof form.push === "function" && Array.isArray(form.controls)) {//-------------- If Form Array
      this.clearFormArray(form as FormArray);
      (form as FormArray).reset();
      (form as FormControl).setErrors(null);
      // @ts-ignore
    } else if (typeof form.push !== "function" && form.controls && !Array.isArray(form.controls)) {//-------- If Form Group
      let formKeys: string[] = Object.keys((form as FormGroup).controls);
      if (formKeys) {
        formKeys.forEach(key => {
          this.resetForm((form as FormGroup).controls[key]);
        })
      }
      (form as FormGroup).reset();
      (form as FormControl).setErrors(null);
    } else {//-------------------------------------------- If Form Control
      (form as FormControl).reset();
      (form as FormControl).markAsPristine();
      (form as FormControl).markAsUntouched();
      (form as FormControl).setErrors(null);
    }
  }

  public async addQueryParam(param: string, value: any) {
    if (!this._ROUTE || !this._ROUTER)
      throw new ApplicationError(BaseComponent.name, this.addQueryParam.name, 'Pass in Router and ActivatedRoute to super class constructor');

    let params = {...this._ROUTE.snapshot.queryParams};
    params[param] = value;
    await this._ROUTER.navigate([], {
      relativeTo: this._ROUTE,
      queryParams: params,
      queryParamsHandling: "merge"
    });

  }

  public async deleteQueryParam(param: string) {
    if (!this._ROUTE || !this._ROUTER)
      throw new ApplicationError(BaseComponent.name, this.addQueryParam.name, 'Pass in Router and ActivatedRoute to super class constructor');
    let baseUrl = 'http://localhost';
    // @ts-ignore
    let url = new URL(baseUrl + this._ROUTE.snapshot._routerState.url);
    url.searchParams.delete(param)
    await this._ROUTER.navigateByUrl(url.pathname + url.search);
  }

  public getQueryParam(param: string) {
    if (!this._ROUTE)
      throw new ApplicationError(BaseComponent.name, this.addQueryParam.name, 'Pass in ActivatedRoute to super class constructor');
    return this._ROUTE.snapshot.queryParams[param] != 'undefined' && this._ROUTE.snapshot.queryParams[param] != 'null' ? this._ROUTE.snapshot.queryParams[param] : undefined
  }

  public onDeleteFilter(form: any, tableConfigurations: any) {
    tableConfigurations.filters = []
    tableConfigurations.sortKeys = []

    tableConfigurations.columns.filter((x: any) => x.isFiltered).forEach((col: any) => {
      col.isFiltered = false;
      col.isSorted = false;
    })

    tableConfigurations.pagination.pageIndex = 0;
    form.reset();

    form.controls?.toDate.setValue(new Date(this.PagesCommonService.identityService.getActiveYearlastDate()))
    form.controls?.fromDate.setValue(new Date(this.PagesCommonService.identityService.getActiveYearStartDate()))
    this.get();
  }


}
