import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";


export class UpdateReadStatusReceiptCommand extends IRequest<UpdateReadStatusReceiptCommand, UpdateReadStatusReceiptCommand> {
  public ids: Array<number> | undefined = undefined;
  public isRead: boolean | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: UpdateReadStatusReceiptCommand): UpdateReadStatusReceiptCommand {

    this.mapBasics(entity, this)

    return this;
  }

  mapTo(): UpdateReadStatusReceiptCommand {

    throw new ApplicationError(UpdateReadStatusReceiptCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/UpdateReadStatusReceipt";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateReadStatusReceiptCommandHandler.name)
export class UpdateReadStatusReceiptCommandHandler implements IRequestHandler<UpdateReadStatusReceiptCommand, UpdateReadStatusReceiptCommand> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateReadStatusReceiptCommand): Promise<UpdateReadStatusReceiptCommand> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateReadStatusReceiptCommand> = new HttpRequest<UpdateReadStatusReceiptCommand>(request.url, request);


    return await this._httpService.Put<UpdateReadStatusReceiptCommand, ServiceResult<UpdateReadStatusReceiptCommand>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
