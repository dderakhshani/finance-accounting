import { Injectable } from '@angular/core';
import {HttpSnackbarComponent} from "../../../core/components/material-design/http-snackbar/http-snackbar.component";
import {MatSnackBar} from "@angular/material/snack-bar";


@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(
    private _snackBar: MatSnackBar

  ) { }

  public isLoader: boolean = false
  public isLoaderDropdown: boolean = false
  showSuccessMessage(message?: string) {
    this.isLoader = false;
    this._snackBar.openFromComponent(HttpSnackbarComponent,
      {
        data: message ?? 'عملیات با موفقیت انجام شد',
        panelClass: ['success-snackbar'],
      })
  }
  showFailureMessage(message: string, statusCode: number | undefined = undefined) {
    this.isLoader = false;
    this._snackBar.openFromComponent(HttpSnackbarComponent,
      {
        data:   (statusCode ?? '') + ' ' + (message ? message :  'خطا در برقراری ارتباط با سرور'),
        panelClass: ['failure-snackbar'],
      });
  }
  showWarningMessage(message: string) {
    this.isLoader = false;
    this._snackBar.openFromComponent(HttpSnackbarComponent,
      {
        data: message,
        panelClass: ['warning-snackbar'],
      })
  }
}
