import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent, HttpResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from "rxjs/operators";
import { NotificationService } from "../../shared/services/notification/notification.service";
import { Router } from "@angular/router";
import { LocalStorageRepository } from "../services/storage/local-storage-repository.service";
import { ApplicationError } from "../exceptions/application-error";
import { ServiceResult } from "../models/service-result";
import { IdentityService } from "../../modules/identity/repositories/identity.service";
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import {
  HttpValidationErrorDialogComponent
} from "../components/material-design/http-validation-error-dialog/http-validation-error-dialog.component";

@Injectable()
export class GlobalHttpInterceptorService implements HttpInterceptor {

  constructor(
    private notificationService: NotificationService,
    private identityService: IdentityService,
    private router: Router,
    private cookieService: LocalStorageRepository,
    private matDialog: MatDialog
  ) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((httpErrorResponse) => {

        let serverResponse: ServiceResult<any> = httpErrorResponse.error;
        if (httpErrorResponse.status === 401) {
          this.identityService.logout()
        }


        else if (httpErrorResponse.status === 400) {

          if (serverResponse?.errors?.length > 0) {
            let dialogConfig = new MatDialogConfig();
            dialogConfig.data = serverResponse.errors
            this.matDialog.open(HttpValidationErrorDialogComponent, dialogConfig)
          }

        }
        else if (httpErrorResponse.status === 403) {
          this.notificationService.showFailureMessage('خطا عدم دسترسی', httpErrorResponse.status);
        }
        else if (httpErrorResponse.status === 422) {

          this.notificationService.showFailureMessage((serverResponse.ObjResult?.Error.Message), httpErrorResponse.status);
        }
        else if (httpErrorResponse.status === 499) {

          this.notificationService.showWarningMessage('عملیات در حال انجام است ، چند دقیقه دیگر به پایان می رسد.');
        }
        else {
          this.notificationService.showFailureMessage('خطا در برقراری ارتباط با سرور', httpErrorResponse.status);
        }
        return throwError(new ApplicationError(GlobalHttpInterceptorService.name, this.intercept.name, httpErrorResponse.message + " " + serverResponse));


      })
    )
  }
}
