import { IContainer } from "./interfaces";
import {ApplicationError} from "../../exceptions/application-error";

export namespace Container {
  const container: IContainer = {
    handlers: {},
    registeredHandlers: {}
  };

  export function Register(message: string, callback: Function): void {
    container.handlers[message] = callback;
  }
  export function Get(message: string): Function {
    message += 'Handler';
    if (container.handlers.hasOwnProperty(message)) {
      return container.handlers[message];
    }
    throw new ApplicationError('Mediator Container',Container.Get.name,`Handler for '${message}' was not registered!`);
  }
}
