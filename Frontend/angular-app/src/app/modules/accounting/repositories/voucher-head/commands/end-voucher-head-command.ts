import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../../core/services/http/http.service";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {VoucherHead} from "../../../entities/voucher-head";

export class EndVoucherHeadCommand extends IRequest<EndVoucherHeadCommand, VoucherHead>{

  public replaceEndVoucherFlag:boolean | undefined = undefined;


  mapFrom(entity: any): EndVoucherHeadCommand {
    return new EndVoucherHeadCommand();
  }

  mapTo(): any {
  }

  get url(): string {
    return "/accounting/vouchersHead/EndVouchersHead";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }


}

@MediatorHandler(EndVoucherHeadCommandHandler.name)
export class EndVoucherHeadCommandHandler implements IRequestHandler<EndVoucherHeadCommand, VoucherHead> {

  constructor(
    @Inject(HttpService) private httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {}

  async Handle(request: EndVoucherHeadCommand): Promise<any> {
    let httpRequest = new HttpRequest(request.url, request);
      await this.httpService.Post<EndVoucherHeadCommand, ServiceResult<VoucherHead>>(httpRequest).toPromise().then(res => {
      if(res.succeed)
        this._notificationService.showSuccessMessage();
      else 
        this._notificationService.showFailureMessage(res.message);
        
        return; 
       
      
    })
  }
}
