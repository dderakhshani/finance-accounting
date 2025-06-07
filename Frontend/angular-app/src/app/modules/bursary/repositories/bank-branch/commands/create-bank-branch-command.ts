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

export class CreateBankBranchCommand extends IRequest<CreateBankBranchCommand, BankBranch> {
  bankId:number | undefined = undefined;
  bankTitle:string | undefined = undefined;
  code:string | undefined = undefined;
  title:string | undefined = undefined;
  countryDivisionId:number | undefined = undefined;
  address:string | undefined = undefined;
  phoneNumber:string | undefined = undefined;
  managerFullName:string | undefined = undefined;


  constructor() {
    super();
  }

  mapFrom(entity: BankBranch): CreateBankBranchCommand {
    throw new ApplicationError(CreateBankBranchCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): BankBranch {
    throw new ApplicationError(CreateBankBranchCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/bankBranches/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateBankBranchCommandHandler.name)
export class CreateBankBranchCommandHandler implements IRequestHandler<CreateBankBranchCommand, BankBranch> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: CreateBankBranchCommand): Promise<BankBranch> {
    let httpRequest: HttpRequest<CreateBankBranchCommand> = new HttpRequest<CreateBankBranchCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateBankBranchCommand, ServiceResult<BankBranch>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
