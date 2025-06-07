import {IRequest, IRequestHandler} from "../../../../core/services/mediator/interfaces";
import {Attachment} from "../../entities/attachment";
import {MediatorHandler} from "../../../../core/services/mediator/decorator";
import {HttpRequest} from "../../../../core/services/http/http-request";
import {Inject} from "@angular/core";
import {HttpService} from "../../../../core/services/http/http.service";
import {NotificationService} from "../../../../shared/services/notification/notification.service";
import {ServiceResult} from "../../../../core/models/service-result";


export class DeleteAttachmentCommand extends IRequest<DeleteAttachmentCommand, Attachment>{
  constructor(public entityId:number, public archiveId?:number) {
    super();
  }

  mapFrom(entity: Attachment): DeleteAttachmentCommand {
    throw new Error("Method not implemented.");
  }
  mapTo(): Attachment {
    throw new Error("Method not implemented.");
  }
  get url(): string {
    return "/fileTransfer/fileTransfer/DeleteAttachment";
  }
  get validationRules(): any[] {
    return [];
  }
}
@MediatorHandler(DeleteAttachmentCommandHandler.name)
export class DeleteAttachmentCommandHandler implements IRequestHandler<DeleteAttachmentCommand, Attachment> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: DeleteAttachmentCommand): Promise<Attachment> {
    let httpRequest: HttpRequest<DeleteAttachmentCommand> = new HttpRequest<DeleteAttachmentCommand>(request.url, request);
    httpRequest.Query  += `id=${request.entityId}`;
    if(request.archiveId) httpRequest.Query += `&archiveId=${request.archiveId}`

    return await this._httpService.Delete<ServiceResult<Attachment>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
