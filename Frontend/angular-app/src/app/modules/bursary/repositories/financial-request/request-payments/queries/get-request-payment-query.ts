import { Inject } from "@angular/core";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { ServiceResult } from "src/app/core/models/service-result";
import { HttpService } from "src/app/core/services/http/http.service";
import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { RequestPayment } from "src/app/modules/bursary/entities/request-payment";
import {HttpRequest} from "../../../../../../core/services/http/http-request";

export class GetRequestPaymentQuery extends IRequest<GetRequestPaymentQuery, RequestPayment> {

  public id!:number;

  constructor(id:number ) {
    super();
    this.id = id;
  }


  mapFrom(entity: RequestPayment): GetRequestPaymentQuery {
    throw new ApplicationError(GetRequestPaymentQuery.name,this.mapFrom.name,'Not Implemented Yet')
  }

  mapTo(): RequestPayment {
    throw new ApplicationError(GetRequestPaymentQuery.name,this.mapTo.name,'Not Implemented Yet')
  }

  get url(): string {
    return "";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}
@MediatorHandler(GetRequestPaymentQueryHandler.name)
export class GetRequestPaymentQueryHandler implements IRequestHandler<GetRequestPaymentQuery, RequestPayment> {
  constructor(
    @Inject(HttpService) private _httpService:HttpService
  ) {
  }
  async Handle(request: GetRequestPaymentQuery) : Promise<RequestPayment> {
    let httpRequest = new HttpRequest<GetRequestPaymentQuery>(request.url,request);
    httpRequest.Query += `id=${request.id}`;

    return await this._httpService.Get<ServiceResult<RequestPayment>>(httpRequest).toPromise().then(res => {
      return res.objResult
    });

  }
}
