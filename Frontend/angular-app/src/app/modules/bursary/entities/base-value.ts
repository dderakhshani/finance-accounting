export class BaseValue {
  public id!: number;
  public parentId!: number;
  public baseValueTypeId!: number;
  public levelCode!: string;
  public code!: string;
  public title!: string;
  public value!: string;
  public uniqueName!: string;
  public groupName!: string;
  public subSystem!: string;
  public isReadOnly!: boolean;
  public orderIndex!: number;
  public children!: BaseValue[];
}
