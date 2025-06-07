import {Container} from "./container";
import {ServiceLocator} from "../service-locator/service-locator";
import {IRequest, IRequestHandler} from "./interfaces";
import {ReflectiveInjector} from "@angular/core";

export abstract class BaseMediator {

  public abstract send<HttpRequest  extends object,TResponse>(request: IRequest<HttpRequest,TResponse>): Promise<TResponse>

  protected resolve(command: string): IRequestHandler<any,any> {
    let handlerClass: any = Container.Get(command);

    return ReflectiveInjector.resolveAndCreate([handlerClass],ServiceLocator.injector).get(handlerClass);
  }
}
