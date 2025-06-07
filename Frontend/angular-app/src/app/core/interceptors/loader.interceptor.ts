// loader-interceptor.service.ts
import { Injectable } from '@angular/core';
import {
  HttpResponse,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { LoaderService } from '../services/loader.service';

@Injectable()
export class LoaderInterceptor implements HttpInterceptor {
  private requests: HttpRequest<any>[] = [];

  constructor(private loaderService: LoaderService) { }

  removeRequest(req: HttpRequest<any>) {
    const i = this.requests.indexOf(req);
    if (i >= 0) {
      this.requests.splice(i, 1);
    }
    const requestKey = this.getRequestKey(req);
    const loaderKey = this.loaderService.isLoading[requestKey ?? '0'];
    if (loaderKey)
      loaderKey.next(false);
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    this.requests.push(req);


    const requestKey = this.getRequestKey(req);
    const subject = new BehaviorSubject<boolean>(true);
    const loaderKey = this.loaderService.isLoading[requestKey ?? '0'];
    if (loaderKey)
      loaderKey.next(false);
    else
      this.loaderService.isLoading[requestKey ?? '0'] = subject;

    return new Observable(observer => {
      const subscription = next.handle(req)
        .subscribe(
          event => {
            if (event instanceof HttpResponse) {
              this.removeRequest(req);
              observer.next(event);
            }
          },
          err => {
            this.removeRequest(req);
            observer.error(err);
          },
          () => {
            this.removeRequest(req);
            observer.complete();
          });
      // remove request from queue when cancelled
      return () => {
        this.removeRequest(req);
        subscription.unsubscribe();
      };
    });
  }

  private getRequestKey(req: HttpRequest<any>): string{
    const requestUrl = req.url.split('/');
    const requestKey = requestUrl[requestUrl.length - 1];
    return requestKey.split('?')[0];
  }
}
