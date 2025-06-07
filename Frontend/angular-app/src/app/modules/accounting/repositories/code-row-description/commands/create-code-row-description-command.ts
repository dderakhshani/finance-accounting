import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {CodeRowDescription} from "../../../entities/code-row-description";



export class CreateCodeRowDescriptionCommand extends IRequest<CreateCodeRowDescriptionCommand, CodeRowDescription> {

  public companyId: number | undefined = undefined;
  public title: string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity:CodeRowDescription): CreateCodeRowDescriptionCommand {
    throw new ApplicationError(CreateCodeRowDescriptionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo():CodeRowDescription {
    throw new ApplicationError(CreateCodeRowDescriptionCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/accounting/codeRowDescription/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateCodeRowDescriptionCommandHandler.name)
export class CreateCodeRowDescriptionCommandHandler implements IRequestHandler<CreateCodeRowDescriptionCommand, CodeRowDescription> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateCodeRowDescriptionCommand): Promise<CodeRowDescription> {
    let httpRequest: HttpRequest<CreateCodeRowDescriptionCommand> = new HttpRequest<CreateCodeRowDescriptionCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateCodeRowDescriptionCommand, ServiceResult<CodeRowDescription>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })


  }
}
