import { Inject } from "@angular/core";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { ServiceResult } from "src/app/core/models/service-result";
import { HttpRequest } from "src/app/core/services/http/http-request";
import { HttpService } from "src/app/core/services/http/http.service";
import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { FinancialRequest } from "src/app/modules/bursary/entities/financial-request";
import { NotificationService } from "src/app/shared/services/notification/notification.service";

export class UpdateIsPendingCommand extends IRequest<UpdateIsPendingCommand, boolean> {

  public receiveIds: number[] = [] ;



  constructor() {
    super();
  }

  mapFrom(entity: boolean):UpdateIsPendingCommand  {
    throw new ApplicationError(UpdateIsPendingCommand.name, this.mapTo.name, 'Not Implemented Yet')
    }


  mapTo(): boolean {
    throw new ApplicationError(UpdateIsPendingCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/CustomerReceipt/UpdateIsPending";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateIsPendingCommandHandler.name)
export class UpdateIsPendingCommandHandler implements IRequestHandler<UpdateIsPendingCommand, boolean> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateIsPendingCommand) {
    let httpRequest: HttpRequest<UpdateIsPendingCommand> = new HttpRequest<UpdateIsPendingCommand>(request.url, request);

    return await this._httpService.Put<UpdateIsPendingCommand, ServiceResult<boolean>>(httpRequest).toPromise().then(response => {
      // this._notificationService.showSuccessMessage()
      return response.succeed
    })

  }
}
