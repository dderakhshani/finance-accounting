import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../../core/services/http/http-request";
import { CorrectionRequest } from "../../../../entities/CorrectionRequest";


export class GetCorrectionRequestByIdQuery extends IRequest<GetCorrectionRequestByIdQuery, CorrectionRequest> {
  constructor(public Id: number) {
    super();
  }


  mapFrom(entity: CorrectionRequest): GetCorrectionRequestByIdQuery {
    throw new ApplicationError(GetCorrectionRequestByIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CorrectionRequest {
    throw new ApplicationError(GetCorrectionRequestByIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/GetCorrectionRequestById";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetCorrectionRequestByIdQueryHandler.name)
export class GetCorrectionRequestByIdQueryHandler implements IRequestHandler<GetCorrectionRequestByIdQuery, CorrectionRequest> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetCorrectionRequestByIdQuery): Promise<CorrectionRequest> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<GetCorrectionRequestByIdQuery> = new HttpRequest<GetCorrectionRequestByIdQuery>(request.url, request);
    httpRequest.Query += `Id=${request.Id}`;


    return await this._httpService.Get<ServiceResult<CorrectionRequest>>(httpRequest).toPromise().then(response => {
     
     
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
