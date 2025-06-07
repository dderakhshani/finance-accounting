export class Assets {
  public id: number | undefined;
  public warehouseId: number | undefined;
  public commodityId: number | undefined;
  public measureId: number | undefined;
  public assetGroupId: number | undefined;
  public unitId: number | undefined;
  public documentDate: string | undefined;
  public commoditySerial: string | undefined;
  public assetSerial: string | undefined;
  public depreciationTypeBaseId: number | undefined;
  public price: number | undefined;
  public depreciatedPrice: number | undefined;
  public isActive: boolean | undefined;
  public documentItemId: number | undefined;
  public startWithNumber: number | undefined;
  public prefix: string | undefined;
  public warehousesTitle: string | undefined;
  public categoryTitle: string | undefined;
  public measureTitle: string | undefined;
  public searchTerm: string | undefined;
  public categoryLevelCode: string | undefined;
  public commodityTitle: string | undefined;
  public unitsTitle: string | undefined;
  public depreciationTitle: string | undefined;
  public assetGroupTitle: string | undefined;
  public commodityCode: string | undefined;
  public isHaveWast: boolean | undefined;
  public isAsset: boolean | undefined;
  public isConsumable: boolean | undefined;
  public documentNo: number | undefined;
  public description: string | undefined;
  public title: string | undefined;

  public documentItemsDescription: string | undefined;
  public documentDescription: string | undefined;
  public totalDescription: string | undefined;
  public assetsSerials: AssetsSerial[] | undefined;
}
export class AssetsSerial {
  public commodityId: number | undefined;
  public serial: string | undefined;
  public id: number | undefined;
  public selected: boolean | undefined;
  public title: boolean | undefined;
  public commoditySerial: boolean | undefined;
  public description: string | undefined ;
  

}
