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

export class GetRecepitListIdQuery extends IRequest<GetRecepitListIdQuery, Receipt> {
  public ListIds: string[] = [];
  constructor() {
    super();
  }


  mapFrom(entity: Receipt): GetRecepitListIdQuery {
    throw new ApplicationError(GetRecepitListIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Receipt {
    throw new ApplicationError(GetRecepitListIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetByListId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetRecepitListIdQueryHandler.name)
export class GetRecepitListIdQueryHandler implements IRequestHandler<GetRecepitListIdQuery, GetRecepitListIdQuery> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetRecepitListIdQuery): Promise<GetRecepitListIdQuery> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetRecepitListIdQuery> = new HttpRequest<GetRecepitListIdQuery>(request.url, request);


    return await this._httpService.Post<GetRecepitListIdQuery, ServiceResult<GetRecepitListIdQuery>>(httpRequest).toPromise().then(response => {

     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
