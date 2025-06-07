import {Component, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../core/abstraction/base.component";
import {Attachment} from "../../entities/attachment";
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import {SearchAttachmentsQuery} from "../../repositories/queries/search-attachments-query";
import {environment} from "../../../../../environments/environment";
import {PageEvent} from "@angular/material/paginator";

@Component({
  selector: 'app-search-attachments',
  templateUrl: './search-attachments.component.html',
  styleUrls: ['./search-attachments.component.scss']
})
export class SearchAttachmentsComponent extends BaseComponent {


  attachments: Attachment[] = []

  pageIndex = 1;
  pageSize = 24;
  searchQuery = '';
  magnifiedAttachment: Attachment | null = null;


  constructor(private mediator: Mediator) {
    super();
  }

  get isAnyPhotoSelected() {
    // @ts-ignore
    return this.attachments.filter(x => x.isSelected).length > 0
  }
  async ngOnInit() {
    await this.resolve()
  }

  async resolve(params?: any) {
    await this.get();
  }

  initialize(params?: any) {
    throw new Error('Method not implemented.');
  }

  add(param?: any) {
    throw new Error('Method not implemented.');
  }

  async get() {
    this.isLoading = true
    await this.mediator.send(new SearchAttachmentsQuery(this.pageIndex, this.pageSize, this.searchQuery)).then(res => {
      this.totalItems = res.totalCount
      this.attachments = res.data.map(x => {
        x.url = environment.fileServer + "/" + x.url
        if (!x.title.trim()) x.title = 'بدون عنوان'
        return x;
      })
      this.isLoading = false;
    })
      .catch(() => {
        this.isLoading = false
      })
  }

  update(param?: any) {
    throw new Error('Method not implemented.');
  }

  delete(param?: any) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }

  download(attachment?: Attachment) {
    if(attachment) this.downloadURLs(attachment.url)
    else { // @ts-ignore
      let selectedAttachments = this.attachments.filter(x => x.isSelected);
      if( selectedAttachments.length > 0) this.downloadURLs(selectedAttachments.map(x => x.url))
    }
  }

  downloadURLs(urls:string[] | string) {
    if (!Array.isArray(urls)) {
      urls = [urls]; // Convert single URL to array
    }

    urls.forEach(url => {
      const a = document.createElement('a');
      a.href = url;
      a.download = ''; // Trigger download behavior
      a.target= '_blank'
      a.style.display = 'none';
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    });
  }

  onBackgroundClick(event: MouseEvent): void {
    const target = event.target as HTMLElement;

    if (target.tagName.toLowerCase() !== 'img') {
      this.magnifiedAttachment = null;
    }
  }

  async onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    return await this.get();
  }
}
