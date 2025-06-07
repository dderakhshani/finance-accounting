import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {Cheque} from "../../../../entities/cheque";
import { ChequeSheet } from "../../../../entities/cheque-sheet";

export class CreateChequeSheetCommand extends IRequest<CreateChequeSheetCommand, ChequeSheet> {

  public id	: number | undefined =undefined
  public payChequeId	: number | undefined =undefined
  public sheetSeqNumber	 : number | undefined =undefined
  public sheetUniqueNumber	 : string | undefined =undefined
  public title	 : string | undefined =undefined

  public sheetSeriNumber	 : number | undefined =undefined
  public totalCost	 : number | undefined =undefined
  public accountReferenceId	: number | undefined =undefined

  public bankId :number | undefined = undefined
  public bankBranchId	: number | undefined =undefined
  public issueDate	 : number | undefined =undefined
  public receiptDate	 : number | undefined =undefined
  public issuerEmployeeId	: number | undefined =undefined
  public description	 : string | undefined =undefined
  public accountNumber	 : string | undefined =undefined
  public branchName	 : string | undefined =undefined
  public chequeSheetId	 : number | undefined =undefined
  public ownerChequeReferenceId : number | undefined = undefined
  public ownerChequeReferenceGroupId	: number | undefined =undefined

  public receiveChequeReferenceId : number | undefined = undefined
  public receiveChequeReferenceGroupId	: number | undefined =undefined

  public recipientChequeReferenceId : number | undefined = undefined
  public chequeTypeBaseId	: number | undefined = undefined
  public isActive	: boolean | undefined = false
  public approveReceivedChequeSheet	: boolean | undefined = undefined


  constructor() {
    super();
  }

  mapFrom(entity: ChequeSheet): CreateChequeSheetCommand {

    this.mapBasics(entity,this);
    return this;

  }

  mapTo(): ChequeSheet {
    throw new ApplicationError(CreateChequeSheetCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/chequeSheet/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateChequeSheetCommandHandler.name)
export class CreateChequeSheetCommandHandler implements IRequestHandler<CreateChequeSheetCommand, ChequeSheet> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateChequeSheetCommand): Promise<ChequeSheet> {
    let httpRequest: HttpRequest<CreateChequeSheetCommand> = new HttpRequest<CreateChequeSheetCommand>(request.url, request);

    return await this._httpService.Post<CreateChequeSheetCommand, ServiceResult<ChequeSheet>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
