import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {Person} from "../../../entities/person";

export class CreatePersonCommand extends IRequest<CreatePersonCommand, Person> {

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

  public accountReferenceCode: string | undefined = undefined;
  public profileImageReletiveAddress: string | undefined = undefined;
  public signatureImageReletiveAddress: string | undefined = undefined;
  public accountReferenceGroupId: number | undefined = undefined;
  public workshopCode: string | undefined = undefined;

  public photoUrl: string | undefined = '/assets/images/avatar-placeholder.jpg';
  public signatureUrl: string | undefined = '/assets/images/signature-placeholder.jpg';

  constructor() {
    super();
  }

  mapFrom(entity: Person): CreatePersonCommand {
    throw new ApplicationError(CreatePersonCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Person {
    throw new ApplicationError(CreatePersonCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/person/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreatePersonCommandHandler.name)
export class CreatePersonCommandHandler implements IRequestHandler<CreatePersonCommand, Person> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreatePersonCommand): Promise<Person> {
    let httpRequest: HttpRequest<CreatePersonCommand> = new HttpRequest<CreatePersonCommand>(request.url, request);

    return await this._httpService.Post<CreatePersonCommand, ServiceResult<Person>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
