import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {VoucherHead} from "../../../entities/voucher-head";
import {HttpService} from "../../../../../core/services/http/http.service";
import {Inject} from "@angular/core";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class CreateStartVoucherHeadCommand extends IRequest<CreateStartVoucherHeadCommand,VoucherHead> {

  public yearId:number | undefined = undefined;
  public voucherNo:number | undefined = undefined;

  mapFrom(entity: any): CreateStartVoucherHeadCommand {
    return new CreateStartVoucherHeadCommand();
  }

  mapTo(): any {
    return undefined;
  }

  get url(): string {
    return "/accounting/vouchersHead/StartVoucherHead";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}

@MediatorHandler(CreateStartVoucherHeadCommandHandler.name)
export class CreateStartVoucherHeadCommandHandler implements IRequestHandler<CreateStartVoucherHeadCommand, VoucherHead>{
  constructor(
    @Inject(HttpService) private httpService:HttpService,
    @Inject(NotificationService)private _notificationService: NotificationService,
  ) {
  }
  async Handle (request: CreateStartVoucherHeadCommand) : Promise<any> {

    let httpRequest = new HttpRequest(request.url,request);
    return await this.httpService.Post<any,ServiceResult<any>>(httpRequest).toPromise().then(res => {
      if(res.succeed)
        this._notificationService.showSuccessMessage();
      else 
        this._notificationService.showFailureMessage(res.message);
        
        return; 
    })
  };
}
