import {Inject} from "@angular/core";
import {ServiceResult} from "../../../../../core/models/service-result";
import {IRequest, IRequestHandler} from "../../../../../core/services/mediator/interfaces";
import {BaseValue} from "../../../entities/base-value";
import {HttpService} from "../../../../../core/services/http/http.service";
import {ApplicationError} from "../../../../../core/exceptions/application-error";
import {MediatorHandler} from "../../../../../core/services/mediator/decorator";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {HttpRequest} from "../../../../../core/services/http/http-request";
import {ValidationRule} from "../../../../../core/validation/validation-rule";
import {HttpClient, HttpXhrBackend} from "@angular/common/http";

export class CreateBaseValueCommand extends IRequest<CreateBaseValueCommand, BaseValue> {

  public parentId: number | undefined = undefined;
  public baseValueTypeId: number | undefined = undefined;
  public code: string | undefined = undefined;
  public title: string | undefined = undefined;
  public levelCode: string | undefined = undefined;
  public value: string | undefined = undefined;
  public isReadOnly: boolean | undefined = undefined;
  public orderIndex: number | undefined = undefined;
  public uniqueName: string | undefined = undefined;

  constructor() {
    super();
  }

  mapFrom(entity: BaseValue): CreateBaseValueCommand {
    throw new ApplicationError(CreateBaseValueCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  mapTo(): BaseValue {
    throw new ApplicationError(CreateBaseValueCommand.name, this.mapTo.name, 'Not Implemented Yet')
  }

  get url(): string {
    return "/admin/BaseValue/add";
  }

  get validationRules(): ValidationRule[] {
   return [
     {
       title: "BaseValueTypeId",
       expressions: [
         {
           query: "x => x.BaseValueTypeId != null && x.BaseValueTypeId > 0 ",
           message: {
             key: "IsRequired",
             values: [],
             messageValue: "You must fill this field ",
             translatedPropertyName: "BaseValueType"
           }
         }
       ],
     },
     {
       title: "ParentId",
       expressions: [
         {
           query: "x => (x.ParentId == null) != (x.ParentId != null && x.ParentId > 0) ",
           message: {
             key: "IsNotValidValue",
             values: [
               "0"
             ],
             messageValue: "Value 0 is not valid ",
             translatedPropertyName: "parentid"
           }
         }
       ],
     },
     {
       title: "Code",
       expressions: [
         {
           query: "x => !string.IsNullOrEmpty(x.Code) ",
           message: {
             key: "IsRequired",
             values: [],
             messageValue: "You must fill this field ",
             translatedPropertyName: "code"
           }
         },
         {
           query: "x => !string.IsNullOrEmpty(x.Code) && (x.Code.Length > 0 && x.Code.Length < 50)",
           message: {
             key: "Between",
             values: [
               "0",
               "50"
             ],
             messageValue: "Must be more than 0 and less than 50 ",
             translatedPropertyName: "code"
           }
         }
       ],
     },
     {
       title: "Title",
       expressions: [
         {
           query: "x =>  !string.IsNullOrEmpty(x.Title) ",
           message: {
             key: "IsRequired",
             values: [],
             messageValue: "You must fill this field ",
             translatedPropertyName: "title"
           }
         },
         {
           query: "x => !string.IsNullOrEmpty(x.Title) && (x.Title.Length > 0 && x.Title.Length < 250)",
           message: {
             key: "Between",
             values: [
               "0",
               "250"
             ],
             messageValue: "Must be more than 0 and less than 250 ",
             translatedPropertyName: "title"
           }
         }
       ],
     },
     {
       title: "UniqueName",
       expressions: [
         {
           query: "x => !string.IsNullOrEmpty(x.UniqueName) ",
           message: {
             key: "IsRequired",
             values: [],
             messageValue: "You must fill this field ",
             translatedPropertyName: "uniquename"
           }
         },
         {
           query: "x => !string.IsNullOrEmpty(x.UniqueName) && (x.UniqueName.Length > 0 && x.UniqueName.Length < 50)",
           message: {
             key: "Between",
             values: [
               "0",
               "50"
             ],
             messageValue: "Must be more than 0 and less than 50 ",
             translatedPropertyName: "uniquename"
           }
         }
       ],
     },
     {
       title: "Value",
       expressions: [
         {
           query: "x => !string.IsNullOrEmpty(x.Value) ",
           message: {
             key: "IsRequired",
             values: [],
             messageValue: "You must fill this field ",
             translatedPropertyName: "value"
           }
         },
         {
           query: "x => !string.IsNullOrEmpty(x.Value) && (!string.IsNullOrEmpty(x.Value) && (x.Value.Length > 0 && x.Value.Length < 50))",
           message: {
             key: "Between",
             values: [
               "0",
               "50"
             ],
             messageValue: "Must be more than 0 and less than 50 ",
             translatedPropertyName: "value"
           }
         }
       ],
     },
     {
       title: "OrderIndex",
       expressions: [
         {
           query: "x => x.OrderIndex != null && x.OrderIndex > 0",
           message: {
             key: "IsRequired",
             values: [],
             messageValue: "You must fill this field ",
             translatedPropertyName: "orderindex"
           }
         }
       ],
     }
   ]
  }
}

@MediatorHandler(CreateBaseValueCommandHandler.name)
export class CreateBaseValueCommandHandler implements IRequestHandler<CreateBaseValueCommand, BaseValue> {
  constructor(
    @Inject(HttpService) private _httpService: HttpService,
    @Inject(NotificationService) private _notificationService: NotificationService,
  ) {
  }

  async Handle(request: CreateBaseValueCommand): Promise<BaseValue> {
    let httpRequest: HttpRequest<CreateBaseValueCommand> = new HttpRequest<CreateBaseValueCommand>(request.url, request);

    return await this._httpService.Post<CreateBaseValueCommand, ServiceResult<BaseValue>>(httpRequest).toPromise().then(response => {
      this._notificationService.showSuccessMessage()
      return response.objResult
    })
  }
}
