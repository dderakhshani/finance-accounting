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
export class UpdateReturnToWarehousePersonsDebitedCommand extends IRequest<UpdateReturnToWarehousePersonsDebitedCommand, PersonsDebitedCommodities> {


  public id: number | undefined = undefined;
  public warehouseId: number | undefined = undefined;
  public commodityTitle: string | undefined = undefined;
  public commodityCode: string | undefined = undefined;
  public fullName: string | undefined = undefined;
  public createDate: Date | undefined = undefined;
  public description: string | undefined = undefined;
  constructor() {
    super();
  }

  mapFrom(entity: PersonsDebitedCommodities): UpdateReturnToWarehousePersonsDebitedCommand {
    this.mapBasics(entity, this);

    return this;
  }

  mapTo(): PersonsDebitedCommodities {

    throw new ApplicationError(UpdateReturnToWarehousePersonsDebitedCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/Inventory/PersonsDebitedCommodities/UpdateReturnToWarehouseDebited";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateReturnToWarehousePersonsDebitedCommandHandler.name)
export class UpdateReturnToWarehousePersonsDebitedCommandHandler implements IRequestHandler<UpdateReturnToWarehousePersonsDebitedCommand, PersonsDebitedCommodities> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateReturnToWarehousePersonsDebitedCommand): Promise<PersonsDebitedCommodities> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<UpdateReturnToWarehousePersonsDebitedCommand> = new HttpRequest<UpdateReturnToWarehousePersonsDebitedCommand>(request.url, request);




    return await this._httpService.Post<UpdateReturnToWarehousePersonsDebitedCommand, ServiceResult<PersonsDebitedCommodities>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })
  }
}
