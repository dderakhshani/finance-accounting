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

export class UpdateLanguageCommand extends IRequest<UpdateLanguageCommand, Language> {

  public id:number | undefined = undefined;
  public title:string | undefined = undefined;
  public seoCode:string | undefined = undefined;
  public culture:string | undefined = undefined;
  public flagImageUrl:string | undefined = undefined;
  public directionBaseId:number | undefined = undefined;
  public defaultCurrencyBaseId:number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: Language): UpdateLanguageCommand {
    this.mapBasics(entity,this)
    return this;
  }

  mapTo(): Language {
    throw new ApplicationError(UpdateLanguageCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/language/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateLanguageCommandHandler.name)
export class UpdateLanguageCommandHandler implements IRequestHandler<UpdateLanguageCommand, Language> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateLanguageCommand): Promise<Language> {
    let httpRequest: HttpRequest<UpdateLanguageCommand> = new HttpRequest<UpdateLanguageCommand>(request.url, request);

    return await this._httpService.Put<UpdateLanguageCommand, ServiceResult<Language>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
