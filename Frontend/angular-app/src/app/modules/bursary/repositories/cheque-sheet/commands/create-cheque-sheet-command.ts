import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ChequeSheet} from "../../../entities/cheque-sheet";

export class CreateChequeSheetCommand extends IRequest<CreateChequeSheetCommand, ChequeSheet> {


  constructor() {
    super();
  }

  mapFrom(entity: ChequeSheet): CreateChequeSheetCommand {
    this.mapBasics(entity, this)
    return this;
  }

  mapTo(): ChequeSheet {
    throw new ApplicationError(CreateChequeSheetCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/chequeSheets/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateChequeSheetCommandHandler.name)
export class CreateChequeSheetCommandHandler implements IRequestHandler<CreateChequeSheetCommand, ChequeSheet> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: CreateChequeSheetCommand): Promise<ChequeSheet> {
    let httpRequest: HttpRequest<CreateChequeSheetCommand> = new HttpRequest<CreateChequeSheetCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateChequeSheetCommand, ServiceResult<ChequeSheet>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
