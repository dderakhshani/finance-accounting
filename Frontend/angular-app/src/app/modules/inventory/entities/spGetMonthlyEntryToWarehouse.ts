export interface spGetMonthlyEntryToWarehouse {
  title: string;
  commodityCode: string;
  far: number | null;
  ord: number | null;
  khor: number | null;
  tir: number | null;
  mor: number | null;
  shah: number | null;
  meh: number | null;
  aba: number | null;
  aza: number | null;
  dey: number | null;
  bah: number | null;
  esf: number | null;
  total: number | null;
}

export interface MonthlyEntryToWarehouse {
  commodityCode: string;
  title: string;
  enter: spGetMonthlyEntryToWarehouse | undefined ;
  exit: spGetMonthlyEntryToWarehouse | undefined;
  //totalEnter: number;
  //totalExit: number;
}
