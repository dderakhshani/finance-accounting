import { Injectable } from '@angular/core';
import {ToastrConfig} from "./toastr-config";
import {ToastrService} from "ngx-toastr";


@Injectable({
  providedIn: 'root'
})
export class Toastr_Service {

  constructor(
    private toastr: ToastrService
  ) { }
  showToast(config: ToastrConfig): void {
    const { title, message, type = 'info', position = 'top-right', options } = config;
    const positionClassMap = {
      'top-left': 'toast-top-left',
      'top-center': 'toast-top-center',
      'top-right': 'toast-top-right',
      'bottom-left': 'toast-bottom-left',
      'bottom-center': 'toast-bottom-center',
      'bottom-right': 'toast-bottom-right',
    };
    const positionClass = positionClassMap[position];
    const defaultOptions = {
      positionClass,
      disableTimeOut: false ,
      newestOnTop : true,
      progressBar: true,
      closeButton: true,
      ...options,
    };
    switch (type) {
      case 'success':
        this.toastr.success(message, title, defaultOptions);
        break;
      case 'error':
        this.toastr.error(message, title, defaultOptions);
        break;
      case 'warning':
        this.toastr.warning(message, title, defaultOptions);
        break;
      case 'info':
      default:
        this.toastr.info(message, title, defaultOptions);
        break;
    }
  }

  showSuccess(message: string, title?: string): void {
    this.toastr.success(message, title);
  }

  showError(message: string, title?: string): void {
    this.toastr.error(message, title);
  }

  showWarning(message: string, title?: string): void {
    this.toastr.warning(message, title);
  }

  showInfo(message: string, title?: string): void {
    this.toastr.info(message, title);
  }

}
