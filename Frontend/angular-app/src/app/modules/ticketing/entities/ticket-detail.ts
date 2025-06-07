export interface TicketDetailModel {
    id: number;
    ticketId: number;
    ticketTitle: string;
    description: string;
    creatDate: Date;
    attachmentIds: string;
    detailCreatorUserId: number;
    detailCreatorUserFullName: string;
    ticketCreatorUserId: number;
    ticketCreatorUserFullName: string;
    readDate: Date;
    roleId: number;
    roleName: string;
    readerUserId: number;
    readerUserFullName: string;
    detailCreatorUserRoleName: string;
    detailCreatoruserRoleId: number;
    historyCount: number;
    ticketStatus:  number;

    attachmentIdsNumber: string[];
}