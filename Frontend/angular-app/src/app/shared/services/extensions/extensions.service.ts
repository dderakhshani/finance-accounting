import { Injectable } from '@angular/core';
import * as moment from "jalali-moment";
import {TreeData} from "mat-tree-select-input";

@Injectable({
  providedIn: 'root'
})
export class ExtensionsService {

  constructor() { }


  newGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  listToTree<T>(list : T[], childrenKey: string, idKey: string , parentKey: string) {
    let roots = [];

    // @ts-ignore
    roots = list.filter(x => !x[parentKey]);

    roots.forEach(rootNode => {
      // @ts-ignore
      this.setChildren(list, rootNode, childrenKey,idKey,parentKey);
    });
    return roots;
  }

  private setChildren(list:[],node:any,childrenKey: string, idKey: string , parentKey: string) {
    node[childrenKey] = [];

    let possibleChildren = list.filter(x =>  x[parentKey] === node[idKey]);

    if (possibleChildren && possibleChildren?.length > 0) {
      node[childrenKey] = possibleChildren;
      // @ts-ignore
      node[childrenKey]?.forEach(child => {
        this.setChildren(list,child,childrenKey, idKey, parentKey)
      })
    }
  }

  toPersianDate(date:string | Date, includeHoursAndSeconds: boolean = false) {
    if (date) {
      return moment(date).format((includeHoursAndSeconds ? 'HH:mm'  : '') + ' jYYYY/jMM/jDD');
    } else {
      return ''
    }
  }

  sortByKey(array: any[], key:string) {
    if (array?.length) {
      return array.sort(function(a, b) {
        let x = a[key]; let y = b[key];
        return ((x < y) ? -1 : ((x > y) ? 1 : 0));
      });
    } else {
      return array
    }
  }



  propName = ( obj : any ) => new Proxy(obj, {
    get(_, key) {
      return key;
    }
  });


  ///Construct Our Data to TreeData For SelectTree
  constructTreeData(data:any):TreeData[]{
    return data.map(
      (item:any)=> {
        let o = {
          name: item.title,
          children: (item.children.length) ? this.constructTreeData(item.children) : [],
          value: item.id.toString()
        }
        return o
      })
  }


}
