import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {ChequeSheet} from "../../../entities/cheque-sheet";


export class GetChequeSheetQuery extends IRequest<GetChequeSheetQuery, ChequeSheet> {

  constructor(public entityId?: number) {
    super();
  }
  mapFrom(entity: ChequeSheet): GetChequeSheetQuery {
    return new GetChequeSheetQuery();
  }

  mapTo(): ChequeSheet {
    return new ChequeSheet();
  }

  get url(): string {
    return "/bursary/chequeSheets/get";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


@MediatorHandler(GetChequeSheetQueryHandler.name)
export class GetChequeSheetQueryHandler implements IRequestHandler<GetChequeSheetQuery, ChequeSheet>{
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request:GetChequeSheetQuery) : Promise<ChequeSheet> {
    let httpRequest = new HttpRequest(request.url,request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Get<ServiceResult<ChequeSheet>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }
}
