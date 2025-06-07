import {PersonAddress} from "./person-address";
import {PersonFingerprint} from "./person-fingerprint";
import {PersonPhone} from "./person-phone";
import {PersonBankAccount} from "./person-bank-account";
import {PersonCustomer} from "./person-customer";

export class Person {
  public id!:number;
  public selected!:boolean;
  public firstName!:string;
  public lastName!:string;
  public nationalNumber!:string;
  public economicCode!:string;
  public accountReferenceId!:number;
  public accountReferenceCode!:number;
  public birthDate!:Date;
  public birthPlaceCountryDivisionId!:number;
  public email!:string;
  public fatherName!:string;
  public genderBaseId!:number;
  public genderBaseTitle!:number;
  public governmentalBaseId!:number;
  public governmentalBaseTitle!:number;
  public identityNumber!:string;
  public insuranceNumber!:string;
  public legalBaseId!:number;
  public mobileJson!:string;
  public personAddressList:PersonAddress[] = [];
  public personFingerprintsList:PersonFingerprint[] = [];
  public personPhonesList:PersonPhone[] = []
  public personBankAccountsList:PersonBankAccount[] = []
  public personCustomer!:PersonCustomer;
  public photoURL!:string;
  public signatureURL!:string;
  public taxIncluded!:boolean;
  public employeeCode!:string;
  public createdBy!:string;
  public createdAt!:Date;
  public modifiedBy!:string;
  public modifiedAt!:Date;
  public depositId !:string;
  public address !:string;
  public postalCode !:string;
  public phoneNumber !:string;

}
