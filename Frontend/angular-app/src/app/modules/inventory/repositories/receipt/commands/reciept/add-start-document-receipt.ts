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



export class CreateStartDocumentCommand extends IRequest<CreateStartDocumentCommand, Receipt> {
  
  public tags: string | undefined = undefined;
  public documentDescription: number | undefined = undefined;
  public codeVoucherGroupId: number | undefined = undefined;
  public warehouseId: number | undefined = undefined;
  public documentDate: Date | undefined = undefined;
  public creditAccountReferenceId: number | undefined = undefined;
  public creditAccountReferenceGroupId: number | undefined = undefined;
  public documentStauseBaseValue: number | undefined = undefined;
  public viewId: number | undefined = undefined;
  public documentNo: number | undefined = undefined;
  public yearId: number | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: Receipt): CreateStartDocumentCommand {

    return this;
  }

  mapTo(): Receipt {

    throw new ApplicationError(CreateStartDocumentCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/inventory/Receipt/CreateStartDocument";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateStartDocumentCommandHandler.name)
export class CreateStartDocumentCommandHandler implements IRequestHandler<CreateStartDocumentCommand, Receipt> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateStartDocumentCommand): Promise<Receipt> {
    this._notificationService.isLoader = true;
    let httpRequest: HttpRequest<CreateStartDocumentCommand> = new HttpRequest<CreateStartDocumentCommand>(request.url, request);


    return await this._httpService.Post<CreateStartDocumentCommand, ServiceResult<Receipt>>(httpRequest).toPromise().then(response => {

       this._notificationService.showSuccessMessage()
      return response.objResult
    }).finally(() => {
      this._notificationService.isLoader = false;
    })

  }
}
