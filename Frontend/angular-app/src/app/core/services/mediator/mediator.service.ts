import { Injectable } from '@angular/core';
import {IMediatorMiddleware, IRequest, IRequestHandler} from "./interfaces";
import {BaseMediator} from "./base-mediator";

@Injectable({
  providedIn: 'root'
})
export class Mediator extends BaseMediator{

  private middlewares: IMediatorMiddleware[] = [];

  public async send<HttpRequest extends object,TResponse>(request: IRequest<HttpRequest,TResponse>): Promise<TResponse> {
    let handler: IRequestHandler<any, any> = super.resolve(request.constructor.name);

    this.middlewares.forEach(m => m.PreProcess(request));

    try {
      if(handler.Validate)
        handler.Validate(request);
    } catch (ex) {
      throw ex;
    }

    if(handler.Log)
      handler.Log();

    let response: TResponse = await handler.Handle(request);


    this.middlewares.reverse().forEach(m => m.PostProcess(request, response));
    return response;
  }

  public use(middleware: IMediatorMiddleware): void {
    this.middlewares.push(middleware);
  }

}
