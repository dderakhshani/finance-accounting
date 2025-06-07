export class ChequeBooksSheet {
  chequeBookId!: number;
  chequeSheetNo!: number;
  sayyadNo!: string | null;
  isCanceled!: boolean;
  cancelDate!: string | null;
  cancelDescp!: string | null;
  statusCode!: string | null;
  statusName!: string | null;
  payables_Documents!: any[];
  id!: number;
  isDeleted!: boolean;
  amount!: number|string;
}
