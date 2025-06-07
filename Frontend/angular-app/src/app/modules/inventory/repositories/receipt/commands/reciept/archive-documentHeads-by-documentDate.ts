import {Inject} from "@angular/core";
import {Receipt} from "../../../../entities/receipt";
import {ServiceResult} from "../../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {ValidationRule} from "../../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../../core/services/http/http-request";



export class ArchiveDocumentHeadsByDocumentDateCommand extends IRequest<ArchiveDocumentHeadsByDocumentDateCommand, Receipt> {
  

  public warehouseId: number = 1;
  public fromDate: Date | undefined = undefined;
  public toDate: Date | undefined = undefined;
  public documentStatuesBaseValue: number | undefined = undefined;
  

  constructor() {
    super();
  }

  mapFrom(entity: Receipt): ArchiveDocumentHeadsByDocumentDateCommand {

    return this;
  }

  mapTo(): Receipt {

    throw new ApplicationError(ArchiveDocumentHeadsByDocumentDateCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/ArchiveDocumentHeadsByDocumentDate";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(ArchiveDocumentHeadsByDocumentDateCommandHandler.name)
export class ArchiveDocumentHeadsByDocumentDateCommandHandler implements IRequestHandler<ArchiveDocumentHeadsByDocumentDateCommand, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: ArchiveDocumentHeadsByDocumentDateCommand): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<ArchiveDocumentHeadsByDocumentDateCommand> = new HttpRequest<ArchiveDocumentHeadsByDocumentDateCommand>(request.url, request);


    return await this._httpService.Post<ArchiveDocumentHeadsByDocumentDateCommand, ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
