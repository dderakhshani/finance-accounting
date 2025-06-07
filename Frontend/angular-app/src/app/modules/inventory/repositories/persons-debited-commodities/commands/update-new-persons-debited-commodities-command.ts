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
export class UpdateNewPersonsDebitedCommoditiesCommand extends IRequest<UpdateNewPersonsDebitedCommoditiesCommand, PersonsDebitedCommodities> {


  public id: number | undefined = undefined;
  public debitAccountReferenceId: number | undefined = undefined;
  public debitAccountReferenceGroupId: number | undefined = undefined;
  public commodityTitle: string | undefined = undefined;
  public commodityCode: string | undefined = undefined;
  public debitReferenceTitle: string | undefined = undefined;
  public description: string | undefined = undefined;
  
  public createDate: Date | undefined = undefined;
  public attachmentAssets: AttachmentAssets[] = [];
  constructor() {
    super();
  }

  mapFrom(entity: PersonsDebitedCommodities): UpdateNewPersonsDebitedCommoditiesCommand {
    this.mapBasics(entity, this);

    return this;
  }

  mapTo(): PersonsDebitedCommodities {

    throw new ApplicationError(UpdateNewPersonsDebitedCommoditiesCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/PersonsDebitedCommodities/UpdateNewPersonsDebited";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateNewPersonsDebitedCommoditiesCommandHandler.name)
export class UpdateNewPersonsDebitedCommoditiesCommandHandler implements IRequestHandler<UpdateNewPersonsDebitedCommoditiesCommand, PersonsDebitedCommodities> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateNewPersonsDebitedCommoditiesCommand): Promise<PersonsDebitedCommodities> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateNewPersonsDebitedCommoditiesCommand> = new HttpRequest<UpdateNewPersonsDebitedCommoditiesCommand>(request.url, request);




    return await this._httpService.Post<UpdateNewPersonsDebitedCommoditiesCommand, ServiceResult<PersonsDebitedCommodities>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
