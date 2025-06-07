export class ApplicationError extends Error{
  constructor(className:string,functionName:string, message:string) {

    super(`${className} -> ${functionName}: ${message}`);
  }
}
