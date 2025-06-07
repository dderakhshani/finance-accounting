import { Inject } from "@angular/core";
import { Assets } from "../../../entities/Assets";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";

export class UpdateAssetsCommand extends IRequest<UpdateAssetsCommand, Assets> {



  public id: number | undefined = undefined;
  public assetGroupId: number | undefined = undefined;
  public unitId: number | undefined = undefined;
  public commoditySerial: string | undefined = undefined;
  public assetSerial: string | undefined = undefined;
  public depreciationTypeBaseId: number | undefined = undefined;
  public isActive: boolean | undefined = undefined;
  public commodityTitle: string | undefined = undefined;
  public description: string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: Assets): UpdateAssetsCommand {
    this.mapBasics(entity, this);

    return this;
  }

  mapTo(): Assets {

    throw new ApplicationError(UpdateAssetsCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/Assets/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateAssetsCommandHandler.name)
export class UpdateAssetsCommandHandler implements IRequestHandler<UpdateAssetsCommand, Assets> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateAssetsCommand): Promise<Assets> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateAssetsCommand> = new HttpRequest<UpdateAssetsCommand>(request.url, request);




    return await this._httpService.Put<UpdateAssetsCommand, ServiceResult<Assets>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
