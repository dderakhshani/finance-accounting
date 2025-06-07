export class SearchQuery {
  propertyName!: string;
  comparison!: SearchComparisonTypes;
  values!: any[];
  nextOperand!: string;

  constructor(searchQuery?: {
    propertyName: string
    comparison: SearchComparisonTypes
    values: any[]
    nextOperand?: string
  }) {
    if (searchQuery) {
      this.propertyName = searchQuery.propertyName;
      this.comparison = searchQuery.comparison;
      this.nextOperand = searchQuery.nextOperand ?? 'or';
      this.values = searchQuery.values;
    }
  }


}

export class SearchComparisonTypes {
  public static equals = 'equal';
  public static notEquals = 'notEqual';
  public static contains = 'contains';
  public static notContains = 'notContains';
  public static between = 'between';

  public static startsWith = 'startsWith';
  public static endsWith = 'endsWith';
  public static greaterThan = 'greaterThan';
  public static lessThan = 'lessThan';

  public static in = 'in';
  public static inList = 'inList';
  public static ofList = 'ofList';

}

export class SearchOperandTypes {
  public static and = 'and';
  public static or = 'or';
}
