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

export class DeleteLanguageCommand extends IRequest<DeleteLanguageCommand, Language> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: Language): DeleteLanguageCommand {
    throw new ApplicationError(DeleteLanguageCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Language {
    throw new ApplicationError(DeleteLanguageCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/language/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteLanguageCommandHandler.name)
export class DeleteLanguageCommandHandler implements IRequestHandler<DeleteLanguageCommand, Language> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteLanguageCommand): Promise<Language> {
    let httpRequest: HttpRequest<DeleteLanguageCommand> = new HttpRequest<DeleteLanguageCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Delete<ServiceResult<Language>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
