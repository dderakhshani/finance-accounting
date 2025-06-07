import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {AutoVoucherFormula} from "../../../entities/AutoVoucherFormula";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PaginatedList} from "../../../../../core/models/paginated-list";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetAutoVoucherFormulaQuery extends IRequest<GetAutoVoucherFormulaQuery, AutoVoucherFormula> {


  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: AutoVoucherFormula): GetAutoVoucherFormulaQuery {
    throw new ApplicationError(GetAutoVoucherFormulaQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }
  mapTo(): AutoVoucherFormula {
    throw new ApplicationError(GetAutoVoucherFormulaQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/autoVoucherFormula/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAutoVoucherFormulaQueryHandler.name)
export class GetAutoVoucherFormulaQueryHandler implements IRequestHandler<GetAutoVoucherFormulaQuery, AutoVoucherFormula> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetAutoVoucherFormulaQuery): Promise<AutoVoucherFormula> {
    let httpRequest: HttpRequest<GetAutoVoucherFormulaQuery> = new HttpRequest<GetAutoVoucherFormulaQuery>(request.url, request);
     httpRequest.Query += `Id=${request.entityId}`;
    return await this._httpService.Get<ServiceResult<AutoVoucherFormula>>(httpRequest).toPromise().then(response => {

      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}

