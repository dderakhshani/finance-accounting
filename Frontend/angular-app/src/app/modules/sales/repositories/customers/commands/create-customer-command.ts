import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Customer} from "../../../entities/Customer";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class CreateCustomerCommand extends IRequest<CreateCustomerCommand, Customer> {

  public personId: number | undefined = undefined
  public accountReferenceGroupId: number | undefined = undefined
  public customerTypeBaseId: number | undefined = undefined
  public customerCode: string | undefined = undefined
  public currentAgentId: number | undefined = undefined
  public economicCode: string | undefined = undefined
  public description: string | undefined = undefined
  public isActive: boolean | undefined = undefined

  constructor() {
    super();
  }

  mapFrom(entity: Customer): CreateCustomerCommand {
    throw new ApplicationError(CreateCustomerCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Customer {
    throw new ApplicationError(CreateCustomerCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/sale/customers/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateCustomerCommandHandler.name)
export class CreateCustomerCommandHandler implements IRequestHandler<CreateCustomerCommand, Customer> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private notificationService: NotificationService

  ) {
  }

  async Handle(request: CreateCustomerCommand): Promise<Customer> {
    let httpRequest: HttpRequest<CreateCustomerCommand> = new HttpRequest<CreateCustomerCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateCustomerCommand, ServiceResult<Customer>>(httpRequest).toPromise().then(response => {
      this.notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
