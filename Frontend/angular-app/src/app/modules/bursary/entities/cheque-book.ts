
import {ChequeBooksSheet} from "./cheque-books-sheet";
import {BankAccounts} from "./bank-accounts";

export class ChequeBook {
  bankAccountId!: number;
  getDate!: string;
  serial!: string;
  sheetsCount!: number;
  startNumber!: number;
  descp!: string | null;
  bankAccount!: BankAccounts;
  payables_ChequeBooksSheets!: ChequeBooksSheet[];
  id!: number;
  isDeleted!: boolean;
}
