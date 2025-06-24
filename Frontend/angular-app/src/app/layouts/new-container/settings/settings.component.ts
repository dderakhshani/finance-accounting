import {Component, Input, Output, OnInit, EventEmitter} from '@angular/core';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})

export class SettingsComponent implements OnInit {

  @Input() isSettingToggled: boolean = false;
  @Output() clickHandler = new EventEmitter();
  isClosing: boolean = false;

  ngOnInit() {
    console.log(this.isSettingToggled);
  }

  setLocalStorage(key: string, value: any) {
    localStorage.setItem(key, value);
  }

  closeHandler(): void {
    this.isClosing = true;

    setTimeout(() => {
      this.isClosing = false;
      this.clickHandler.emit(this.isSettingToggled);
    }, 300);
  }
}
