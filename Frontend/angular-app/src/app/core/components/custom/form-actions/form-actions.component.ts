import {Component, EventEmitter, HostListener, Input, OnInit, Output, ViewEncapsulation} from '@angular/core';
import {FormAction} from "../../../models/form-action";
import {FormActionTypes} from "../../../constants/form-action-types";
import {ExtensionsService} from "../../../../shared/services/extensions/extensions.service";

@Component({
  selector: 'app-form-actions',
  templateUrl: './form-actions.component.html',
  styleUrls: ['./form-actions.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FormActionsComponent implements OnInit {

  @Input() actions:FormAction[] = [];

  @Output() OnAdd:EventEmitter<any> = new EventEmitter<any>();
  @Output() OnSave:EventEmitter<any> = new EventEmitter<any>();
  @Output() OnSaveAndExit:EventEmitter<any> = new EventEmitter<any>();
  @Output() OnEdit:EventEmitter<any> = new EventEmitter<any>();
  @Output() OnRefresh:EventEmitter<any> = new EventEmitter<any>();
  @Output() OnDelete:EventEmitter<any> = new EventEmitter<any>();
  @Output() OnShowList:EventEmitter<any> = new EventEmitter<any>();
  @Output() OnHistory:EventEmitter<any> = new EventEmitter<any>();


  constructor(
    private extensionService:ExtensionsService
  ) { }

  ngOnInit(): void {
    this.actions = this.extensionService.sortByKey(this.actions, 'sortIndex')
  }

  clickHandler(formAction:FormAction) {

    if (formAction.id === FormActionTypes.add.id) {
      this.OnAdd.emit();
    }
    if (formAction.id === FormActionTypes.save.id) {
      this.OnSave.emit();
    }
    if (formAction.id === FormActionTypes.saveandexit.id) {
      this.OnSaveAndExit.emit();
    }
    if (formAction.id === FormActionTypes.edit.id) {
      this.OnEdit.emit();
    }
    if (formAction.id === FormActionTypes.refresh.id) {
      this.OnRefresh.emit();
    }
    if (formAction.id === FormActionTypes.delete.id) {
      this.OnDelete.emit();
    }
    if (formAction.id === FormActionTypes.list.id) {
      this.OnShowList.emit();
    }
    if (formAction.id === FormActionTypes.history.id) {
      this.OnHistory.emit();
    }
  }
  private lastkey = '';

  
}
