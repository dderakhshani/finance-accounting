import {Inject} from "@angular/core";
import {CodeRowDescription} from "../../../entities/code-row-description";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class DeleteCodeRowDescriptionCommand extends IRequest<DeleteCodeRowDescriptionCommand, CodeRowDescription> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: CodeRowDescription): DeleteCodeRowDescriptionCommand {
    throw new ApplicationError(DeleteCodeRowDescriptionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CodeRowDescription {
    throw new ApplicationError(DeleteCodeRowDescriptionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/codeRowDescription/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteCodeRowDescriptionCommandHandler.name)
export class DeleteCodeRowDescriptionCommandHandler implements IRequestHandler<DeleteCodeRowDescriptionCommand, CodeRowDescription> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteCodeRowDescriptionCommand): Promise<CodeRowDescription> {
    let httpRequest: HttpRequest<DeleteCodeRowDescriptionCommand> = new HttpRequest<DeleteCodeRowDescriptionCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;


    return await this._httpService.Delete<ServiceResult<CodeRowDescription>>(httpRequest).toPromise().then(response => {
       this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
