import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Branch} from "../../../entities/branch";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class GetBranchQuery extends IRequest<GetBranchQuery, Branch> {
  constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: Branch): GetBranchQuery {
    throw new ApplicationError(GetBranchQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Branch {
    throw new ApplicationError(GetBranchQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/branch/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBranchQueryHandler.name)
export class GetBranchQueryHandler implements IRequestHandler<GetBranchQuery, Branch> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetBranchQuery): Promise<Branch> {
    let httpRequest: HttpRequest<GetBranchQuery> = new HttpRequest<GetBranchQuery>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<Branch>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
