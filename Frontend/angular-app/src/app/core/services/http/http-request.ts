import {HttpHeaders} from "@angular/common/http";

export class HttpRequest<T> {
  Body: T | undefined | {};
  BodyFormat: 'json' | 'form' = 'json';
  Headers: HttpHeaders = new HttpHeaders();
  IsAuthenticatedEndPoint: boolean = true;
  Query: string = '';
  EndPoint: string = '';
  ResponseType: 'json' | 'text' | 'arraybuffer' | 'blob' = 'json';


  constructor(endpoint: string, body?: T, isAuthenticatedEndPoint: boolean = true) {
    this.EndPoint = endpoint;
    this.Body = body ? body : {};
    this.IsAuthenticatedEndPoint = isAuthenticatedEndPoint;
  }
}
