export interface CreateTicketModel {
    title: string | undefined | null;
    topicId: number | undefined | null;
    roleId: number | undefined | null;
    priority: number | undefined | null;
    receiverUserId: number | undefined | null;
    description: string | undefined | null;
    attachedId: number | undefined | null;
    pageUrl: string | undefined | null;
}