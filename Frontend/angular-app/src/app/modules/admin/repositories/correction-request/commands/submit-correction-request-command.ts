import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";

export class SubmitCorrectionRequestCommand extends IRequest<SubmitCorrectionRequestCommand, boolean> {

  constructor(public id:number,public isAccepted:boolean) {
    super();
  }

  mapFrom(entity: boolean): SubmitCorrectionRequestCommand {
    throw new ApplicationError(SubmitCorrectionRequestCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): boolean {
    throw new ApplicationError(SubmitCorrectionRequestCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/correctionRequest/submit";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(SubmitCorrectionRequestCommandHandler.name)
export class SubmitCorrectionRequestCommandHandler implements IRequestHandler<SubmitCorrectionRequestCommand, boolean> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: SubmitCorrectionRequestCommand): Promise<boolean> {
    let httpRequest: HttpRequest<SubmitCorrectionRequestCommand> = new HttpRequest<SubmitCorrectionRequestCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Put<SubmitCorrectionRequestCommand, ServiceResult<boolean>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
