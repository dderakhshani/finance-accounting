import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {Cheque} from "../../../entities/cheque";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetChequeQuery extends IRequest<GetChequeQuery, Cheque> {
  constructor(public entityId: number) {
    super();
  }

  mapFrom(entity: Cheque): GetChequeQuery {
    throw new ApplicationError(GetChequeQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Cheque {
    throw new ApplicationError(GetChequeQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/cheque/getbyid";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetChequeQueryHandler.name)
export class GetChequeQueryHandler implements IRequestHandler<GetChequeQuery, Cheque> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetChequeQuery): Promise<Cheque> {
    let httpRequest: HttpRequest<GetChequeQuery> = new HttpRequest<GetChequeQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<Cheque>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
