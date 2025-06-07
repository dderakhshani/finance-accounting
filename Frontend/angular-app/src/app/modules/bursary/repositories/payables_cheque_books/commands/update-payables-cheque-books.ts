import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ChequeBook} from "../../../../bursary/entities/cheque-book";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";

export class UpdatePayablesChequeBooksCommand extends IRequest<UpdatePayablesChequeBooksCommand, ChequeBook> {
  public bankAccountId: number|undefined =undefined;
  public getDate: string|undefined =undefined;
  public serial: string|undefined =undefined;
  public sheetsCount: number|undefined =undefined;
  public startNumber: number|undefined =undefined;
  public descp: string | null|undefined =undefined;
  public bankAccount: any|undefined =undefined;
  public payables_ChequeBooksSheets: any[]|undefined =undefined;
  public id: number|undefined =undefined;
  public isDeleted: boolean|undefined =undefined;


  constructor() {
    super();
  }

  mapFrom(entity: ChequeBook): UpdatePayablesChequeBooksCommand {
    this.mapBasics(entity,this)
    return this;
  }

  mapTo(): ChequeBook {
    throw new ApplicationError(UpdatePayablesChequeBooksCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/Payables_ChequeBooks/Update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdatePayablesChequeBooksCommandHandler.name)
export class UpdatePayablesChequeBooksCommandHandler implements IRequestHandler<UpdatePayablesChequeBooksCommand, ChequeBook> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: UpdatePayablesChequeBooksCommand): Promise<ChequeBook> {
    let httpRequest: HttpRequest<UpdatePayablesChequeBooksCommand> = new HttpRequest<UpdatePayablesChequeBooksCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Put<UpdatePayablesChequeBooksCommand, ServiceResult<ChequeBook>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
