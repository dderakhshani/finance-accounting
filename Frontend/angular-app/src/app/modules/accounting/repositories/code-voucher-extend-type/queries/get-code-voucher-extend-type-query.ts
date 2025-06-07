import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {CodeVoucherExtendType} from "../../../entities/code-voucher-extend-type";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetCodeVoucherExtendTypeQuery extends IRequest<GetCodeVoucherExtendTypeQuery, CodeVoucherExtendType> {
  constructor(public entityId: number) {
    super();
  }



  mapFrom(entity: CodeVoucherExtendType): GetCodeVoucherExtendTypeQuery {
    throw new ApplicationError(GetCodeVoucherExtendTypeQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CodeVoucherExtendType {
    throw new ApplicationError(GetCodeVoucherExtendTypeQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/CodeVoucherExtendType/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCodeVoucherExtendTypeQueryHandler.name)
export class GetCodeVoucherExtendTypeQueryHandler implements IRequestHandler<GetCodeVoucherExtendTypeQuery, CodeVoucherExtendType> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCodeVoucherExtendTypeQuery): Promise<CodeVoucherExtendType> {
    let httpRequest: HttpRequest<GetCodeVoucherExtendTypeQuery> = new HttpRequest<GetCodeVoucherExtendTypeQuery>(request.url, request);

    return await this._httpService.Get<ServiceResult<CodeVoucherExtendType>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
