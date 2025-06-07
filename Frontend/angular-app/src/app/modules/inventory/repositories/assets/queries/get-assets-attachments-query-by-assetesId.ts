
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";


export class GetAssetAttachmentsIdByAssetIdQuery extends IRequest<GetAssetAttachmentsIdByAssetIdQuery, number[]> {


  constructor(public assetId: number, public personsDebitedCommoditiesId:number) {
    super();
  }


  mapFrom(entity: number[]): GetAssetAttachmentsIdByAssetIdQuery {
    throw new ApplicationError(GetAssetAttachmentsIdByAssetIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): number[] {
    throw new ApplicationError(GetAssetAttachmentsIdByAssetIdQuery.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Assets/GetAssetAttachmentsIdByPersonsDebitedCommoditiesId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(GetAssetAttachmentsIdByAssetIdQueryHandler.name)
export class GetAssetAttachmentsIdByAssetIdQueryHandler implements IRequestHandler<GetAssetAttachmentsIdByAssetIdQuery, number[]> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: GetAssetAttachmentsIdByAssetIdQuery): Promise<number[]> {
    let httpRequest: HttpRequest<GetAssetAttachmentsIdByAssetIdQuery> = new HttpRequest<GetAssetAttachmentsIdByAssetIdQuery>(request.url, request);
    httpRequest.Query += `AssetId=${request.assetId}&PersonsDebitedCommoditiesId=${request.personsDebitedCommoditiesId}`;



    return await this._httpService.Get<ServiceResult<number[]>>(httpRequest).toPromise().then(response => {
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
