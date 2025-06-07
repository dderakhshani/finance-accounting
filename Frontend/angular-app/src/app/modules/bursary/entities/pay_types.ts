export namespace PayTypesEnum {
  export enum Id {
    Cash = 28497,
    Cheque = 28498,
    Transfer = 28777
  }

  export const Labels = {
    [Id.Cash]: 'پرداخت نقدی',
    [Id.Cheque]: 'چک',
    [Id.Transfer]: 'حواله'
  } as const;
}
