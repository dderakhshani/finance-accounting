import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class DeleteCommodityCommand extends IRequest<DeleteCommodityCommand, boolean> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: boolean): DeleteCommodityCommand {
    throw new ApplicationError(DeleteCommodityCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): boolean {
    throw new ApplicationError(DeleteCommodityCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/commodity/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteCommodityCommandHandler.name)
export class DeleteCommodityCommandHandler implements IRequestHandler<DeleteCommodityCommand, boolean> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteCommodityCommand): Promise<boolean> {
    let httpRequest: HttpRequest<DeleteCommodityCommand> = new HttpRequest<DeleteCommodityCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;
    this._notificationService.isLoader = true;

    return await this._httpService.Delete<ServiceResult<boolean>>(httpRequest).toPromise().then(response => {
       
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
