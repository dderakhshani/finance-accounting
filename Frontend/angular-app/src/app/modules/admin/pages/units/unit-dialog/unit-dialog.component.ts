import {Component, Inject} from '@angular/core';
import {FormControl, FormGroup} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Unit} from "../../../entities/unit";
import {Branch} from "../../../entities/branch";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Position} from "../../../entities/position";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {CreateUnitCommand} from "../../../repositories/unit/command/create-unit-command";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import {PageModes} from "../../../../../core/enums/page-modes";
import {UpdateUnitCommand} from "../../../repositories/unit/command/update-unit-command";
import {GetPositionsQuery} from "../../../repositories/position/queries/get-positions-command";
import {DeleteUnitCommand} from "../../../repositories/unit/command/delete-unit-command";
import {forkJoin, Observable} from "rxjs";
import {GetBranchesQuery} from "../../../repositories/branch/queries/get-branches-query";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {GetUnitQuery} from "../../../repositories/unit/queries/get-unit-query";
import {map, startWith} from "rxjs/operators";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";



@Component({
  selector: 'app-unit-dialog',
  templateUrl: './unit-dialog.component.html',
  styleUrls: ['./unit-dialog.component.scss']
})
export class UnitDialogComponent extends BaseComponent {


  pageModes = PageModes;
  tableConfigurations!: TableConfigurations;
  branches: Branch[] = [];
  unit!: Unit;
  units:Unit[] = [];
  filteredUnits!:Observable<Unit[]>
  positions: Position[] = [];
  unitPositions: Position[] = [];

  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<UnitDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any) {
    super()
    this.unit = data.unit;
    this.units = data.units;
    this.pageMode = data.pageMode;
    this.request = new CreateUnitCommand()
  }

  ngAfterViewInit() {
    this.actionBar.actions=[
      PreDefinedActions.delete()
    ]
  }

  async ngOnInit() {
    await this.resolve()
  }


  async resolve() {


    this.formActions = [
      FormActionTypes.delete
    ]
    let columns: TableColumn[] = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      new TableColumn('title', 'عنوان', TableColumnDataType.Text, '25%'),
      new TableColumn('levelCode', 'کد', TableColumnDataType.Text, '25%'),

    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true, undefined, true))


    forkJoin([
      this._mediator.send(new GetBranchesQuery()),
      this._mediator.send(new GetPositionsQuery()),
    ]).subscribe(async ([
       branchResponse,
        positionsResponse
         ]) => {
      this.branches = branchResponse.data;
      this.positions= positionsResponse.data;
      await this.initialize()
    })

  }

  async initialize() {

    if (this.pageMode === PageModes.Add) {
      this.request = new CreateUnitCommand();
    }
    if (this.pageMode === PageModes.Update) {
      this.request = new UpdateUnitCommand().mapFrom(this.unit)
    }

    this.form.controls['cloneFromUnit'].disable();

    this.filteredUnits = this.form.get('cloneFromUnit').valueChanges.pipe(
      startWith(''),
      map(unit => unit ? this.units.filter(x => x.title.toLowerCase().includes((<string>unit).toLowerCase()) && x.id !== this.unit.id) : this.units)
    )


    this.form.controls['positionIds'].valueChanges.subscribe((ids: number[]) => {
      this.unitPositions = []
      ids.forEach((positionId: number) => {
        let position = this.positions.find(x => x.id === positionId)
        if (position) this.unitPositions.push(position)
      })
      this.unitPositions = [...this.unitPositions]

    })


    this.unitPositions = [];
    let positionIds: number[] = this.form.controls['positionIds']?.getRawValue()
    positionIds.forEach(positionId => {
      let position = this.positions.find(x => x.id === positionId)
      if (position) { // @ts-ignore
        position._selected = true
        this.unitPositions.push(position)
      }
    })
    this.positions = [...this.positions]
    this.unitPositions = [...this.unitPositions];
  }

  handlePositionSelection(position: Position) {
    // @ts-ignore
    if (position._selected) {
      this.form.controls['positionIds']?.push(new FormControl(position.id))
    } else {
      this.form.controls['positionIds'].controls.splice(
        this.form.controls['positionIds'].controls.indexOf(
          this.form.controls['positionIds'].controls.find((x: FormControl) => x.value === position.id)
        ), 1
      )
    }

    (this.form as FormGroup).patchValue(this.form.getRawValue())
  }


  removePositionFromUnit() {
    // @ts-ignore
    let positionsSelectedInTable = this.unitPositions.filter(x => x.selected)
    positionsSelectedInTable.forEach(position => {
      // @ts-ignore
      position._selected = false;
      let controllerToRemove = this.form.controls['positionIds'].controls.find((x: FormControl) => x.value === position.id)
      if (controllerToRemove) this.form.controls['positionIds'].controls.splice(this.form.controls['positionIds'].controls.indexOf(controllerToRemove), 1)
    });

    (this.form as FormGroup).patchValue(this.form.getRawValue());

  }

  async add(param?: any) {
    await this._mediator.send(<CreateUnitCommand>this.request).then(response => {
      this.dialogRef.close({
        response: response,
        pageMode: this.pageMode
      })
    })
  }

  async update(param?: any) {
    await this._mediator.send(<UpdateUnitCommand>this.request).then(response => {
      this.dialogRef.close({
        response: response,
        pageMode: this.pageMode
      })
    })
  }

  async delete(param?: any) {
    await this._mediator.send(new DeleteUnitCommand((<UpdateUnitCommand>this.request).id ?? 0)).then(response => {
      this.dialogRef.close({
        response: response,
        pageMode: PageModes.Delete
      })
    })
  }

  get(param?: any): any {
  }

  async getPositionsByUnitId(unitId:number) {
    let selectedUnit = await  this._mediator.send(new GetUnitQuery(unitId))

    let unitPositions = this.form.controls['positionIds']?.value

    selectedUnit.positionIds.forEach(p => {
      if (!unitPositions.find((x:number) =>  x === p)) {
        this.form.controls['positionIds']?.push(new FormControl(p))
        // @ts-ignore
        this.positions.find(x => x.id === p)._selected = true
      }
    });

    (this.form as FormGroup).patchValue(this.form.getRawValue())

  }

  close(): any {
  }
  unitDisplayFn(unitId: number) {
    return this.units.find(x => x.id === unitId)?.title ?? ''
  }

}
