import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";

export class InsertBetweenVouchersHeadCommand extends IRequest<InsertBetweenVouchersHeadCommand, any>{
  public insertAfterVoucherNo:number | undefined = undefined;
  public fromNo:number | undefined = undefined;
  public toNo:number | undefined = undefined;


  mapFrom(entity: any): InsertBetweenVouchersHeadCommand {
    return new InsertBetweenVouchersHeadCommand;
  }

  mapTo(): any {
  }

  get url(): string {
    return "/accounting/vouchersHead/insertbetween";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }


}

@MediatorHandler(InsertBetweenVouchersHeadCommandHandler.name)
export class InsertBetweenVouchersHeadCommandHandler implements IRequestHandler<InsertBetweenVouchersHeadCommand, any>{

  constructor(
    @Inject(HttpService) private _httpService:HttpService
  ) {
  }

  async Handle(request: InsertBetweenVouchersHeadCommand) {
    let httpRequest = new HttpRequest(request.url,request);
    return this._httpService.Post<any,ServiceResult<any>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })  }

}

