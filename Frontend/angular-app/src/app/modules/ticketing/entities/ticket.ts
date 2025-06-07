export interface TicketModel {
    id: number;
    title: string;
    roleId: number;
    roleTitle:string;
    status: number;
    createDate: Date;
    updateDate: Date;
    serviceId: number;
    priority: number;
    receiverUserId: number;
    creatorUserId: number;
}