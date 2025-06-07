import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";
import { MakeProductPrice } from "../../../../entities/makeProductPrice";



export class CreateMakeProductPriceCommand extends IRequest<CreateMakeProductPriceCommand, MakeProductPrice> {
  
  fromDate: Date | undefined = undefined;
  toDate: Date | undefined = undefined;
 
  constructor() {
    super();
  }

  mapFrom(entity: MakeProductPrice): CreateMakeProductPriceCommand {

    return this;
  }

  mapTo(): MakeProductPrice {

    throw new ApplicationError(CreateMakeProductPriceCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/CreateMakeProductPrice";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateMakeProductPriceCommandHandler.name)
export class CreateMakeProductPriceCommandHandler implements IRequestHandler<CreateMakeProductPriceCommand, MakeProductPrice> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateMakeProductPriceCommand): Promise<MakeProductPrice> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<CreateMakeProductPriceCommand> = new HttpRequest<CreateMakeProductPriceCommand>(request.url, request);


    return await this._httpService.Post<CreateMakeProductPriceCommand, ServiceResult<MakeProductPrice>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
