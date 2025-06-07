import {Component, Inject, OnInit} from '@angular/core';
import {MAT_SNACK_BAR_DATA} from "@angular/material/snack-bar";

@Component({
  selector: 'app-http-snackbar',
  templateUrl: './http-snackbar.component.html',
  styleUrls: ['./http-snackbar.component.scss']
})
export class HttpSnackbarComponent implements OnInit {

  message:string = 'عملیات با موفقیت انجام شد.';
  constructor(
    @Inject(MAT_SNACK_BAR_DATA) public data: string
  ) {
    if (data) {
      this.message = data;
    }
  }

  ngOnInit(): void {
  }

}
