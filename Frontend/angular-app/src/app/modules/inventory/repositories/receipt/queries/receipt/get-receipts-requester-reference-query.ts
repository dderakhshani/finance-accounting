import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { Receipt } from "../../../../entities/receipt";
import { Data } from "@angular/router";


export class GetRecepitRequesterReferenceQuery extends IRequest<GetRecepitRequesterReferenceQuery, Receipt> {
 

  constructor(public entityId: number, public fromDate: Date | undefined = undefined, public toDate: Date | undefined = undefined) {
    super();
  }


  mapFrom(entity: Receipt): GetRecepitRequesterReferenceQuery {
    throw new ApplicationError(GetRecepitRequesterReferenceQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Receipt {
    throw new ApplicationError(GetRecepitRequesterReferenceQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/TemporaryReceipt/GetByRequesterReferenceId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetRecepitRequesterReferenceQueryHandler.name)
export class GetRecepitRequesterReferenceQueryHandler implements IRequestHandler<GetRecepitRequesterReferenceQuery, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
   
  ) {
  }

  async Handle(request: GetRecepitRequesterReferenceQuery): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetRecepitRequesterReferenceQuery> = new HttpRequest<GetRecepitRequesterReferenceQuery>(request.url, request);
    httpRequest.Query += `RequesterReferenceId=${request.entityId}&fromDate=${request?.fromDate}&toDate=${request?.toDate}`;


    return await this._httpService.Get<ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {
     
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
