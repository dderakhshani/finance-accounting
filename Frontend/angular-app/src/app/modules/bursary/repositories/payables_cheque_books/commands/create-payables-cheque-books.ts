import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";

import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {ChequeBook} from "../../../../bursary/entities/cheque-book";



export class CreatePayablesChequeBooksCommand  extends IRequest<CreatePayablesChequeBooksCommand , ChequeBook> {
  public bankAccountId: number|undefined =undefined;
  public getDate: string|undefined =undefined;
  public serial: string|undefined =undefined;
  public sheetsCount: number|undefined =undefined;
  public startNumber: number|undefined =undefined;
  public descp: string | null|undefined =undefined;
  public id: number|undefined =undefined;



  constructor() {
    super();
  }

  mapFrom(entity: ChequeBook): CreatePayablesChequeBooksCommand  {
    throw new ApplicationError(CreatePayablesChequeBooksCommand .name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): ChequeBook {
    throw new ApplicationError(CreatePayablesChequeBooksCommand .name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/bursary/Payables_ChequeBooks/Add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreatePayablesChequeBooksCommandHandler.name)
export class CreatePayablesChequeBooksCommandHandler implements IRequestHandler<CreatePayablesChequeBooksCommand , ChequeBook> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: CreatePayablesChequeBooksCommand ): Promise<ChequeBook> {
    let httpRequest: HttpRequest<CreatePayablesChequeBooksCommand > = new HttpRequest<CreatePayablesChequeBooksCommand >(request.url, request);

    return await this._httpService.Post<CreatePayablesChequeBooksCommand , ServiceResult<ChequeBook>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
