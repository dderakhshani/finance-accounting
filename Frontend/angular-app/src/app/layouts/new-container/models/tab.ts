import {Route} from "@angular/router";

export interface Tab {
  title: string
  component: any
  active: boolean
  instanceRoute: string
  actualRoute: string
  id: number
  guid: string
  isLoading: boolean
}
