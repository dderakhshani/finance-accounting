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

export class UpdateExtraCostCommand extends IRequest<UpdateExtraCostCommand, Receipt> {


  public id: number | undefined = undefined; 
 
  public isFreightChargePaid: boolean | undefined = false;
  public extraCost: number | undefined = undefined;
  
  constructor() {
    super();
  }

  mapFrom(entity: Receipt): UpdateExtraCostCommand {

    this.mapBasics(entity, this)
    
    return this;
  }

  mapTo(): Receipt {

    throw new ApplicationError(UpdateExtraCostCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/UpdateExtraCost";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateExtraCostCommandHandler.name)
export class UpdateExtraCostCommandHandler implements IRequestHandler<UpdateExtraCostCommand, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateExtraCostCommand): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateExtraCostCommand> = new HttpRequest<UpdateExtraCostCommand>(request.url, request);


    return await this._httpService.Post<UpdateExtraCostCommand, ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
