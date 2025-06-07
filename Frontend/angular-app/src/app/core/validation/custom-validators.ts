import {AbstractControl, ValidatorFn} from "@angular/forms";
import {ValidationRule} from "./validation-rule";

export class CustomValidators {

  static getValidators(validations?: ValidationRule | null) {
    let validators: ValidatorFn[] = [];
    if (validations) {
      validations.expressions.forEach(expression => {
        switch (expression.message.key.toLowerCase()) {
          case ('IsRequired').toLowerCase():
            return validators.push(this.isRequired([expression.message.translatedPropertyName, expression.message.messageValue].join(' ')));
          case ('Between').toLowerCase():
            return validators.push(this.between([expression.message.translatedPropertyName, expression.message.messageValue].join(' '), +expression.message.values[0], +expression.message.values[1]));
          case ('MustBeNumber').toLowerCase():
            return validators.push(this.mustBeNumber([expression.message.translatedPropertyName, expression.message.messageValue].join(' ')))
          case ('MustBeNumberOrEmpty').toLowerCase():
            return validators.push(this.mustBeNumberOrEmpty([expression.message.translatedPropertyName, expression.message.messageValue].join(' ')))
          default:
            return
        }
      });
    }
    return validators;
  }


  static isRequired(message: string): ValidatorFn {
    return (control: AbstractControl): {
      [key: string]: string
    } | null => {
      if (!control.value) {
        return {
          isRequired: message ?? ''
        }
      }
      return {};
    }
  }

  static between(message: string, min: number, max: number): ValidatorFn {
    return (control: AbstractControl): {
      [key: string]: string
    } | null => {
      if (control.value?.length < min || control.value?.length > max) {
        return {
          between: message
        }
      }
      return {};
    }
  }

  static mustBeNumber(message: string): ValidatorFn {
    return (control: AbstractControl): {
      [key: string]: string
    } | null => {
      if (typeof control.value !== 'number') {
        return {
          isRequired: message ?? ''
        }
      }
      return {};
    }
  }
  static mustBeNumberOrEmpty(message: string): ValidatorFn {
    return (control: AbstractControl): {
      [key: string]: string
    } | null => {
      if (control.value && typeof control.value !== 'number') {
        return {
          isRequired: message ?? ''
        }
      }
      return {};
    }
  }

}
