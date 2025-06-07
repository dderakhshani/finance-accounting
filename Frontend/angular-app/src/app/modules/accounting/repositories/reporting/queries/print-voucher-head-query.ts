import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class PrintVoucherHeadQuery extends IRequest<PrintVoucherHeadQuery, any> {
  constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: any): PrintVoucherHeadQuery {
    throw new ApplicationError(PrintVoucherHeadQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): any {
    throw new ApplicationError(PrintVoucherHeadQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/reporting/printVoucherHead";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}
function download(data:any, filename:any, type:any) {
  let file = new Blob([data], {type: type});
  // @ts-ignore
  if (window.navigator.msSaveOrOpenBlob) // IE10+
    { // @ts-ignore
      window.navigator.msSaveOrOpenBlob(file, filename);
    }
  else { // Others
    let a = document.createElement("a"),
      url = URL.createObjectURL(file);
    a.href = url;
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    setTimeout(function() {
      document.body.removeChild(a);
      window.URL.revokeObjectURL(url);
    }, 0);
  }
}
@MediatorHandler(PrintVoucherHeadQueryHandler.name)
export class PrintVoucherHeadQueryHandler implements IRequestHandler<PrintVoucherHeadQuery, any> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService
  ) {
  }

  async Handle(request: PrintVoucherHeadQuery): Promise<any> {
    let httpRequest: HttpRequest<PrintVoucherHeadQuery> = new HttpRequest<PrintVoucherHeadQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    httpRequest.ResponseType = "blob";
    return await this._httpService.Get<ServiceResult<any>>(httpRequest).toPromise().then((response:any) => {
      download(response,'test.pdf','application/pdf')
      return response
    })
  }
}
