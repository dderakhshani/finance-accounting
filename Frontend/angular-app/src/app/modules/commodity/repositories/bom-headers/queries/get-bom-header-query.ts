import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import { BomHeader } from "../../../entities/boms-header";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";

export class GetBomHeaderQuery extends IRequest<GetBomHeaderQuery, BomHeader> {
  constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: BomHeader): GetBomHeaderQuery {
    throw new ApplicationError(GetBomHeaderQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): BomHeader {
    throw new ApplicationError(GetBomHeaderQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/bomValueHeader/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBomHeaderQueryHandler.name)
export class GetBomHeaderQueryHandler implements IRequestHandler<GetBomHeaderQuery, BomHeader> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetBomHeaderQuery): Promise<BomHeader> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetBomHeaderQuery> = new HttpRequest<GetBomHeaderQuery>(request.url, request);
    httpRequest.Query += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<BomHeader>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
