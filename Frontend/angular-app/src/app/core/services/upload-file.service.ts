/* eslint-disable @typescript-eslint/type-annotation-spacing */
/* eslint-disable @typescript-eslint/explicit-function-return-type */
/* eslint-disable @typescript-eslint/naming-convention */
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { UploadFileData } from '../components/custom/uploader/uploader.component';
import { ServiceResult } from '../models/service-result';
import { LocalStorageRepository } from './storage/local-storage-repository.service';

@Injectable({
    providedIn: 'root'
})
export class UploadFileService {
  SERVER_URL: string = `${environment.fileServer}/api/fileTransfer/FileTransfer/UploadAttachment`;
  SERVER_URL_Delete: string = `${environment.fileServer}/api/fileTransfer/FileTransfer/DeleteAttachment`;
  SERVER_URL_GetIds: string = `${environment.fileServer}/api/fileTransfer/FileTransfer/GetAttachments`;
    //  SERVER_URL_AVATAR : string = `${environment.apiUrl}/File/UploadAvatar`;
    constructor(private httpClient: HttpClient,
      private cookieService : LocalStorageRepository,) {

       }

  public upload(formData: FormData) {
      let headers = new HttpHeaders();

      headers = headers.append('Authorization', `Bearer ${this.cookieService.get('token')}`)

    return this.httpClient.post<ServiceResult<string>>(this.SERVER_URL, formData, {
            headers: headers,
            observe: 'events'
        });
  }
  public uploadInventory(formData: FormData) {
    let headers = new HttpHeaders();
    let SERVER_URL: string = `${environment.apiURL}/inventory/Reports/UploadInventory`;
    headers = headers.append('Authorization', `Bearer ${this.cookieService.get('token')}`)


    return this.httpClient.post<ServiceResult<string>>(SERVER_URL, formData, {
      headers: headers,
      observe: 'events'
    });
  }

  async delete(id: number) {

    let headers = new HttpHeaders();

    headers = headers.append('Authorization', `Bearer ${this.cookieService.get('token')}`)
    return await this.httpClient.delete<ServiceResult<string>>(
      this.SERVER_URL_Delete + `?id=${id}`,
      {
        headers: headers,
        observe: 'events'
      }
    );



  }
  public getAttachments(ids:number[]) {
    let headers = new HttpHeaders();

    headers = headers.append('Authorization', `Bearer ${this.cookieService.get('token')}`)
    return this.httpClient.post<UploadFileData[]>(this.SERVER_URL_GetIds, ids, {
      headers: headers,
      observe: 'events'
    });
  }
}

