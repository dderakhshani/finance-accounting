import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TicketListComponent } from './pages/ticket-list/ticket-list.component';
import { CreateTicketComponent } from './pages/create-ticket/create-ticket.component';
import { TicketDitailListComponent } from './pages/ticket-ditail-list/ticket-ditail-list.component';

const routes: Routes = [
  {
    path: "list", component: TicketListComponent, data: {
      title: 'فهرست تیکت ها',
      id: '#ticketsList',
      isTab: true
    }
  },
  {
    path: "detail", component: TicketDitailListComponent, data: {
      title: 'جزییات تیکت',
      id: '#ticketsList',
      isTab: true
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TicketsRoutingModule { }