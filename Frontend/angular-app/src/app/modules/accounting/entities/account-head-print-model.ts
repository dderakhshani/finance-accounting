export class AccountHeadPrintModel {
    level2debit: number = 0;
    level2credit: number = 0;
    level3: level3Model[] = [];
}
export class level3Model {

    level3debit: number = 0;
    level3credit: number = 0;
    level4: level4Model[] = [];
}
export class level4Model {

    level4debit: number = 0;
    level4credit: number = 0;
}