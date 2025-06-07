import {Inject} from "@angular/core";
import {Quantities, Receipt} from "../../../../entities/receipt";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";


export class UpdateIsDocumentIssuanceCommand extends IRequest<UpdateIsDocumentIssuanceCommand, UpdateIsDocumentIssuanceCommand> {

  public ids: string[]= [];
  
  constructor() {
    super();
  }

  mapFrom(entity: UpdateIsDocumentIssuanceCommand): UpdateIsDocumentIssuanceCommand {

    this.mapBasics(entity, this)

    return this;
  }

  mapTo(): UpdateIsDocumentIssuanceCommand {

    throw new ApplicationError(UpdateIsDocumentIssuanceCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/UpdateIsDocumentIssuance";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateIsDocumentIssuanceCommandHandler.name)
export class UpdateIsDocumentIssuanceCommandHandler implements IRequestHandler<UpdateIsDocumentIssuanceCommand, UpdateIsDocumentIssuanceCommand> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateIsDocumentIssuanceCommand): Promise<UpdateIsDocumentIssuanceCommand> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateIsDocumentIssuanceCommand> = new HttpRequest<UpdateIsDocumentIssuanceCommand>(request.url, request);


    return await this._httpService.Post<UpdateIsDocumentIssuanceCommand, ServiceResult<UpdateIsDocumentIssuanceCommand>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
