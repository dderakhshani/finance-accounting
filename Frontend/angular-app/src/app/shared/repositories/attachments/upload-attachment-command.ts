import {Inject} from "@angular/core";
import {ServiceResult} from "../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../core/services/mediator/interfaces";
import {HttpService} from "../../../core/services/http/http.service";
import {ApplicationError} from "../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../core/services/mediator/decorator";

import {NotificationService} from "../../services/notification/notification.service";
import {ValidationRule} from "../../../core/validation/validation-rule";
import {HttpRequest} from "../../../core/services/http/http-request";

export class UploadAttachmentCommand extends IRequest<UploadAttachmentCommand, string> {




  constructor(public file:File) {
    super();
  }

  mapFrom(entity: string): UploadAttachmentCommand {
    throw new ApplicationError(UploadAttachmentCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): string {
    throw new ApplicationError(UploadAttachmentCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/fileTransfer/fileTransfer/upload";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(UploadAttachmentCommandHandler.name)
export class UploadAttachmentCommandHandler implements IRequestHandler<UploadAttachmentCommand, string> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: UploadAttachmentCommand): Promise<string> {
    let httpRequest: HttpRequest<UploadAttachmentCommand> = new HttpRequest<UploadAttachmentCommand>(request.url, request);
    httpRequest.BodyFormat="form";
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<UploadAttachmentCommand, ServiceResult<string>>(httpRequest).toPromise().then(response => {
      // this._notificationService.showHttpSuccessMessage()
      return response.objResult
    })

  }
}
