import {HttpValidationError} from "./http-validation-error";

export class ServiceResult<T> {
  objResult!: T;
  succeed!: boolean;
  errors: HttpValidationError[] = [];
  message!: string;
  ///Execptions
  ObjResult !: ObjResult;

  constructor(data: T) {
    this.objResult = data;
  }
}

export class Error {
  
  StackTrace !: string;
  Message !: string;
  Data: any;
  InnerException !: string;
  HelpLink !: string;
  Source !: string;
  HResult !: string;
}

export class ObjResult {
  Error !: Error;
  Path !: string;
 
}
