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


export class UpdatePrintCountCommand extends IRequest<UpdatePrintCountCommand, UpdatePrintCountCommand> {



  public id: number | undefined = undefined;
  
  constructor() {
    super();
  }

  mapFrom(entity: UpdatePrintCountCommand): UpdatePrintCountCommand {

    this.mapBasics(entity, this)

    return this;
  }

  mapTo(): UpdatePrintCountCommand {

    throw new ApplicationError(UpdatePrintCountCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/UpdatePrintCountReceipt";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdatePrintCountCommandHandler.name)
export class UpdatePrintCountCommandHandler implements IRequestHandler<UpdatePrintCountCommand, UpdatePrintCountCommand> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdatePrintCountCommand): Promise<UpdatePrintCountCommand> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdatePrintCountCommand> = new HttpRequest<UpdatePrintCountCommand>(request.url, request);


    return await this._httpService.Post<UpdatePrintCountCommand, ServiceResult<UpdatePrintCountCommand>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
