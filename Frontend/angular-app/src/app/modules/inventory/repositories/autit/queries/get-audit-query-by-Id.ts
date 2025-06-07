
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import { SpAudit } from "../../../entities/spAudit";
import { PaginatedList } from "../../../../../core/models/paginated-list";


export class GetAuditByIdQuery extends IRequest<GetAuditByIdQuery, PaginatedList<SpAudit>> {


  constructor(public primaryId: number, public tableName:string) {
    super();
  }
  

  mapFrom(entity: PaginatedList<SpAudit>): GetAuditByIdQuery {
    throw new ApplicationError(GetAuditByIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PaginatedList<SpAudit> {
    throw new ApplicationError(GetAuditByIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Audit/GetAuditById";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAuditByIdQueryHandler.name)
export class GetAuditByIdQueryHandler implements IRequestHandler<GetAuditByIdQuery, PaginatedList<SpAudit>> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetAuditByIdQuery): Promise<PaginatedList<SpAudit>> {
    let httpRequest: HttpRequest<GetAuditByIdQuery> = new HttpRequest<GetAuditByIdQuery>(request.url, request);
    httpRequest.Query += `PrimaryId=${request.primaryId}&TableName=${request.tableName}`;



    return await this._httpService.Get<ServiceResult<PaginatedList<SpAudit>>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
