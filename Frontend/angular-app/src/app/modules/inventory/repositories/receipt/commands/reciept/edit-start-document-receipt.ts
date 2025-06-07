import {Inject} from "@angular/core";
import {Receipt} from "../../../../entities/receipt";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";



export class UpdateStartDocumentCommand extends IRequest<UpdateStartDocumentCommand, Receipt> {
  
  
  public commodityId: number | undefined = undefined;
  public warehouseId: number | undefined = undefined;
  public documentDate: Date | undefined = undefined;
  public unitPrice: number | undefined = undefined;
  public quantity: number | undefined = undefined;
  public yearId: number | undefined = undefined;

  
  constructor() {
    super();
  }

  mapFrom(entity: Receipt): UpdateStartDocumentCommand {

    return this;
  }

  mapTo(): Receipt {

    throw new ApplicationError(UpdateStartDocumentCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/UpdateStartDocument";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateStartDocumentCommandHandler.name)
export class UpdateStartDocumentCommandHandler implements IRequestHandler<UpdateStartDocumentCommand, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateStartDocumentCommand): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateStartDocumentCommand> = new HttpRequest<UpdateStartDocumentCommand>(request.url, request);


    return await this._httpService.Post<UpdateStartDocumentCommand, ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
