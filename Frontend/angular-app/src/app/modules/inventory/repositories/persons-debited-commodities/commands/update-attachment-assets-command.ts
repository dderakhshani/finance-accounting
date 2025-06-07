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
export class UpdateAttachmentAssetsCommand extends IRequest<UpdateAttachmentAssetsCommand, PersonsDebitedCommodities> {


  public id: number | undefined = undefined;
  public commodityTitle: string | undefined = undefined;
  public commodityCode: string | undefined = undefined;
  public fullName: string | undefined = undefined;
  public attachmentAssets: AttachmentAssets[] = [];
  constructor() {
    super();
  }

  mapFrom(entity: PersonsDebitedCommodities): UpdateAttachmentAssetsCommand {
    this.mapBasics(entity, this);

    return this;
  }

  mapTo(): PersonsDebitedCommodities {

    throw new ApplicationError(UpdateAttachmentAssetsCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/PersonsDebitedCommodities/UpdateAssetsAttachment";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateAttachmentAssetsCommandHandler.name)
export class UpdateAttachmentAssetsCommandHandler implements IRequestHandler<UpdateAttachmentAssetsCommand, AttachmentAssets> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateAttachmentAssetsCommand): Promise<AttachmentAssets> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateAttachmentAssetsCommand> = new HttpRequest<UpdateAttachmentAssetsCommand>(request.url, request);




    return await this._httpService.Post<UpdateAttachmentAssetsCommand, ServiceResult<AttachmentAssets>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
