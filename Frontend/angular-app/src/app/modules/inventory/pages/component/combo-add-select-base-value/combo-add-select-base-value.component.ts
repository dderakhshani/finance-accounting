
import { EventEmitter, OnChanges, Output, ViewChild } from '@angular/core';
import { Component, Input } from '@angular/core';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { PageModes } from '../../../../../core/enums/page-modes';
import { BaseValueType } from '../../../../admin/entities/base-value-type';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { CodeRowDescriptionDialogComponent } from '../../../../accounting/pages/base-values/code-row-description/code-row-description-dialog/code-row-description-dialog.component';
import { CodeRowDescription } from '../../../../accounting/entities/code-row-description';
import { GetCodeRowDescriptionsQuery } from '../../../../accounting/repositories/code-row-description/queries/code-row-descriptions-query';
import { MatMenuTrigger } from '@angular/material/menu';


@Component({
  selector: 'app-combo-add-select-base-value',
  templateUrl: './combo-add-select-base-value.component.html',
  styleUrls: ['./combo-add-select-base-value.component.scss']
})
export class ComboAddSelectBaseValueComponent implements OnChanges {
 
  @ViewChild(MatMenuTrigger) trigger: MatMenuTrigger | undefined;
  openMenu() {
    this.trigger?.openMenu();
  }

  @Input() isDisable: boolean = false;
  @Input() lablelTitleCombo: string = "";

  @Output() SelectedValue = new EventEmitter<string>();
  @Input() tabindex: number = 1;
  @Input() title: any = "";

  /*searchTerm: string = "";*/
  activeBaseValueType!: BaseValueType;
  entities: CodeRowDescription[] = [];
  entities_filter: CodeRowDescription[] = [];
  constructor(
    public dialog: MatDialog,
    private _mediator: Mediator,
    private Service: PagesCommonService,
    public _notificationService: NotificationService,

  ) {

  }
 
  async ngOnChanges() {

    

  }

  async ngOnInit() {

    await this.resolve();
  }

  async resolve() {

    await this.initialize()
  }
  async initialize() {
    
    await this.ReferenceFilter()

   
  }
  //اگر از لیست طریق انتخاب شود.
  onSelectNode(id: any) {
    var title = this.entities.find(a => a.id == id)?.title
    
    this.title += title != undefined && title != null ? title : '';

    this.SelectedValue.emit(this.title.replace('null', ''));
  }
 

  onSearchTerm() {

    
    if (this.title) {

      this.entities = this.entities_filter.filter(x =>
        x.title.toString().includes(this.title)
      )
    } else {
      this.entities = [...this.entities_filter]

    }
    this.SelectedValue.emit(this.title);
  }
  async ReferenceFilter() {
    let searchQueries: SearchQuery[] = []

    let request = new GetCodeRowDescriptionsQuery(0, 0, searchQueries, '')
    let response = await this._mediator.send(request);
    this.entities = response.data;
    this.entities_filter = response.data;

   
  }
  async add() {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(CodeRowDescriptionDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      if (response && pageMode === PageModes.Add) {
        this.entities.push(response)
        this.entities = [...this.entities]
        this.entities_filter.push(response);
        this.entities_filter = [...this.entities_filter]

      }
    })
  }

}



