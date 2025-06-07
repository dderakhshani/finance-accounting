import { Inject } from "@angular/core";
import { ServiceResult } from "../../../../../core/models/service-result";
import { IRequest, IRequestHandler } from "../../../../../core/services/mediator/interfaces";
import { HttpService } from "../../../../../core/services/http/http.service";
import { ApplicationError } from "../../../../../core/exceptions/application-error";
import { MediatorHandler } from "../../../../../core/services/mediator/decorator";
import { NotificationService } from "../../../../../shared/services/notification/notification.service";
import { ValidationRule } from "../../../../../core/validation/validation-rule";
import { HttpRequest } from "../../../../../core/services/http/http-request";
import { PersonsDebitedCommodities } from '../../../entities/persons-debited-commodities';
import { AttachmentAssets } from "../../../entities/attachment";
export class UpdateAssetSerialPersonsDebitedCommand extends IRequest<UpdateAssetSerialPersonsDebitedCommand, PersonsDebitedCommodities> {


  public id: number | undefined = undefined;
  public assetId: number | undefined = undefined;

  //-------------------------------------------------------
  public commodityTitle: string | undefined = undefined;
  public commodityCode: string | undefined = undefined;
  public fullName: string | undefined = undefined;
  public assetSerial: string | undefined = undefined;
  
  constructor() {
    super();
  }

  mapFrom(entity: PersonsDebitedCommodities): UpdateAssetSerialPersonsDebitedCommand {
    this.mapBasics(entity, this);

    return this;
  }

  mapTo(): PersonsDebitedCommodities {

    throw new ApplicationError(UpdateAssetSerialPersonsDebitedCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/PersonsDebitedCommodities/UpdateAssetSerialPersonsDebited";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateAssetSerialPersonsDebitedCommandHandler.name)
export class UpdateAssetSerialPersonsDebitedCommandHandler implements IRequestHandler<UpdateAssetSerialPersonsDebitedCommand, PersonsDebitedCommodities> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateAssetSerialPersonsDebitedCommand): Promise<PersonsDebitedCommodities> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateAssetSerialPersonsDebitedCommand> = new HttpRequest<UpdateAssetSerialPersonsDebitedCommand>(request.url, request);




    return await this._httpService.Post<UpdateAssetSerialPersonsDebitedCommand, ServiceResult<PersonsDebitedCommodities>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
