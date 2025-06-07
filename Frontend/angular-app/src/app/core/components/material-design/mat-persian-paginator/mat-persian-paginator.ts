import {MatPaginatorIntl} from "@angular/material/paginator";

export function MatPersianPaginator() {
  const customPaginatorIntl = new MatPaginatorIntl();

  customPaginatorIntl.itemsPerPageLabel = 'تعداد نمایش:';
  customPaginatorIntl.firstPageLabel = 'صفحه اول';
  customPaginatorIntl.lastPageLabel = 'صفحه آخر';
  customPaginatorIntl.nextPageLabel = 'صفحه بعدی';
  customPaginatorIntl.previousPageLabel = 'صفحه قبلی';

  customPaginatorIntl.getRangeLabel = (page: number, pageSize: number, length: number) => {
    if (length === 0 || pageSize === 0) {
      return ` `;
    }
    length = Math.max(length, 0);
    const startIndex = page * pageSize;
    // If the start index exceeds the list length, do not try and fix the end index to the end.
    const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
    return `${startIndex + 1} الی ${endIndex} از ${length}`;
  };
  return customPaginatorIntl;
}
