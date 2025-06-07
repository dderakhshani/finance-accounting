import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {ChequeBooksSheet} from "../../../entities/cheque-books-sheet";

export class UpdatePayablesChequeBooksSheetCommand extends IRequest<UpdatePayablesChequeBooksSheetCommand, ChequeBooksSheet> {
  public id: number|undefined =undefined;
  public sayyadNo: string|undefined =undefined;

  constructor() {
    super();
  }

  mapFrom(entity: ChequeBooksSheet): UpdatePayablesChequeBooksSheetCommand {
    this.mapBasics(entity,this)
    return this;
  }

  mapTo(): ChequeBooksSheet {
    throw new ApplicationError(UpdatePayablesChequeBooksSheetCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/Payables_ChequeBookSheets/Update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdatePayablesChequeBooksSheetCommandHandler.name)
export class UpdatePayablesChequeBooksSheetCommandHandler implements IRequestHandler<UpdatePayablesChequeBooksSheetCommand, ChequeBooksSheet> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: UpdatePayablesChequeBooksSheetCommand): Promise<ChequeBooksSheet> {
    let httpRequest: HttpRequest<UpdatePayablesChequeBooksSheetCommand> = new HttpRequest<UpdatePayablesChequeBooksSheetCommand>(request.url, request);


    return await this._httpService.Put<UpdatePayablesChequeBooksSheetCommand, ServiceResult<ChequeBooksSheet>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}

