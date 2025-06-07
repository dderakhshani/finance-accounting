import {Environment} from "./model/environment";

export const environment: Environment = {
  apiURL: 'http://api.eefaceram.local',
  fileServer: 'http://192.168.14.13:50003',
  crmServerAddress: "https://crm-api.eefaceram.com",
  SSRSServerAddress: "https://ssrs.eefaceram.com/default.aspx?ssrsreportname=",
  production: false,
  currentVersion: '1.0.0',
  companyName: 'eefa'
};
