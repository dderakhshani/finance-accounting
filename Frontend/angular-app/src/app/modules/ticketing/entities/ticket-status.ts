export enum TicketStatus {
    Created = 0,
    RequesterAnswered = 1,
    ReceiverAnswered = 2,
    Forwarded = 3,
    Ongoing = 4,
    ClosedByRequester = 5,
    ClosedByReceiver = 6,
    ClosedBySystem = 7
}

export const TicketStatus2LabelMapping: Record<TicketStatus, string> = {
    [TicketStatus.Created]: "ایجاد شده",
    [TicketStatus.RequesterAnswered]: "پاسخ کاربر",
    [TicketStatus.ReceiverAnswered]: "پاسخ پشتیبانی",
    [TicketStatus.Forwarded]: "ارجاع شده",
    [TicketStatus.Ongoing]: "در دست اقدام",
    [TicketStatus.ClosedByRequester]: "بسته شده توسط کاربر",
    [TicketStatus.ClosedByReceiver]: "بسته شده توسط پشتیبانی",
    [TicketStatus.ClosedBySystem]: "بسته شده توسط سیستم",
};


export const TicketStatus2ClassMapping: Record<TicketStatus, string> = {
    [TicketStatus.Created]: "alert-info",
    [TicketStatus.RequesterAnswered]: "alert-green",
    [TicketStatus.ReceiverAnswered]: "alert-info",
    [TicketStatus.Forwarded]: "alert-warning",
    [TicketStatus.Ongoing]: "alert-ongoing",
    [TicketStatus.ClosedByRequester]: "alert-close",
    [TicketStatus.ClosedByReceiver]: "alert-close",
    [TicketStatus.ClosedBySystem]: "alert-close",
};
