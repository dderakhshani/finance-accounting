import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";


export class UpdateStatusReceiptCommand extends IRequest<UpdateStatusReceiptCommand, UpdateStatusReceiptCommand> {



  public id: number | undefined = undefined;
  public statusId: number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: UpdateStatusReceiptCommand): UpdateStatusReceiptCommand {

    this.mapBasics(entity, this)

    return this;
  }

  mapTo(): UpdateStatusReceiptCommand {

    throw new ApplicationError(UpdateStatusReceiptCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/UpdateStatusReceipt";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateStatusReceiptCommandHandler.name)
export class UpdateStatusReceiptCommandHandler implements IRequestHandler<UpdateStatusReceiptCommand, UpdateStatusReceiptCommand> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateStatusReceiptCommand): Promise<UpdateStatusReceiptCommand> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateStatusReceiptCommand> = new HttpRequest<UpdateStatusReceiptCommand>(request.url, request);


    return await this._httpService.Post<UpdateStatusReceiptCommand, ServiceResult<UpdateStatusReceiptCommand>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
