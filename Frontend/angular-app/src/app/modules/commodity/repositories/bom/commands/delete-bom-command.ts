import {Inject} from "@angular/core";
import Bom from "../../../entities/bom";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";

export class DeleteBomCommand extends IRequest<DeleteBomCommand, Bom> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: Bom): DeleteBomCommand {
    throw new ApplicationError(DeleteBomCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): Bom {
    throw new ApplicationError(DeleteBomCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/bom/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteBomCommandHandler.name)
export class DeleteBomCommandHandler implements IRequestHandler<DeleteBomCommand, Bom> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: DeleteBomCommand): Promise<Bom> {
    let httpRequest: HttpRequest<DeleteBomCommand> = new HttpRequest<DeleteBomCommand>(request.url, request);

    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Delete<ServiceResult<Bom>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
