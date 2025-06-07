import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {BankAccount} from "../../../entities/bank-account";

export class CreateBankAccountCommand extends IRequest<CreateBankAccountCommand, BankAccount> {
  parentId: number | undefined = undefined;
  bankBranchId: number | undefined = undefined;
  bankBranchTitle: string | undefined = undefined;
  title: string | undefined = undefined;
  sheba: string | undefined = undefined;
  accountNumber: string | undefined = undefined;
  subsidiaryCodeId: number | undefined = undefined;
  relatedBankAccountId: number | undefined = undefined;
  accountTypeBaseId: number | undefined = undefined;
  accountReferenceGroupId: number | undefined = undefined;
  accountStatus: number | undefined = undefined;
  withdrawalLimit: number | undefined = undefined;
  haveChekBook: boolean | undefined = undefined;
  currenceTypeBaseId: number | undefined = undefined;
  signersJson: string | undefined = undefined;


  constructor() {
    super();
  }

  mapFrom(entity: BankAccount): CreateBankAccountCommand {
    throw new ApplicationError(CreateBankAccountCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): BankAccount {
    throw new ApplicationError(CreateBankAccountCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/bankAccounts/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateBankAccountCommandHandler.name)
export class CreateBankAccountCommandHandler implements IRequestHandler<CreateBankAccountCommand, BankAccount> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: CreateBankAccountCommand): Promise<BankAccount> {
    let httpRequest: HttpRequest<CreateBankAccountCommand> = new HttpRequest<CreateBankAccountCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateBankAccountCommand, ServiceResult<BankAccount>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
