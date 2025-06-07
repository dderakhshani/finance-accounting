import {ValidationRule} from "../../validation/validation-rule";

export interface CommandBase {
  url():string;
  validationRules():ValidationRule[]
}
