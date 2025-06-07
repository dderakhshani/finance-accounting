import { BomHeaderValue } from "./bom-header-value";

export interface BomHeader {
  id: number;
  bomId: number;
  bomTitle: string;
  commodityTitle: string;  
  bomDate: string;
  title: string;
  name: string;
  values: BomHeaderValue[];
  
}
