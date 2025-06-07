import {Inject} from "@angular/core";
import {Receipt} from "../../../../entities/receipt";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";


export class UpdateStatusDirectReceiptCommand extends IRequest<UpdateStatusDirectReceiptCommand, UpdateStatusDirectReceiptCommand> {



  public id: number | undefined = undefined;
  public statusId: number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: UpdateStatusDirectReceiptCommand): UpdateStatusDirectReceiptCommand {

    this.mapBasics(entity, this)

    return this;
  }

  mapTo(): UpdateStatusDirectReceiptCommand {

    throw new ApplicationError(UpdateStatusDirectReceiptCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/UpdateStatusDirectReceipt";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateStatusDirectReceiptCommandHandler.name)
export class UpdateStatusDirectReceiptCommandHandler implements IRequestHandler<UpdateStatusDirectReceiptCommand, UpdateStatusDirectReceiptCommand> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateStatusDirectReceiptCommand): Promise<UpdateStatusDirectReceiptCommand> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateStatusDirectReceiptCommand> = new HttpRequest<UpdateStatusDirectReceiptCommand>(request.url, request);


    return await this._httpService.Post<UpdateStatusDirectReceiptCommand, ServiceResult<UpdateStatusDirectReceiptCommand>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
