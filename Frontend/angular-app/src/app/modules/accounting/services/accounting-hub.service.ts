import {EventEmitter, Injectable} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {IdentityService} from "../../identity/repositories/identity.service";
import {AccountingModule} from "../accounting.module";
import {LockVoucherForUpdateCommand} from "../repositories/voucher-head/commands/lock-voucher-for-update-command";
import {IHttpConnectionOptions} from "@microsoft/signalr";

@Injectable({
  providedIn: 'root'
})
export class AccountingHubService {

  isInitialized = false;
  hubUrl: string;
  connection: any;

  connectionStateChanged: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(private identityService: IdentityService) {
    this.hubUrl = 'http://127.0.0.1:50002/accountingHub';
    // console.log('Accounting Hub Service Created')
  }

  public async init(): Promise<void> {
    if (!this.isInitialized) {

      let options: IHttpConnectionOptions = {
        accessTokenFactory: () => {
          return this.identityService.applicationUser.token;
        }
      };
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl(this.hubUrl, options)
        .withAutomaticReconnect()
        .build();

      this.applyListeners();

      await this.connection.start()
        .then(() => {
          this.connectionStateChanged.emit(true)
          // console.log(`SignalR connection success! connectionId: ${this.connection.connectionId}`);
          this.isInitialized = true


          this.connection.invoke('HandleInvocation',LockVoucherForUpdateCommand.name, JSON.stringify(new LockVoucherForUpdateCommand(1)));
        })
        .catch((error: any) => {
          this.connectionStateChanged.emit(false)
          // console.log(`SignalR connection failure: ${error}`);
        });

    }
  }

  public applyListeners(): void {
    this.connection.on('DisplayMessage', this.messageRecieved)
  }

  public async messageRecieved(message: string) {
    console.log(`SignalR Message Recieved: ${message}`)
  }
}
