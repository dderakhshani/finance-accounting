import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {HttpRequest} from "./http-request";
import {environment} from "../../../../environments/environment";
import {LocalStorageRepository} from "../storage/local-storage-repository.service";
import {TabManagerService} from "../../../layouts/main-container/tab-manager.service";
import { catchError } from 'rxjs/internal/operators/catchError';
import { timeout } from "rxjs/operators";
@Injectable({
  providedIn: 'root'
})
export class HttpService {
  constructor(
    private http: HttpClient,
    private cookieService : LocalStorageRepository,
  ) {}


  Post<TIn, TOut>(request: HttpRequest<TIn>) : Observable<TOut> {
    request = this.EvaluateRequest(request);
    // @ts-ignore
    return this.http.post<TOut>(request.EndPoint,request.Body, { headers: request.Headers, responseType: request.ResponseType});
  }
  Put<TIn, TOut>(request: HttpRequest<TIn>) : Observable<TOut> {
    request = this.EvaluateRequest(request);

    return this.http.put<TOut>(request.EndPoint,request.Body, { headers: request.Headers});
  }
  Get<TOut>(request: HttpRequest<any>): Observable<TOut> {

    request = this.EvaluateRequest(request);
   
    // @ts-ignore
    return this.http.get<TOut>(request.EndPoint, { headers: request.Headers, 'responseType': request.ResponseType })
  }
  Delete<TOut>(request: HttpRequest<any>) : Observable<TOut> {
    request = this.EvaluateRequest(request);

    return this.http.delete<TOut>(request.EndPoint, { headers: request.Headers, body:request.Body});
  }


  private EvaluateRequest(request : HttpRequest<any>) : HttpRequest<any> {
    // Add Base URL to API Route


    if (request.EndPoint.startsWith("/"))
    request.EndPoint = environment.apiURL +  request.EndPoint;

    if (request.BodyFormat === 'form') {
      let formData = new FormData();
      this.buildFormData(formData,request.Body);
      request.Body = formData;
    }
    if(request.Query) {
      request.EndPoint += '?' + request.Query;
    }
    
    if(request.IsAuthenticatedEndPoint) {
      request.Headers = request.Headers.append('Authorization', `Bearer ${this.cookieService.get('token')}`)
    }
    return request
  }

  private buildFormData(formData:FormData, data:any, parentKey?:any) {
    if (data && typeof data === 'object' && !(data instanceof Date) && !(data instanceof File)) {
      Object.keys(data).forEach(key => {
        this.buildFormData(formData, data[key], parentKey ? `${parentKey}[${key}]` : key);
      });
    } else {
      const value = data == null ? '' : data;

      formData.append(parentKey, value);
    }
  }
}
