import { Injectable } from '@angular/core';
import {FormArray, FormBuilder, FormGroup} from "@angular/forms";
import {AuditableEntity} from "../../../core/abstraction/auditable-entity";
import {ApplicationError} from "../../../core/exceptions/application-error";

@Injectable({
  providedIn: 'root'
})
export class FormService {

  constructor(
    private _fb: FormBuilder
  ) { }


  createFormFromEntity<T extends AuditableEntity>(entity: T, bindEntityToForm: boolean = false, resetForm:boolean = false) : FormGroup {
      let entityForm = {};
      // @ts-ignore
      Object.keys(entity).forEach(key => {

        // @ts-ignore
        if(!entity[key]) {
          // @ts-ignore
          entityForm[key] = null;
        }
        // @ts-ignore
        else if (typeof entity[key] === "string" || typeof entity[key] === "number" || typeof entity[key] === "boolean" ) {
          // Its either number or string or bool
          //@ts-ignore
          entityForm[key] = entity[key]
        }
        else {
          // Its an Array
          //@ts-ignore
          if (entity[key]?.length >= 0) {
            //@ts-ignore
            entityForm[key] = this._fb.array([])
          }
            // Its a Date
          //@ts-ignore
          else if ( typeof entity[key]?.getMonth === 'function') {
            //@ts-ignore
            entityForm[key] = entity[key]
          }
          else {
            //@ts-ignore
            entityForm[key] = this.createFormFromEntity(entity[key])
          }
        }
      });

      let descriptors = Object.getOwnPropertyDescriptors(Object.getPrototypeOf(entity));
      Object.keys(descriptors).forEach(key => {
        // @ts-ignore
        if (typeof descriptors[key]?.get === "function" && key !== '__proto__'){
          // @ts-ignore
          entityForm[key] = entity[key] ? entity[key] : null;
        }

      });
      let form = this._fb.group(entityForm);

      if (bindEntityToForm) {
        this.bindEntityToFormGroup(entity,form);
      }

      if (resetForm) {
        form.reset();
      }
      return form;
  }

  bindEntityToFormGroup<T extends AuditableEntity>(entity: T, form: FormGroup, referenceFields: string[] = []) {
    if (!this.checkFieldsMatching(entity, form)) {
      throw new ApplicationError(FormService.name,this.bindEntityToFormGroup.name,'entity and Form fields is not matching');
    }
    this.setFormFromEntity(entity, form);
    const formKeys = Object.keys(form.controls);
    formKeys.forEach(key => {
      if (referenceFields.includes(key)) {
        form.controls[key].valueChanges.subscribe(
          (newValue) => {
            //@ts-ignore
            entity[key] = newValue.id;
          }
        )
      } else {
        form.controls[key].valueChanges.subscribe(
          (newValue) => {
            //@ts-ignore
            entity[key] = newValue;
          }
        )
      }
    });
  }

  bindEntityToFormArray<T>(entity: T[],form : FormArray, referenceKeys:string[] = []) {
    if (referenceKeys.length > 0) {

    } else {
      form.valueChanges.subscribe((newValue : T[]) => {
        entity.splice(0,entity.length);
        entity.push(...newValue);
      })
    }
  }
  setFormFromEntity<T extends AuditableEntity>(entity:T | T[], form:FormGroup | FormArray){
    // FormArray
    //@ts-ignore
    if (typeof form.push === "function" && entity.length > 0) {
      // @ts-ignore
      this.clearFormArray(form);

      // @ts-ignore
      entity.forEach(item => {
        // @ts-ignore
        form.push(this.createFormFromEntity(item))
      })
    }
    // FormGroup
    else {
      const keys = Object.keys(form.controls);
      keys.forEach(key => {
        //@ts-ignore
        if(typeof form.controls[key].push === "function") {
          //@ts-ignore
          this.clearFormArray(form.controls[key]);
          //@ts-ignore
          entity[key]?.forEach( item => {
            //@ts-ignore
            form.controls[key].push(this.createFormFromEntity(item));
          })
        } else {
          //@ts-ignore
          let value = entity[key];
          if (value) {
            //@ts-ignore
            form.controls[key].setValue(value);
          }
        }
      });
    }
  }

  private checkFieldsMatching(entity: any, form: FormGroup): boolean {
    const formKeys = Object.keys(form.controls);
    const entityKeys = Object.keys(entity);
    // @ts-ignore
    formKeys.forEach(formKey => {
      if (!entityKeys.includes(formKey)) {
        return false;
      }
    });
    return true;
  }


  addEntityToFormArray<T extends AuditableEntity>(entity:T | T[], form: FormArray) {
    //@ts-ignore
    if (entity.length > 0) {
      //@ts-ignore
      entity.forEach(item => {
        form.push(this.createFormFromEntity(item))
      })
    } else {
      //@ts-ignore
      let entityForm = this.createFormFromEntity(entity);
      form.push(entityForm);
    }
  }

  clearFormArray(form: FormArray) {
    while (form.controls.length > 0) {
      form.removeAt(0);
    }
  }
}
