import {Inject} from "@angular/core";
import {Quantities, Receipt} from "../../../../entities/receipt";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";


export class SplitCommodityQuantityCommand extends IRequest<SplitCommodityQuantityCommand, SplitCommodityQuantityCommand> {



  public documentItemId: number | undefined = undefined;
  public quantities: Quantities[] |undefined= [];
  
  constructor() {
    super();
  }

  mapFrom(entity: SplitCommodityQuantityCommand): SplitCommodityQuantityCommand {

    this.mapBasics(entity, this)

    return this;
  }

  mapTo(): SplitCommodityQuantityCommand {

    throw new ApplicationError(SplitCommodityQuantityCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/SplitCommodityQuantity";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(SplitCommodityQuantityCommandHandler.name)
export class SplitCommodityQuantityCommandHandler implements IRequestHandler<SplitCommodityQuantityCommand, SplitCommodityQuantityCommand> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: SplitCommodityQuantityCommand): Promise<SplitCommodityQuantityCommand> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<SplitCommodityQuantityCommand> = new HttpRequest<SplitCommodityQuantityCommand>(request.url, request);


    return await this._httpService.Post<SplitCommodityQuantityCommand, ServiceResult<SplitCommodityQuantityCommand>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
