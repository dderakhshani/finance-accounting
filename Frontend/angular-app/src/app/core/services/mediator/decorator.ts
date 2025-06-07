import { Container } from "./container";

export function MediatorHandler(message: string): (target: Function) => void {
  return (target: Function) => {
    Container.Register(message, target);
  };
}
