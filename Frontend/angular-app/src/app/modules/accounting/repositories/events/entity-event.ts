export class EntityEvent {
  id!:string
  originId!:string
  entityId!:number
  entityType!:string
  descriptions!:string
  state!:number
  payload!:any
  payloadJSON!:string
  payloadType!:string
  type!:number
  createdAt!:Date
  createdById!:number
  createdBy!:string

  subEvents: EntityEvent[] =[]
}
