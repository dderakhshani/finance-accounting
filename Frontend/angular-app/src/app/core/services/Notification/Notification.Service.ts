import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {IHttpConnectionOptions} from "@microsoft/signalr";
import {IdentityService} from "../../../modules/identity/repositories/identity.service";
import {HttpSnackbarComponent} from "../../components/material-design/http-snackbar/http-snackbar.component";
import {MatSnackBar} from "@angular/material/snack-bar";
import {ToastrService} from "ngx-toastr";
import {Notification} from "rxjs";
import {ObjectNotification} from "../../models/ObjectNotification";
import {environment} from "../../../../environments/environment";
import {Toastr_Service} from "../../../shared/services/toastrService/toastr_.service";


@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private hubConnection: signalR.HubConnection;
  private readonly retryDelay = 2000; // 2 seconds
  private readonly maxRetries = 5;
  private retryCount = 0;
  private options: IHttpConnectionOptions;
  constructor(
    private identityService: IdentityService,
    private snackBar: MatSnackBar,
    private toastr: Toastr_Service,
  ) {
    this.options=   {
     withCredentials: true,
      accessTokenFactory: () => {
        return this.identityService.applicationUser.token;
      },
      skipNegotiation:true,
      transport:signalR.HttpTransportType.WebSockets
    };

    this.hubConnection = new signalR.HubConnectionBuilder()
     // .withUrl(`http://127.0.0.1:50017/NotificationUserHub`,this.options)
     .withUrl(`${environment.apiURL}/NotificationUserHub`,this.options)
      .withAutomaticReconnect([0, 1000, 5000, 10000])
      .configureLogging(signalR.LogLevel.Information)
      .build();
    this.hubConnection.serverTimeoutInMilliseconds = 300000; // 5 minutes
    this.hubConnection.keepAliveIntervalInMilliseconds = 10000; // 10 seconds

  }
  startConnection() {
    if(this.hubConnection.state==="Disconnected")
    {
      // console.log("Attempting to start connection..."); // Log for starting connection
      this.hubConnection
        .start()
        .then(() => {
          // console.log("Connection successfully started");
          this.listenForNotifications();
        })
        .catch(err => {
          console.error("Error while starting connection: ", err);
          if (this.retryCount < this.maxRetries) {
            this.retryCount++;
            setTimeout(() => this.startConnection(), this.retryDelay);
          } else {
            console.error("Max retries reached. Could not connect.");
          }
        });
    }
    this.hubConnection.onclose(() => {
      // console.log('Connection closed');
      setTimeout(() => this.startConnection(), 5000); // Retry connection in 5 seconds if closed
    });
  }

  listenForNotifications(): void {
     this.hubConnection.on("notifyUpdateVoucherHead", (notificationModel: any) => {
     this.notifyUpdateVoucherHead(notificationModel)
    });
    this.hubConnection.on("notifyNewTicket", (notificationModel: any) => {
      this.notifyNewTicket(notificationModel)
    });
    this.hubConnection.on("notifyInventoryReciept", (notificationModel: any) => {
      this.notifyInventoryReciept(notificationModel)
    });

  }

  disconnect(): void {
    this.hubConnection.stop();
    this.hubConnection.onclose(error => {
      console.error("Connection closed unexpectedly: ", error?.message);
    });

  }

  notifyUpdateVoucherHead(notificationModel: ObjectNotification) {
    this.toastr.showToast(
      {title : notificationModel.messageTitle,
        message : notificationModel.messageContent.length>200? notificationModel.messageContent.substring(0,200) :notificationModel.messageContent,
        type : 'info'   ,
        options :{
          disableTimeOut: true,
          closeButton:true,
          newestOnTop : true
        }
      }
    );
  }
  notifyNewTicket(notificationModel: any) {
    console.log(notificationModel);
    this.toastr.showToast(
      {title : notificationModel.messageTitle,
        message : notificationModel.messageContent.length>200? notificationModel.messageContent.substring(0,200) :notificationModel.messageContent,
        type : 'info'   ,
        options :{
          disableTimeOut: true,
          closeButton:true,
          newestOnTop : true
        }
      }
    );
  }
  notifyInventoryReciept(notificationModel: ObjectNotification) {
    this.toastr.showToast(
      {title : notificationModel.messageTitle,
        message :notificationModel.messageContent.length>200? notificationModel.messageContent.substring(0,200) :notificationModel.messageContent,
        type : 'info'   ,
        options :{
          disableTimeOut: true,
          closeButton:true,
          newestOnTop : true
        }
      }
    );
  }
}
