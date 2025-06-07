import {AfterViewInit, ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {BehaviorSubject} from "rxjs";
import {ThemePalette} from "@angular/material/core/common-behaviors/color";



@Component({
  selector: 'app-action-bar',
  templateUrl: './action-bar.component.html',
  styleUrls: ['./action-bar.component.scss']
})
export class ActionBarComponent implements OnInit,AfterViewInit {
  primaryActions: BehaviorSubject<Action[]> = new BehaviorSubject<Action[]>([]);
  isOpen = false;


  secondaryActions: BehaviorSubject<Action[]> = new BehaviorSubject<Action[]>([]);
  private _actions: Action[] = []
  @Input() set actions(actions: Action[]) {
    this._actions = actions;
    this.primaryActions.next(actions)
  }
  @Input()  viewMode:'default'| 'slider'   = 'default';

  get actions() {
    return this.primaryActions.value.filter(x => x.show !== false);
  }


  @Output() onActionClicked: EventEmitter<Action> = new EventEmitter<Action>()

  @Output() onAdd: EventEmitter<Action> = new EventEmitter<Action>();
  @Output() onEdit: EventEmitter<Action> = new EventEmitter<Action>();
  @Output() onDelete: EventEmitter<Action> = new EventEmitter<Action>();
  @Output() onRefresh: EventEmitter<Action> = new EventEmitter<Action>();
  @Output() onFilter: EventEmitter<Action> = new EventEmitter<Action>();
  @Output() onPrint: EventEmitter<Action> = new EventEmitter<Action>();
  @Output() onList: EventEmitter<Action> = new EventEmitter<Action>();
  @Output() onSave: EventEmitter<Action> = new EventEmitter<Action>();
  @Output() onSaveAndExit: EventEmitter<Action> = new EventEmitter<Action>();
  @Output() onHistory: EventEmitter<Action> = new EventEmitter<Action>();
  @Output() onCustomClick: EventEmitter<Action> = new EventEmitter<Action>();

  constructor(private cdr: ChangeDetectorRef) {
  }

  ngOnInit(): void {
  }
  toggleSideBar() {
    this.isOpen = !this.isOpen;
  }
  actionClickHandler(action: Action) {
    this.onActionClicked.emit(action);
    if (!action.disabled) {
      switch (action.type) {
        case ActionTypes.add:
          this.onAdd.emit(action)
          break;
        case ActionTypes.edit:
          this.onEdit.emit(action)
          break;
        case ActionTypes.delete:
          this.onDelete.emit(action)
          break;
        case ActionTypes.refresh:
          this.onRefresh.emit(action)
          break;
        case ActionTypes.filter:
          this.onFilter.emit(action)
          break;
        case ActionTypes.print:
          this.onPrint.emit(action)
          break;
        case ActionTypes.list:
          this.onList.emit(action)
          break;
        case ActionTypes.save:
          this.onSave.emit(action)
          break;
        case ActionTypes.saveAndExit:
          this.onSaveAndExit.emit(action)
          break;
        case ActionTypes.history:
          this.onHistory.emit(action)
          break;
        case ActionTypes.custom:
          this.onCustomClick.emit(action)
          break;
      }
    }
}
  ngAfterViewInit(): void {
    this.cdr.detectChanges();
  }
}

export class Action {
  constructor(title: string, icon: string, type: ActionTypes,uniqueName: string | undefined = undefined, color: ThemePalette | 'none' = "primary", show: boolean = true, disabled: boolean = false, customClass: string = '') {
    this.title = title;
    this.icon = icon;
    this.type = type;
    this.uniqueName = uniqueName;
    this.color = color;
    this.show = show;
    this.disabled = disabled;
    this.customClass = customClass;
  }

  public title!: string;
  public icon!: string;
  public show!: boolean;
  public disabled!: boolean;
  public color!: ThemePalette | 'none';
  public customClass!: string;
  public type!: ActionTypes;
  public uniqueName!:string | undefined;


  setTitle(title: string) {
    this.title = title;
    return this;
  }

  setIcon(icon: string) {
    this.icon = icon;
    return this;
  }
 setDisable(disable: boolean) {
    this.disabled = disable;
    return this;
}
setShow(show: boolean) {
this.show = show;
return this;
}
  setColor(color: ThemePalette ) {
    this.color = color;
    return this;
  }

  setCustomClass(className: string) {
    this.customClass = className;
    return this;
  }
}


export enum ActionTypes {
  add,
  edit,
  delete,
  refresh,
  filter,
  print,
  list,
  save,
  saveAndExit,
  history,
  custom
}


export class PreDefinedActions {

  static add = () => new Action('افزودن', 'add', ActionTypes.add, undefined, undefined , true,false,'bg-green-500')
  static edit = () => new Action('ویرایش', 'edit', ActionTypes.edit, undefined, undefined , true,false,'bg-amber-500')
  static delete = () => new Action('حذف', 'delete', ActionTypes.delete , undefined, undefined , true,false,'bg-red-500')
  static refresh = () => new Action('تازه سازی', 'refresh', ActionTypes.refresh)
  static filter = () => new Action('فیلتر', 'filter', ActionTypes.filter)
  static print = () => new Action('چاپ', 'print', ActionTypes.print)
  static list = () => new Action('لیست', 'list', ActionTypes.list)
  static save = () => new Action('ذخیره', 'save', ActionTypes.save)
  static saveAndExit = () => new Action('ذخیره', 'save', ActionTypes.saveAndExit)
  static history = () => new Action('تاریخچه', 'history', ActionTypes.history)
}

export const Default_Actions = [
  PreDefinedActions.add(),
]


