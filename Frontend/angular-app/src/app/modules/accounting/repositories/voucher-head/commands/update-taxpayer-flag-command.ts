import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {TaxpayerFlagUpdateModel, TaxpayerFlag} from "../../../entities/TaxpayerFlag";

export class UpdateTaxpayerFlagCommand extends IRequest<UpdateTaxpayerFlagCommand, any> {
  public saveChanges !: boolean;
  public menueId !: number ;
  public taxpayerFlags !:  TaxpayerFlagUpdateModel[];
  constructor() {
    super();
  }

  mapFrom(entity: any): UpdateTaxpayerFlagCommand {
    this.mapBasics(entity,this)
    return this;
  }

  mapTo(): TaxpayerFlag {
    throw new ApplicationError(UpdateTaxpayerFlagCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/VouchersHead/UpdateTaxpayerFlag";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateTaxpayerFlagCommandHandler.name)
export class UpdateTaxpayerFlagCommandHandler implements IRequestHandler<UpdateTaxpayerFlagCommand, TaxpayerFlag> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService
  ) {
  }

  async Handle(request: UpdateTaxpayerFlagCommand): Promise<TaxpayerFlag> {
    let httpRequest: HttpRequest<UpdateTaxpayerFlagCommand> = new HttpRequest<UpdateTaxpayerFlagCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<UpdateTaxpayerFlagCommand, ServiceResult<TaxpayerFlag>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
