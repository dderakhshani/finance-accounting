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

export class UpdatePersonCommand extends IRequest<UpdatePersonCommand, Person> {
  public id: number | undefined = undefined;
  public nationalNumber: string | undefined = undefined;
  public economicCode: string | undefined = undefined;
  public firstName: string | undefined = undefined;
  public lastName: string | undefined = undefined;
  public fatherName: string | undefined = undefined;

  public email: string | undefined = undefined;

  public identityNumber: string | undefined = undefined;
  public insuranceNumber: string | undefined = undefined;

  public birthDate: Date | undefined = undefined;
  public birthPlaceCountryDivisionId: number | undefined = undefined;

  public genderBaseId: number | undefined = undefined;
  public legalBaseId: number | undefined = undefined;
  public governmentalBaseId: number | undefined = undefined;
  public taxIncluded:boolean | undefined = undefined;

  public accountReferenceId: string | undefined = undefined;
  public accountReferenceCode: string | undefined = undefined;

  public profileImageReletiveAddress: string | undefined = undefined;
  public signatureImageReletiveAddress: string | undefined = undefined;
  public accountReferenceGroupId: number | undefined = undefined;
  public workshopCode: string | undefined = undefined;

  public photoUrl: string | undefined = '/assets/images/avatar-placeholder.jpg';
  public signatureUrl: string | undefined = '/assets/images/signature-placeholder.jpg';

  public personAddressList:UpdatePersonAddressCommand[] = []
  public personPhonesList:UpdatePersonPhoneCommand[] = []
  public personBankAccountsList:UpdatePersonBankAccountCommand[] = []
  public personCustomer:PersonCustomer | undefined = undefined;
  constructor() {
    super();
  }

  mapFrom(entity: Person): UpdatePersonCommand {
    this.mapBasics(entity,this);
    if(entity.photoURL) this.photoUrl = environment.apiURL + "/" + entity.photoURL;
    if(entity.signatureURL) this.signatureUrl = environment.apiURL + "/" + entity.signatureURL;

    this.profileImageReletiveAddress = entity.photoURL;
    this.signatureImageReletiveAddress = entity.signatureURL;
    this.personAddressList = entity.personAddressList ? entity.personAddressList?.map(x => new UpdatePersonAddressCommand().mapFrom(x)) : [];
    this.personPhonesList = entity.personPhonesList ? entity.personPhonesList?.map(x => new UpdatePersonPhoneCommand().mapFrom(x)) : [];
    this.personBankAccountsList = entity.personBankAccountsList ? entity.personBankAccountsList?.map(x => new UpdatePersonBankAccountCommand().mapFrom(x)) : [];
    return this;
  }

  mapTo(): Person {
    throw new ApplicationError(UpdatePersonCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/person/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdatePersonCommandHandler.name)
export class UpdatePersonCommandHandler implements IRequestHandler<UpdatePersonCommand, Person> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdatePersonCommand): Promise<Person> {
    let httpRequest: HttpRequest<UpdatePersonCommand> = new HttpRequest<UpdatePersonCommand>(request.url, request);

    return await this._httpService.Put<UpdatePersonCommand, ServiceResult<Person>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
