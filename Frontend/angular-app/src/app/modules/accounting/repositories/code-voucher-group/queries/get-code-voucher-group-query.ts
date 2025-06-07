import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {CodeVoucherGroup} from "../../../entities/code-voucher-group";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class getCodeVoucherGroupQuery extends IRequest<getCodeVoucherGroupQuery, CodeVoucherGroup> {
  constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: CodeVoucherGroup): getCodeVoucherGroupQuery {
    throw new ApplicationError(getCodeVoucherGroupQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CodeVoucherGroup {
    throw new ApplicationError(getCodeVoucherGroupQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/CodeVoucherGroup/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(getCodeVoucherGroupQueryHandler.name)
export class getCodeVoucherGroupQueryHandler implements IRequestHandler<getCodeVoucherGroupQuery, CodeVoucherGroup> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: getCodeVoucherGroupQuery): Promise<CodeVoucherGroup> {
    let httpRequest: HttpRequest<getCodeVoucherGroupQuery> = new HttpRequest<getCodeVoucherGroupQuery>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()



    return await this._httpService.Get<ServiceResult<CodeVoucherGroup>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
