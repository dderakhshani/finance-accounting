import { User } from "../../../entities/user";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";

export class DeleteUserCommand extends IRequest<DeleteUserCommand,User>{
  mapFrom(entity: User): DeleteUserCommand {
    return new DeleteUserCommand;
  }

  mapTo(): User {
    return new User();
  }

  get url(): string {
    return "/admin/User/delete";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }
}
@MediatorHandler(UpdateUsanceCommandHandler.name)
export class UpdateUsanceCommandHandler implements IRequestHandler<DeleteUserCommand,any>{
  Handle(request: DeleteUserCommand): Promise<any> {
    return Promise.resolve(undefined);
  }

}
