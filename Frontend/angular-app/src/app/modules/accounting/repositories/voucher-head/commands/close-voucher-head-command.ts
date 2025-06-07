import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import { ApplicationError } from "src/app/core/exceptions/application-error";

export class CloseVoucherHeadCommand extends IRequest<CloseVoucherHeadCommand,any>{

  public ReplaceCloseVoucherFlag:boolean | undefined = undefined;
 
 

  mapFrom(entity: any): CloseVoucherHeadCommand {
      throw new ApplicationError(CloseVoucherHeadCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): any {
    throw new ApplicationError(CloseVoucherHeadCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/vouchersHead/closeVouchers";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CloseVoucherHeadCommandHandler.name)
export class CloseVoucherHeadCommandHandler implements IRequestHandler<CloseVoucherHeadCommand,any>{
  constructor(
    @Inject(HttpService) private _httpService:HttpService,
    @Inject(NotificationService) private _notificationService:NotificationService
  ) {
  }

  async Handle(request: CloseVoucherHeadCommand) {
    let httpRequest = new HttpRequest(request.url,request);
    return await this._httpService.Post<any,ServiceResult<any>>(httpRequest).toPromise().then(res => {
      if(res.succeed)
        this._notificationService.showSuccessMessage();
      else 
        this._notificationService.showFailureMessage(res.message);
        
        return; 
    })
  }
}
