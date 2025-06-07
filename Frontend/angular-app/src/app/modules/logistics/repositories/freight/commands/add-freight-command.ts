import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {Freight} from "../../../entities/freight";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpService} from "../../../../../core/services/http/http.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ServiceResult} from "../../../../../core/models/service-result";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {Inject} from "@angular/core";

export class AddFreightCommand extends IRequest<AddFreightCommand, Freight>{


  public personId: number|undefined= undefined
  public isActive:boolean|undefined= undefined
  public isBlock: boolean|undefined= undefined
  public rate:number|undefined= undefined
  public managerFullName: string|undefined= undefined
  public descriptions: string|undefined= undefined;

  mapFrom(entity: Freight): AddFreightCommand {
    return new AddFreightCommand();
    new Freight();
  }

  mapTo(): Freight {
    return new Freight();
  }

  get url(): string {
    return "/logistics/freight/add";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}
@MediatorHandler(AddFreightCommandHandler.name)
export class AddFreightCommandHandler implements IRequestHandler<AddFreightCommand,Freight>{
  constructor(
    @Inject(HttpService) private httpService :HttpService
  ) {
  }
  Handle(request: AddFreightCommand): Promise<Freight> {
let httpRequest = new HttpRequest(request.url,request)
    return this.httpService.Post<AddFreightCommand,ServiceResult<Freight>>(httpRequest).toPromise().then(res => {
      return res.objResult
    })
  }

}
