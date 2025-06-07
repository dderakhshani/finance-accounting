import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {Language} from "../../../entities/language";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class CreateLanguageCommand extends IRequest<CreateLanguageCommand, Language> {

  public title:string | undefined = undefined;
  public seoCode:string | undefined = undefined;
  public culture:string | undefined = undefined;
  public flagImageUrl:string | undefined = undefined;
  public directionBaseId:number | undefined = undefined;
  public defaultCurrencyBaseId:number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: Language): CreateLanguageCommand {
    throw new ApplicationError(CreateLanguageCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Language {
    throw new ApplicationError(CreateLanguageCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/language/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateLanguageCommandHandler.name)
export class CreateLanguageCommandHandler implements IRequestHandler<CreateLanguageCommand, Language> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateLanguageCommand): Promise<Language> {
    let httpRequest: HttpRequest<CreateLanguageCommand> = new HttpRequest<CreateLanguageCommand>(request.url, request);

    return await this._httpService.Post<CreateLanguageCommand, ServiceResult<Language>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
