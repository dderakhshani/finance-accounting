import {Inject} from "@angular/core";
import Bom from "../../../entities/bom";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";

export class GetBomQuery extends IRequest<GetBomQuery, Bom> {
  constructor(public entityId: number) {
    super();
  }


  mapFrom(entity: Bom): GetBomQuery {
    throw new ApplicationError(GetBomQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Bom {
    throw new ApplicationError(GetBomQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/bom/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetBomQueryHandler.name)
export class GetBomQueryHandler implements IRequestHandler<GetBomQuery, Bom> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetBomQuery): Promise<Bom> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetBomQuery> = new HttpRequest<GetBomQuery>(request.url, request);
    httpRequest.Query += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<Bom>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
