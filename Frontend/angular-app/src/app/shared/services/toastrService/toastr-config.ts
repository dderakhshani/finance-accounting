import {IndividualConfig} from "ngx-toastr";

export interface ToastrConfig {

  title?: string;
  message: string;
  type?: 'success' | 'error' | 'info' | 'warning';
  position?: 'top-left' | 'top-center' | 'top-right' | 'bottom-left' | 'bottom-center' | 'bottom-right';
  options?: Partial<IndividualConfig>;
}
