import {Component} from '@angular/core';
import * as customBuild from '../../../../../../assets/CKEditore-5/ckeditor';
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {ActivatedRoute, Router} from "@angular/router";
import {PageModes} from "../../../../../core/enums/page-modes";
import {Help} from "../../../entities/help";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {CreateHelpCommand} from "../../../repositories/help/command/create-help-command";
import {UpdateHelpCommand} from "../../../repositories/help/command/update-help-command";
import {GetHelpQuery} from "../../../repositories/help/queries/get-help-query";
import {GetMenuItemsQuery} from "../../../repositories/menu-item/queries/get-menu-items-query";
import {MenuItem} from "../../../entities/menu-item";

@Component({
  selector: 'app-help-editor',
  templateUrl: './help-editor.component.html',
  styleUrls: ['./help-editor.component.scss']
})
export class HelpEditorComponent extends BaseComponent {
  editorHelpId: number = 0
  public editorContent: any = null;
  public editor = customBuild;
  menuItems: MenuItem[] = [];
  filteredMenuItems: MenuItem[] = [];

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private _mediator: Mediator,
  ) {
    super(route, router)
    let helpId = +this.getQueryParam('id');
    this.editorHelpId = helpId ?? "C" + (Math.random() * 10)
  }
  async ngAfterViewInit() {
    this.isLoading = true

    this.actionBar.actions = [
      PreDefinedActions.save(),
      PreDefinedActions.add(),
      PreDefinedActions.list().setTitle("لیست راهنما")
    ];
    await this.resolve()
  }
  ngOnInit() {
  }
  async resolve() {
    await this._mediator.send(new GetMenuItemsQuery(0, 0)).then(res => {
      this.menuItems = res.data.filter(x => !res.data.map(x => x.parentId).includes(x.id));
      this.filteredMenuItems = this.menuItems;
    }).then(async () => {
      await this.initialize()
    })
  }

  async initialize(entity?: Help) {
    if (entity || this.getQueryParam('id')) {
      this.pageMode = PageModes.Update;
      if (!entity) entity = await this.get(this.getQueryParam('id'));
      this.request = new UpdateHelpCommand().mapFrom(entity);
      this.editorContent = entity.contents;
    } else {
      this.pageMode = PageModes.Add;
      this.request = new CreateHelpCommand()
    }
    this.form.controls['menuItemId'].valueChanges.subscribe((y: string) => {
      this.filteredMenuItems = this.menuItems.filter(x => x.title.includes(y || ''))
    });
    this.isLoading = false
  }

  menuItemDisplayFn(id: number) {
    return this.menuItems.find(x => x.id === id)?.title;
  }

  async submit() {
    if (this.pageMode === PageModes.Add) await this.add()
    if (this.pageMode === PageModes.Update) await this.update()
  }

  async add() {
    this.form.controls['contents'].setValue(this.editorContent);
    await this._mediator.send(<CreateHelpCommand>this.request)
  }

  async update() {
    this.form.controls['contents'].setValue(this.editorContent);
    await this._mediator.send(<UpdateHelpCommand>this.request)
  }

  async get(id: number) {
    return await this._mediator.send(new GetHelpQuery(id))
  }

  async navigateToHelpsList() {
    await this.router.navigateByUrl('/admin/helps/list')
  }
  override reset() {
    super.reset();
    this.deleteQueryParam("id");
    this.editorContent = ''
  }
  close(): any {
  }
  delete(param?: any): any {
  }
}
