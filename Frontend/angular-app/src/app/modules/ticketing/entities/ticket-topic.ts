export enum TicketTopic {
    Error = 1,
    Feature = 2
}

export const TicketTopic2LabelMapping: Record<TicketTopic, string> = {
    [TicketTopic.Error]: "گزارش خطا",
    [TicketTopic.Feature]: "افزودن ویژگی جدید",
};