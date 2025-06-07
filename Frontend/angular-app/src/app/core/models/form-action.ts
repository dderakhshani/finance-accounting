import {ThemePalette} from "@angular/material/core/common-behaviors/color";

export class FormAction {
  public id!:string;
  public title!:string;
  public disabled:boolean = false;
  public imageUrl!:string;
  public fontTitle!:string;
  public sortIndex!:number;
  public color!:ThemePalette;

}

