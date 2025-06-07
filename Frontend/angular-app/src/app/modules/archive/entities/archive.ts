import {Attachment} from "./attachment";

export class Archive {
  public id!: number;
  public baseValueTypeId!: number;
  public baseValueTypeTitle!: string;
  public typeBaseId!: number;
  public typeBaseTitle!: string;
  public fileNumber!: string;
  public title!: string;
  public description!: string;
  public keyWords!: string;
  public attachments: Attachment[] = [];
}
