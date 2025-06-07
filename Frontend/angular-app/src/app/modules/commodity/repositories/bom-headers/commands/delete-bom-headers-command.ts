import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import { BomHeader } from "../../../entities/boms-header";

export class DeleteBomHeadersCommand extends IRequest<DeleteBomHeadersCommand, BomHeader> {

  constructor(public entityId:number) {
    super();
  }

  mapFrom(entity: BomHeader): DeleteBomHeadersCommand {
    throw new ApplicationError(DeleteBomHeadersCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): BomHeader {
    throw new ApplicationError(DeleteBomHeadersCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/commodity/bomValueHeader/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}

@MediatorHandler(DeleteBomHeadersCommandHandler.name)
export class DeleteBomHeadersCommandHandler implements IRequestHandler<DeleteBomHeadersCommand, BomHeader> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
  ) {
  }

  async Handle(request: DeleteBomHeadersCommand): Promise<BomHeader> {
    let httpRequest: HttpRequest<DeleteBomHeadersCommand> = new HttpRequest<DeleteBomHeadersCommand>(request.url, request);

    httpRequest.Query  += `id=${request.entityId}`;

    return await this._httpService.Delete<ServiceResult<BomHeader>>(httpRequest).toPromise().then(response => {
      return response.objResult
    })
  }
}
