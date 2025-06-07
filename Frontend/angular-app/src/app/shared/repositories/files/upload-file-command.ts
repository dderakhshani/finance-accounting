import {Inject} from "@angular/core";
import {ServiceResult} from "../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../core/services/mediator/interfaces";
import {HttpService} from "../../../core/services/http/http.service";
import {ApplicationError} from "../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../core/services/mediator/decorator";
import {NotificationService} from "../../services/notification/notification.service";
import {ValidationRule} from "../../../core/validation/validation-rule";
import {HttpRequest} from "../../../core/services/http/http-request";

export class UploadFileCommand extends IRequest<UploadFileCommand, string> {
  public file: File | undefined = undefined;
  constructor(file:File) {
    super();
    this.file = file;
  }

  mapFrom(entity: string): UploadFileCommand {
    throw new ApplicationError(UploadFileCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): string {
    throw new ApplicationError(UploadFileCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/fileTransfer/fileTransfer/upload";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UploadFileCommandHandler.name)
export class UploadFileCommandHandler implements IRequestHandler<UploadFileCommand, string> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UploadFileCommand): Promise<string> {
    let httpRequest: HttpRequest<UploadFileCommand> = new HttpRequest<UploadFileCommand>(request.url, request);
    httpRequest.BodyFormat = 'form';
    return await this._httpService.Post<File, ServiceResult<string>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
