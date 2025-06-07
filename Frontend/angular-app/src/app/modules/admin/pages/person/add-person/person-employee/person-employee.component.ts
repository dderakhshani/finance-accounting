import {Component, Input} from '@angular/core';
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {PageModes} from "../../../../../../core/enums/page-modes";
import {GetEmployeeByPersonIdQuery} from "../../../../repositories/employee/queries/get-employee-by-person-id-query";
import {forkJoin} from "rxjs";
import {GetBranchesQuery} from "../../../../repositories/branch/queries/get-branches-query";
import {GetUnitsQuery} from "../../../../repositories/unit/queries/get-units-query";
import {GetPositionsQuery} from "../../../../repositories/position/queries/get-positions-command";
import {Branch} from "../../../../entities/branch";
import {Unit} from "../../../../entities/unit";
import {Position} from "../../../../entities/position";
import {UpdateEmployeeCommand} from "../../../../repositories/employee/commands/update-employee-command";
import {CreateEmployeeCommand} from "../../../../repositories/employee/commands/create-employee-command";
import {Employee} from "../../../../entities/employee";

@Component({
  selector: 'app-person-employee',
  templateUrl: './person-employee.component.html',
  styleUrls: ['./person-employee.component.scss']
})
export class PersonEmployeeComponent extends BaseComponent {

  // entity!: Employee;
  @Input() personId!: number

  branches: Branch[] = [];
  units: Unit[] = [];
  positions: Position[] = [];

  filteredBranches: Branch[] = [];
  filteredUnits: Unit[] = [];
  filteredUnitPositions: Position[] = [];


  constructor(
    private _mediator: Mediator,
  ) {
    super();

    this.request = new CreateEmployeeCommand()
  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {
    forkJoin([
      this._mediator.send(new GetBranchesQuery()),
      this._mediator.send(new GetUnitsQuery()),
      this._mediator.send(new GetPositionsQuery()),
    ]).subscribe(async ([
                          branches,
                          units,
                          positions,
                        ]) => {
      this.branches = branches.data;
      this.filteredBranches = this.branches;

      this.units = units.data;
      this.filteredUnits = this.units;

      this.positions = positions.data;
      this.filteredUnitPositions = this.positions;

      await this.initialize()
    })
  }

  async initialize(entity?: Employee) {
    if (entity || this.personId) {
      this.pageMode = PageModes.Update
      if (!entity) entity = await this.get(this.personId)
      this.request = new UpdateEmployeeCommand().mapFrom(entity);
    } else {
      this.pageMode = PageModes.Add
      let req = new CreateEmployeeCommand();
      req.personId = this.personId;
      this.request = req

      this.form.controls['unitId']?.disable()
      this.form.controls['unitPositionId']?.disable()
    }
    this.form.controls['accountReferenceCode']?.disable()

  }

  async get(id: number) {
    return await this._mediator.send(new GetEmployeeByPersonIdQuery(id))
  }

  override async submit() {
    if (this.pageMode === PageModes.Add && !this.form.pristine) await this.add()
    if (this.pageMode === PageModes.Update && !this.form.pristine) await this.update()
  }

  async add() {
    let request = <CreateEmployeeCommand>this.request
    request.personId = this.personId
    let response = await this._mediator.send(request);
    await this.initialize(response)
  }

  async update() {
    let request = <UpdateEmployeeCommand>this.request
    request.personId = this.personId
    let response = await this._mediator.send(request);
    await this.initialize(response)
  }

  handleEmployeeUnitPositionAndUnitAndBranchSelection() {
    let branch = this.branches.find(x => x.id === this.form.controls['branchId']?.value);
    let unit = this.units.find(x => x.id === this.form.controls['unitId']?.value);
    let unitPosition = this.positions.find(x => x.id === this.form.controls['unitPositionId']?.value);

    let isBranchConnectedToUnit = unit?.branchId === branch?.id;
    let isUnitConnectedToUnitPosition = unitPosition ? unit?.positionIds.includes(unitPosition?.id) : false;

    if (branch && unit && !isBranchConnectedToUnit) {
      this.filteredUnits = this.units.filter(x => x.branchId === branch?.id)
      this.form.controls['unitId'].reset();

      this.filteredUnitPositions = []
      this.form.controls['unitPositionId'].reset();
      this.form.controls['unitPositionId'].disable();
    }

    if (branch && !unit) {
      this.filteredUnits = this.units.filter(x => x.branchId === branch?.id)
      this.form.controls['unitId'].enable();
    }

    if (branch && unit && (!unitPosition || !isUnitConnectedToUnitPosition)) {
      this.filteredUnitPositions = this.positions.filter(x => unit?.positionIds.includes(x.id))
      this.form.controls['unitPositionId'].reset();
      this.form.controls['unitPositionId'].enable();
    }
  }

  delete(param?: any) {
  }

  close(): any {
  }
}
