import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {ServiceResult} from "../../../core/models/service-result";
import {HttpRequest} from "../../../core/services/http/http-request";
import {HttpService} from "../../../core/services/http/http.service";

@Injectable({
  providedIn: 'root'
})
export class FilesService {

  constructor(
    private httpService : HttpService
  ) { }

  upload(photo: File) : Observable<ServiceResult<string>>{
    let request : HttpRequest<any> = new HttpRequest<any>('/fileTransfer/fileTransfer/upload');
    request.Body = {
      file: photo
    };
    request.BodyFormat = 'form';
    return this.httpService.Post<File,ServiceResult<string>>(request);
  }
}
