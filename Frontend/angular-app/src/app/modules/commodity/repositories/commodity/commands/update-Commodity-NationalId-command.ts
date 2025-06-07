import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { Commodity } from "../../../entities/commodity";

export class UpdateCommodityNationalIdCommand extends IRequest<UpdateCommodityNationalIdCommand, Commodity> {
  public ids: number[] = [];
  public commodityNationalId: string | undefined = undefined;
  public commodityNationalTitle: string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: Commodity): UpdateCommodityNationalIdCommand {
    throw new ApplicationError(UpdateCommodityNationalIdCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Commodity {
    throw new ApplicationError(UpdateCommodityNationalIdCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/commodity/updateCommodityNationalId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateCommodityNationalIdCommandHandler.name)
export class UpdateCommodityNationalIdCommandHandler implements IRequestHandler<UpdateCommodityNationalIdCommand, Commodity> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateCommodityNationalIdCommand): Promise<Commodity> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateCommodityNationalIdCommand> = new HttpRequest<UpdateCommodityNationalIdCommand>(request.url, request);

    return await this._httpService.Put<UpdateCommodityNationalIdCommand, ServiceResult<Commodity>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
