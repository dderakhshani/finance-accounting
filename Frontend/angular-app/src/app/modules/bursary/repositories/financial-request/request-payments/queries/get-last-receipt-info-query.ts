 
import { Inject } from "@angular/core";
import { ApplicationError } from "src/app/core/exceptions/application-error";
import { ServiceResult } from "src/app/core/models/service-result";
import { HttpRequest } from "src/app/core/services/http/http-request";
import { HttpService } from "src/app/core/services/http/http.service";
import { MediatorHandler } from "src/app/core/services/mediator/decorator";
import { IRequest, IRequestHandler } from "src/app/core/services/mediator/interfaces";
import { ValidationRule } from "src/app/core/validation/validation-rule";
import { FinancialRequest } from "src/app/modules/bursary/entities/financial-request";
 

export class GetLastReceiptInfoQuery extends IRequest<GetLastReceiptInfoQuery, FinancialRequest> {
    constructor() {
      super();
    }
  
    mapFrom(entity: FinancialRequest): GetLastReceiptInfoQuery {
      throw new ApplicationError(GetLastReceiptInfoQuery.name,this.mapFrom.name,'Not Implemented Yet')
    }
  
    mapTo(): FinancialRequest {
      throw new ApplicationError(GetLastReceiptInfoQuery.name,this.mapTo.name,'Not Implemented Yet')
    }
  
    get url(): string {
      return  "/bursary/FinancialRequest/GetLastReceiptInfo";
    }
  
    get validationRules(): ValidationRule[] {
      return [];
    }
  }
  @MediatorHandler(GetLastReceiptInfoQueryHandler.name)
  export class GetLastReceiptInfoQueryHandler implements IRequestHandler<GetLastReceiptInfoQuery, FinancialRequest> {
    constructor(
      @Inject(HttpService) private _httpService:HttpService
    ) {
    }
    async Handle(request: GetLastReceiptInfoQuery) : Promise<FinancialRequest> {
      let httpRequest = new HttpRequest<GetLastReceiptInfoQuery>(request.url,request);
   
  
      return await this._httpService.Get<FinancialRequest>(httpRequest).toPromise().then(res => {
        return  res ;//"واریز کننده :"+ res.creditAccountReferenceTitle  +" *** " + " مبلغ : "+ res.amount + " *** " + " سند : " +res.voucherHeadCode;
      });
    }
  }