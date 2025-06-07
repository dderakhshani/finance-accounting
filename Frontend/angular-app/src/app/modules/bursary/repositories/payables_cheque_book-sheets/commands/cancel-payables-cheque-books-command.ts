import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ChequeBooksSheet} from "../../../entities/cheque-books-sheet";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";

export class CancelPayablesChequeBooksSheetCommand extends IRequest<CancelPayablesChequeBooksSheetCommand, ChequeBooksSheet> {
  public id: number|undefined =undefined;
  public cancelDate: string|undefined =undefined;
  public cancelDescp: string|undefined =undefined;

  constructor() {
    super();
  }

  mapFrom(entity: ChequeBooksSheet): CancelPayablesChequeBooksSheetCommand {
    this.mapBasics(entity,this)
    return this;
  }

  mapTo(): ChequeBooksSheet {
    throw new ApplicationError(CancelPayablesChequeBooksSheetCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/Payables_ChequeBookSheets/cancelSheet";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CancelPayablesChequeBooksSheetCommandHandler.name)
export class CancelPayablesChequeBooksSheetCommandHandler implements IRequestHandler<CancelPayablesChequeBooksSheetCommand, ChequeBooksSheet> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: CancelPayablesChequeBooksSheetCommand): Promise<ChequeBooksSheet> {
    let httpRequest: HttpRequest<CancelPayablesChequeBooksSheetCommand> = new HttpRequest<CancelPayablesChequeBooksSheetCommand>(request.url, request);


    return await this._httpService.Put<CancelPayablesChequeBooksSheetCommand, ServiceResult<ChequeBooksSheet>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}

