import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TicketListComponent } from './pages/ticket-list/ticket-list.component';
import { TicketsRoutingModule } from './tickets-routing.module';
import { ComponentsModule } from 'src/app/core/components/components.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { AngularMaterialModule } from 'src/app/core/components/material-design/angular-material.module';
import { CreateTicketComponent } from './pages/create-ticket/create-ticket.component';
import { TicketDitailListComponent } from './pages/ticket-ditail-list/ticket-ditail-list.component';
import { ReplyTicketComponent } from './pages/reply-ticket/reply-ticket.component';
import { ForwardTicketComponent } from './pages/forward-ticket/forward-ticket.component';
import { PrivateMessageComponent } from './pages/private-message/private-message.component';



@NgModule({
  declarations: [
    TicketListComponent,
    CreateTicketComponent,
    TicketDitailListComponent,
    ReplyTicketComponent,
    ForwardTicketComponent,
    PrivateMessageComponent
  ],
  imports: [
    CommonModule,
    TicketsRoutingModule,
    ComponentsModule,
    ReactiveFormsModule,
    AngularMaterialModule,
    FormsModule,
    MatIconModule,
  ]
})
export class TicketingModule { }
