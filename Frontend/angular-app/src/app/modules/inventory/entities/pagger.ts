export class Pagger {
  tableName: string;
  itemsPerPage: number;
  currentPage: number;
  pages: number;
  inited: boolean;


  constructor(tableName: string, itemsPerPage: number) {
    this.tableName = tableName;
    this.itemsPerPage = itemsPerPage;
    this.currentPage = 1;
    this.pages = 0;
    this.inited = false;
  }

  showRecords(from: number, to: number): void {
    let element :any= document.getElementById(this.tableName) as HTMLInputElement;
    let rows = element.rows 

    // i starts from 1 to skip table header row
  
    for (let i = 1; i < rows.length; i++) {
      if (i < from || i > to) {
        rows[i].style.display = 'none';
      } else {
        rows[i].style.display = '';
      }
    }
  }

  showPage(pageNumber: number): void {
    if (!this.inited) {
      // Not initialized
      return;
    }

    let oldPageAnchor = document.getElementById('pg' + this.currentPage) as HTMLInputElement;
    oldPageAnchor.className = 'pg-normal';

    this.currentPage = pageNumber;
    let newPageAnchor = document.getElementById('pg' + this.currentPage) as HTMLInputElement;
    newPageAnchor.className = 'pg-selected';

    let from = (pageNumber - 1) * this.itemsPerPage + 1;
    let to = from + this.itemsPerPage - 1;
    this.showRecords(from, to);

    let pgNext = document.querySelector('.pg-next') as HTMLInputElement,
      pgPrev = document.querySelector('.pg-prev') as HTMLInputElement;
    if (this.currentPage == this.pages) {
      pgNext.style.display = 'none';
    } else {
      pgNext.style.display = '';
    }

    if (this.currentPage === 1) {
      pgPrev.style.display = 'none';
    } else {
      pgPrev.style.display = '';
    }
  }

  prev(): void {
    if (this.currentPage > 1) {
      this.showPage(this.currentPage - 1);
    }
  }

  next(): void {
    if (this.currentPage < this.pages) {
      this.showPage(this.currentPage + 1);
    }
  }

  init(): void {
    
    let element: any = document.getElementById(this.tableName) as HTMLInputElement;
    let rows = element.rows 
   
    let records = rows.length - 1;

    this.pages = Math.ceil(records / this.itemsPerPage);
    this.inited = true;
  }

  showPageNav(pagerName: string, positionId: string): void {
    if (!this.inited) {
      // Not initialized
      return;
    }

    let element = document.getElementById(positionId) as HTMLInputElement,
      pagerHtml = '<span onclick="' + pagerName + '.prev();" class="pg-normal pg-prev">«</span>';

    for (let page = 1; page <= this.pages; page++) {
      pagerHtml += '<span id="pg' + page + '" class="pg-normal pg-next" onclick="' + pagerName + '.showPage(' + page + ');">' + page + '</span>';
    }

    pagerHtml += '<span onclick="' + pagerName + '.next();" class="pg-normal">»</span>';

    element.innerHTML = pagerHtml;
  }
}
