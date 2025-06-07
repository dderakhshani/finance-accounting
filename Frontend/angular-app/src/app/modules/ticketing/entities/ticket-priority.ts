export enum TicketPriority {
  Normal = 0,
  High = 1,
  Medium = 2,
  Low = 3,

}

export const TicketPriority2LabelMapping: Record<TicketPriority, string> = {
    [TicketPriority.Normal]: "معمولی",
    [TicketPriority.High]: "بالا",
    [TicketPriority.Medium]: "متوسط",
    [TicketPriority.Low]: "پایین"
};


export const TicketPriority2ClassMapping: Record<TicketPriority, string> = {
    [TicketPriority.Normal]: "alert-normal",
    [TicketPriority.High]: "alert-error",
    [TicketPriority.Medium]: "alert-warning",
    [TicketPriority.Low]: "alert-info"
};

