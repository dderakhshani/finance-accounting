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

export class UpdateCodeRowDescriptionCommand extends IRequest<UpdateCodeRowDescriptionCommand, CodeRowDescription> {
  public id: number | undefined = undefined;
  public companyId: number| undefined = undefined;
  public title: string | undefined = undefined;
  constructor() {
    super();
  }

  mapFrom(entity: CodeRowDescription): UpdateCodeRowDescriptionCommand {
    this.mapBasics(entity,this);
    return this;
  }

  mapTo(): CodeRowDescription {
    throw new ApplicationError(UpdateCodeRowDescriptionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/codeRowDescription/update";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UpdateCodeRowDescriptionCommandHandler.name)
export class UpdateCodeRowDescriptionCommandHandler implements IRequestHandler<UpdateCodeRowDescriptionCommand, CodeRowDescription> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UpdateCodeRowDescriptionCommand): Promise<CodeRowDescription> {
    let httpRequest: HttpRequest<UpdateCodeRowDescriptionCommand> = new HttpRequest<UpdateCodeRowDescriptionCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()


    return await this._httpService.Put<UpdateCodeRowDescriptionCommand, ServiceResult<CodeRowDescription>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })

  }
}
