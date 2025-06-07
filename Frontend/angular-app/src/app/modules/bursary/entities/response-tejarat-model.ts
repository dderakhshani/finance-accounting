import { AccountHistoryItem } from "./account-history-item";

export interface ResponseTejaratModel {
    balance: string;
    transactionCount: number | null;
    detailItems: AccountHistoryItem[];
    filePath: string;
}