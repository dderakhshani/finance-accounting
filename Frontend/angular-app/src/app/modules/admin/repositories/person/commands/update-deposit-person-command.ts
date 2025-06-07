import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {Person} from "../../../entities/person";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {environment} from "../../../../../../environments/environment";
import {UpdatePersonAddressCommand} from "../../person-address/commands/update-person-address-command";
import {UpdatePersonPhoneCommand} from "../../person-phones/commands/update-person-phone-command";
import {UpdatePersonBankAccountCommand} from "../../person-bank-accounts/commands/update-person-bank-account-command";
import {PersonCustomer} from "../../../entities/person-customer";

export class UpdateDepositPersonCommand extends IRequest<UpdateDepositPersonCommand, Person> {
  

    public accountReferences:string[] = [];
  constructor() {
    super();
  }

  mapFrom(entity: Person): UpdateDepositPersonCommand {
    this.mapBasics(entity,this);
 
    return this;
  }

  mapTo(): Person {
    throw new ApplicationError(UpdateDepositPersonCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/person/SetDepositIdForPeopleByReferenceCode";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateDepositPersonCommandHandler.name)
export class UpdateDepositPersonCommandHandler implements IRequestHandler<UpdateDepositPersonCommand, Person> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateDepositPersonCommand): Promise<Person> {
    let httpRequest: HttpRequest<UpdateDepositPersonCommand> = new HttpRequest<UpdateDepositPersonCommand>(request.url, request);

    return await this._httpService.Put<UpdateDepositPersonCommand, ServiceResult<Person>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
