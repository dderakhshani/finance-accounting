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

export class UpdateAutoVoucherFormulaCommand extends IRequest<UpdateAutoVoucherFormulaCommand, AutoVoucherFormula> {
  public id:number | undefined = undefined;
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

  mapFrom(entity: AutoVoucherFormula): UpdateAutoVoucherFormulaCommand {
    this.mapBasics(entity,this);
    this.formulaJson=entity.formula;
    this.conditionsJson=entity.conditions;
    // this.formulaJson = JSON.parse(entity.formula,function(prop, value) {
    //   let lower = prop.charAt(0).toLowerCase() + prop.slice(1);
    //   if(prop === lower) return value;
    //   else this[lower] = value;
    //})
    return this;
  }

  mapTo(): AutoVoucherFormula {
    throw new ApplicationError(UpdateAutoVoucherFormulaCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/autoVoucherFormula/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateAutoVoucherFormulaCommandHandler.name)
export class UpdateAutoVoucherFormulaCommandHandler implements IRequestHandler<UpdateAutoVoucherFormulaCommand, AutoVoucherFormula> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateAutoVoucherFormulaCommand): Promise<AutoVoucherFormula> {
    let httpRequest: HttpRequest<UpdateAutoVoucherFormulaCommand> = new HttpRequest<UpdateAutoVoucherFormulaCommand>(request.url, request);

    return await this._httpService.Put<UpdateAutoVoucherFormulaCommand, ServiceResult<AutoVoucherFormula>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
