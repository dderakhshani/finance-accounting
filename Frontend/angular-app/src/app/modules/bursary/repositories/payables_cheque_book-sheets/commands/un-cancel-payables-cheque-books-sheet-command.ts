import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ChequeBooksSheet} from "../../../entities/cheque-books-sheet";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";

export class UnCancelPayablesChequeBooksSheetCommand extends IRequest<UnCancelPayablesChequeBooksSheetCommand, ChequeBooksSheet> {
  public id: number|undefined =undefined;


  constructor() {
    super();
  }

  mapFrom(entity: ChequeBooksSheet): UnCancelPayablesChequeBooksSheetCommand {
    this.mapBasics(entity,this)
    return this;
  }

  mapTo(): ChequeBooksSheet {
    throw new ApplicationError(UnCancelPayablesChequeBooksSheetCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/Payables_ChequeBookSheets/unCancelSheet";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UnCancelPayablesChequeBooksSheetCommandHandler.name)
export class UnCancelPayablesChequeBooksSheetCommandHandler implements IRequestHandler<UnCancelPayablesChequeBooksSheetCommand, ChequeBooksSheet> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: UnCancelPayablesChequeBooksSheetCommand): Promise<ChequeBooksSheet> {
    let httpRequest: HttpRequest<UnCancelPayablesChequeBooksSheetCommand> = new HttpRequest<UnCancelPayablesChequeBooksSheetCommand>(request.url, request);


    return await this._httpService.Put<UnCancelPayablesChequeBooksSheetCommand, ServiceResult<ChequeBooksSheet>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
