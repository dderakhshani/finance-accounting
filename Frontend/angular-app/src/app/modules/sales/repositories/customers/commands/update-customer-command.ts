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
import {PersonAddress} from "../../../../admin/entities/person-address";
import {PersonPhone} from "../../../../admin/entities/person-phone";
import {PersonBankAccount} from "../../../../admin/entities/person-bank-account";

export class UpdateCustomerCommand extends IRequest<UpdateCustomerCommand, Customer> {



  public id: number | undefined = undefined
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

  mapFrom(entity: Customer): UpdateCustomerCommand {
    this.mapBasics(entity, this);
    return this;
  }

  mapTo(): Customer {
    throw new ApplicationError(UpdateCustomerCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/sale/customers/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateCustomerCommandHandler.name)
export class UpdateCustomerCommandHandler implements IRequestHandler<UpdateCustomerCommand, Customer> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private notificationService: NotificationService
  ) {
  }

  async Handle(request: UpdateCustomerCommand): Promise<Customer> {
    let httpRequest: HttpRequest<UpdateCustomerCommand> = new HttpRequest<UpdateCustomerCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Put<UpdateCustomerCommand, ServiceResult<Customer>>(httpRequest).toPromise().then(response => {
      this.notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
