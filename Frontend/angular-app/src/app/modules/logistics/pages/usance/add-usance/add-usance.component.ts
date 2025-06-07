import {Component} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {BaseValue} from "../../../../admin/entities/base-value";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {AddUsanceCommand} from "../../../repositories/usance/commands/add-usance-command";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {UpdateUsanceCommand} from "../../../repositories/usance/commands/update-usance-command";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../../admin/repositories/base-value/queries/get-base-values-by-unique-name-query";

@Component({
  selector: 'app-add-usance',
  templateUrl: './add-usance.component.html',
  styleUrls: ['./add-usance.component.scss']
})
export class AddUsanceComponent extends BaseComponent{

  commodityType!:BaseValue[]
  constructor(
    private _mediator: Mediator,
    // private router:Router,
    // private route:ActivatedRoute
  ) {
    super();
    this.request = new AddUsanceCommand()
  }


  async ngOnInit() {
    await this.resolve()
  }
  async resolve() {
    await this._mediator.send(new GetBaseValuesByUniqueNameQuery('commodityType')).then(res => {
      this.commodityType = res
    })

    this.formActions=[
      FormActionTypes.add,
      FormActionTypes.save,
    ]
  }
  initialize(): any {

  }
  async add() {
    let response=await this._mediator.send(<AddUsanceCommand>this.request)
    this.request=new UpdateUsanceCommand().mapFrom(response)
  }
  async update(){
    let response=await this._mediator.send(<UpdateUsanceCommand>this.request)
    this.request=new UpdateUsanceCommand().mapFrom(response)
  }
  close(): any {
  }

  delete(): any {
  }

  get(id: number): any {
  }


}
