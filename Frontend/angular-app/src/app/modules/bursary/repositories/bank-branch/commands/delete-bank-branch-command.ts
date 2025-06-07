import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {BankBranch} from "../../../entities/bank-branch";

export class DeleteBankBranchCommand extends IRequest<DeleteBankBranchCommand, BankBranch> {

  constructor(public id: number) {
    super();
  }

  mapFrom(entity: BankBranch): DeleteBankBranchCommand {
    throw new ApplicationError(DeleteBankBranchCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): BankBranch {
    throw new ApplicationError(DeleteBankBranchCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/bankBranches/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteBankBranchCommandHandler.name)
export class DeleteBankBranchCommandHandler implements IRequestHandler<DeleteBankBranchCommand, BankBranch> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: DeleteBankBranchCommand): Promise<BankBranch> {
    let httpRequest: HttpRequest<DeleteBankBranchCommand> = new HttpRequest<DeleteBankBranchCommand>(request.url, request);
    httpRequest.Query  += `id=${request.id}`;

    return await this._httpService.Delete<ServiceResult<BankBranch>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
