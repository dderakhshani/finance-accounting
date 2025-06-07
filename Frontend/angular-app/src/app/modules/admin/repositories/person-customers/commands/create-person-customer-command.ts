import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {PersonCustomer} from "../../../entities/person-customer";

export class CreatePersonCustomerCommand extends IRequest<CreatePersonCustomerCommand, PersonCustomer> {

  public personId!: number;
  public customerTypeBaseId!: number;
  public customerCode!: string;
  public curentExpertId!: number;
  public economicCode!: string;
  public description!: string;

  constructor() {
    super();
  }

  mapFrom(entity: PersonCustomer): CreatePersonCustomerCommand {
    throw new ApplicationError(CreatePersonCustomerCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): PersonCustomer {
    throw new ApplicationError(CreatePersonCustomerCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/personCustomers/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreatePersonCustomerCommandHandler.name)
export class CreatePersonCustomerCommandHandler implements IRequestHandler<CreatePersonCustomerCommand, PersonCustomer> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,

  ) {
  }

  async Handle(request: CreatePersonCustomerCommand): Promise<PersonCustomer> {
    let httpRequest: HttpRequest<CreatePersonCustomerCommand> = new HttpRequest<CreatePersonCustomerCommand>(request.url, request);


    return await this._httpService.Post<CreatePersonCustomerCommand, ServiceResult<PersonCustomer>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
