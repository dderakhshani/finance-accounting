export class NavigationItem {
  id?: number;
  parentId?: number;
  title!: string;
  route!: string;
  showChildren: boolean = false;
  permissionId!:number;
  children: NavigationItem[] = []
  imageUrl!:string
  queryParameterMappings!:string
}
