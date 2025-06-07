import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {PersonCustomer} from "../../../entities/person-customer";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class UpdatePersonCustomerCommand extends IRequest<UpdatePersonCustomerCommand, PersonCustomer> {
  public id: number | undefined = undefined;
  public personId!: number;
  public customerTypeBaseId!: number;
  public customerCode!: string;
  public curentExpertId!: number;
  public economicCode!: string;
  public description!: string;

  constructor() {
    super();
  }

  mapFrom(entity: PersonCustomer): UpdatePersonCustomerCommand {
    this.mapBasics(entity,this)
    return this;
  }

  mapTo(): PersonCustomer {
    throw new ApplicationError(UpdatePersonCustomerCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personCustomers/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdatePersonCustomerCommandHandler.name)
export class UpdatePersonCustomerCommandHandler implements IRequestHandler<UpdatePersonCustomerCommand, PersonCustomer> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,

  ) {
  }

  async Handle(request: UpdatePersonCustomerCommand): Promise<PersonCustomer> {
    let httpRequest: HttpRequest<UpdatePersonCustomerCommand> = new HttpRequest<UpdatePersonCustomerCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Put<UpdatePersonCustomerCommand, ServiceResult<PersonCustomer>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
