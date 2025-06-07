import {Component} from '@angular/core';
import {BaseComponent} from "../../../../core/abstraction/base.component";
import {CommodityCategory} from "../../entities/commodity-category";
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {PageModes} from "../../../../core/enums/page-modes";
import {
  GetCommodityCategoriesQuery
} from "../../repositories/commodity-category/queries/get-commodity-categories-query";
import {CommodityCategoryDialogComponent} from "./commodity-category-dialog/commodity-category-dialog.component";
import {PreDefinedActions} from "../../../../core/components/custom/action-bar/action-bar.component";
import {log} from "echarts/types/src/util/log";
import {Commodity} from "../../entities/commodity";
import {Router} from "@angular/router";

@Component({
  selector: 'app-commodity-categories',
  templateUrl: './commodity-categories.component.html',
  styleUrls: ['./commodity-categories.component.scss']
})
export class CommodityCategoriesComponent extends BaseComponent {

  commodityCategories!: CommodityCategory[]
  commodityCategoryId!: number;
  hasCommodityChildren: boolean = false;



  constructor(
    private _mediator: Mediator,
    private router: Router,
    public dialog: MatDialog
  ) {
    super();
  }

  async ngOnInit() {
    await this.resolve();
  }

  async ngAfterViewInit() {

  }

  async resolve() {
    await this.get()
  }

  async get() {
    await this._mediator.send(new GetCommodityCategoriesQuery(0, 0, undefined, "id ASC")).then(res => {
      this.commodityCategories = res;

    })
  }

  add(commodityCategory: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      commodityCategory: commodityCategory,
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(CommodityCategoryDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response, pageMode}) => {
      if (response && pageMode === PageModes.Add) {
        this.commodityCategories.push(response)
        this.commodityCategories = [...this.commodityCategories]
      }
    })
  }

  update(commodityCategory: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      commodityCategory: commodityCategory,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(CommodityCategoryDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response, pageMode}) => {
      if (response) {
        if (pageMode === PageModes.Update) {
          let commodityCategoryToUpdate = this.commodityCategories.find(x => x.id === response.id)
          if (commodityCategoryToUpdate) {
            Object.keys(response).forEach((key: string) => {
              // @ts-ignore
              commodityCategoryToUpdate[key] = response[key]
            })
          }
        } else if (pageMode === PageModes.Delete) {
          let commodityCategoryToRemove = this.commodityCategories.find(x => x.id === response.id)
          if (commodityCategoryToRemove) {
            this.commodityCategories.splice(this.commodityCategories.indexOf(commodityCategoryToRemove), 1)
            this.commodityCategories = [...this.commodityCategories]
          }
        }
      }
    })
  }

  handleCommodityCategory(commodityCategory: CommodityCategory) {


    setTimeout(
      () => {
        this.commodityCategoryId = commodityCategory.id;
        console.log('commodityCategory id : ', this.commodityCategoryId)
        let children = commodityCategory.children;
        this.hasCommodityChildren = children.length === 0;
        if(this.hasCommodityChildren) {
         this.preDefinedActionsBar()
        }

      }, 0
    )



  }
  preDefinedActionsBar() {
    setTimeout(() => {
      if (this.actionBar) {
        this.actionBar.actions = [
          PreDefinedActions.add() ,
        ];
      }
    }, 0);
  }
  async navigateToCommodity() {
    await this.router.navigateByUrl(`commodity/add?commodityCategoryId=${this.commodityCategoryId }&pageMode=${'Add'}`)
  }
  initialize(params?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }


}
