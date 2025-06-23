import { Component, Input ,Output, OnInit , EventEmitter } from '@angular/core';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})

export class SettingsComponent implements OnInit {

  @Input() isSettingToggled : boolean = false;
  @Output() clickHandler = new EventEmitter();

  ngOnInit() {
    console.log(this.isSettingToggled);
  }

}
