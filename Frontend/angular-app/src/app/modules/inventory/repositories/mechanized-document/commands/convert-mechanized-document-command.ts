import {Inject} from "@angular/core";
import { Receipt } from "../../../entities/receipt";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class ConvertToMechanizedDocumentCommand extends IRequest<ConvertToMechanizedDocumentCommand, Receipt> {
  
 
  constructor() {
    super();
  }

  mapFrom(entity: Receipt): ConvertToMechanizedDocumentCommand {

    throw new ApplicationError(ConvertToMechanizedDocumentCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Receipt {

    throw new ApplicationError(ConvertToMechanizedDocumentCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/ConvertToMechanizedDocument";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(ConvertToMechanizedDocumentCommandHandler.name)
export class ConvertToMechanizedDocumentCommandHandler implements IRequestHandler<ConvertToMechanizedDocumentCommand, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: ConvertToMechanizedDocumentCommand): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<ConvertToMechanizedDocumentCommand> = new HttpRequest<ConvertToMechanizedDocumentCommand>(request.url, request);


    return await this._httpService.Post<ConvertToMechanizedDocumentCommand, ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {

      
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
