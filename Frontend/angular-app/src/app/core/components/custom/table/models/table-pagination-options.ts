export class TablePaginationOptions {
    public pageIndex: number = 0;
    public pageSize: number = 100;
    public totalItems: number = 0;
    public pageSizeOptions: number[] = [100,200,500,1000,2000, 5000 ,10000,20000,50000 , 100000 ];
    public paginationChangeSource: 'scroll' | 'default' = 'default';
}
