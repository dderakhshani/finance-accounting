import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {CorrectionRequest} from "../../../entities/correction-request";

export class CreateCorrectionRequestCommand extends IRequest<CreateCorrectionRequestCommand, CorrectionRequest> {

  constructor() {
    super();
  }

  mapFrom(entity: CorrectionRequest): CreateCorrectionRequestCommand {
    throw new ApplicationError(CreateCorrectionRequestCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): CorrectionRequest {
    throw new ApplicationError(CreateCorrectionRequestCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/correctionRequest/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(CreateCorrectionRequestCommandHandler.name)
export class CreateCorrectionRequestCommandHandler implements IRequestHandler<CreateCorrectionRequestCommand, CorrectionRequest> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: CreateCorrectionRequestCommand): Promise<CorrectionRequest> {
    let httpRequest: HttpRequest<CreateCorrectionRequestCommand> = new HttpRequest<CreateCorrectionRequestCommand>(request.url, request);
    // httpRequest.Query  += ;
    // httpRequest.Headers = httpRequest.Headers.append()

    return await this._httpService.Post<CreateCorrectionRequestCommand, ServiceResult<CorrectionRequest>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })

  }
}
