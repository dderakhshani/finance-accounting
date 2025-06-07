export interface TicketDetailHistoryModel {
    historyId: number;
    ticketDetailId: number;
    creatDate: Date;
    primaryRoleId: number;
    secondaryRoleId: number;
    primaryRoleName: string;
    secondaryRoleName: string;
    message: string;
}