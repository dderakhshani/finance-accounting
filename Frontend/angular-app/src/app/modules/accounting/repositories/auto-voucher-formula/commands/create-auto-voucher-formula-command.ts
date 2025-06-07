import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {AutoVoucherFormula, VoucherFormula} from "../../../entities/AutoVoucherFormula";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class CreateAutoVoucherFormulaCommand extends IRequest<CreateAutoVoucherFormulaCommand, AutoVoucherFormula> {

  public voucherTypeId:number | undefined = undefined;
  public sourceVoucherTypeId:number | undefined = undefined;
  public orderIndex:number | undefined = undefined;
  public debitCreditStatus:number | undefined = undefined;
  public accountHeadId:number | undefined = undefined;
  public rowDescription:string | undefined = undefined;
  public formulaJson:string | undefined = undefined;
  public conditionsJson:string | undefined = undefined;
  constructor() {
    super();

  }

  mapFrom(entity: AutoVoucherFormula): CreateAutoVoucherFormulaCommand {
    this.formulaJson=entity.formula;
    this.conditionsJson=entity.conditions;
    return this;
    //throw new ApplicationError(CreateAutoVoucherFormulaCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): AutoVoucherFormula {
           throw new ApplicationError(CreateAutoVoucherFormulaCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/autoVoucherFormula/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateAutoVoucherFormulaCommandHandler.name)
export class CreateAutoVoucherFormulaCommandHandler implements IRequestHandler<CreateAutoVoucherFormulaCommand, AutoVoucherFormula> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateAutoVoucherFormulaCommand): Promise<AutoVoucherFormula> {
    let httpRequest: HttpRequest<CreateAutoVoucherFormulaCommand> = new HttpRequest<CreateAutoVoucherFormulaCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateAutoVoucherFormulaCommand, ServiceResult<AutoVoucherFormula>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
