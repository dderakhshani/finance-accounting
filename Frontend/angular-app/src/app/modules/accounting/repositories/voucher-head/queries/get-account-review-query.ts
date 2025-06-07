import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {ValidationRule} from "../../../../../core/validation/validation-rule";

export class GetAccountReviewQuery extends IRequest<GetAccountReviewQuery,any> {

  mapFrom(entity: any): GetAccountReviewQuery {
    return new GetAccountReviewQuery();
  }

  mapTo(): any {
    return undefined;
  }

  get url(): string {
    return "";
  }

  get validationRules(): ValidationRule[] {
    return [];
  }

}


export class GetAccountReviewQueryHandler implements IRequestHandler<GetAccountReviewQuery,any>{
  async Handle(request: GetAccountReviewQuery) : Promise<any> {

  };

}
