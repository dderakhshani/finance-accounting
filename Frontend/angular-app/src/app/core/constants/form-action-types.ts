import {FormAction} from "../models/form-action";

export class FormActionTypes {


  public static get save(): FormAction {
    return {
      id: '#Save',
      title: 'ثبت',
      disabled: false,
      imageUrl: '/assets/icons/save.svg',
      fontTitle:'save',
      sortIndex: 2,
      color: 'primary'
    }
  }

  public static get saveandexit(): FormAction {
    return {
      id: '#saveandexit',
      title: 'ثبت و خروج',
      disabled: true,
      imageUrl: '/assets/icons/saveandexit.svg',
      fontTitle:'',
      sortIndex: 3,
      color: 'primary'
    }
  }

  public static get edit(): FormAction {
    return {
      id: '#edit',
      title: 'ویرایش',
      disabled: false,
      imageUrl: '/assets/icons/edit.svg',
      fontTitle:'edit',
      sortIndex: 4,
      color: 'primary'
    }
  }

  public static get add(): FormAction {
    return {
      id: '#Add',
      title: 'جدید',
      disabled: false,
      imageUrl: '/assets/icons/add.svg',
      fontTitle:'add',
      sortIndex: 5,
      color: 'primary'
    }
  }

  public static get refresh(): FormAction {
    return {
      id: '#Refresh',
      title: 'بروز رسانی',
      disabled: false,
      imageUrl: '/assets/icons/refresh.svg',
      fontTitle:'refresh',
      sortIndex: 6,
      color: 'primary'
    }
  }

  public static get list(): FormAction {
    return {
      id: '#list',
      title: 'نمایش فهرست',
      disabled: false,
      imageUrl: '/assets/icons/list.svg',
      fontTitle:'list',
      sortIndex: 7,
      color: 'primary'
    }
  }

  public static get delete(): FormAction {
    return {
      id: '#delete',
      title: 'حذف',
      disabled: false,
      imageUrl: '/assets/icons/delete.svg',
      fontTitle:'delete',
      sortIndex: 8,
      color: 'primary'
    }
  }
  public static get history(): FormAction {
    return {
      id: '#history',
      title: 'سابقه',
      disabled: false,
      imageUrl: '/assets/icons/delete.svg',
      fontTitle:'history',
      sortIndex: 9,
      color: 'primary'
    }
  }

  public static get print(): FormAction {
    return {
      id: '#print',
      title: 'چاپ',
      disabled: false,
      imageUrl: '/assets/icons/print.svg',
      fontTitle:'print',
      sortIndex: 10,
      color: 'primary'
    }
  }

}
