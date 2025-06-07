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

export class UpdateUpdateVoucherHeadIdCommand extends IRequest<UpdateUpdateVoucherHeadIdCommand, boolean> {

  public receiveIds: number[] = [] ;
  public voucherHeadId : number | undefined = undefined


  constructor() {
    super();
  }

  mapFrom(entity: boolean):UpdateUpdateVoucherHeadIdCommand  {
    throw new ApplicationError(UpdateUpdateVoucherHeadIdCommand.name, this.mapTo.name, 'Not Implemented Yet')
    }


  mapTo(): boolean {
    throw new ApplicationError(UpdateUpdateVoucherHeadIdCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/CustomerReceipt/UpdateVoucherHeadId";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateUpdateVoucherHeadIdCommandHandler.name)
export class UpdateUpdateVoucherHeadIdCommandHandler implements IRequestHandler<UpdateUpdateVoucherHeadIdCommand, boolean> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateUpdateVoucherHeadIdCommand) {
    let httpRequest: HttpRequest<UpdateUpdateVoucherHeadIdCommand> = new HttpRequest<UpdateUpdateVoucherHeadIdCommand>(request.url, request);

    return await this._httpService.Put<UpdateUpdateVoucherHeadIdCommand, ServiceResult<boolean>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.succeed
    })

  }
}
