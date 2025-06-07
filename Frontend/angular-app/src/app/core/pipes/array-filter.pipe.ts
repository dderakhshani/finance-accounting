// // import { Pipe, PipeTransform } from '@angular/core';

// // @Pipe({
// //     name: 'arrayFilter',
// //     pure: false
// // })
// // export class ArrayFilterPipe implements PipeTransform {
// //     transform(items: any[], searchableFields :string[],searchTerm:string): any {
// //         if (!items || !searchTerm) {
// //             return items;
// //         }
// //         // filter items array, items which match and return true will be
// //         // kept, false will be filtered out
// //         return items.filter(item => item.title.indexOf(filter.title) !== -1);
// //     }
// // }



// handleAccountOptions() {
//   let accountHead = <AccountHead>(this.accountManagerService.accountHeads.value.find(x => x.id === this.form.value.creditAccountHeadId));
//   let accountReference = <AccountReference>(this.accountManagerService.accountReferences.value.find(x => x.id === this.form.value.creditAccountReferenceId));
//   let accountReferenceGroup = <AccountReferencesGroup>(this.accountManagerService.accountReferenceGroups.value.find(x => x.id === this.form.creditAccountReferenceGroupId));




//   let noneOfTheFieldsAreSelected = !accountHead && !accountReference && !accountReferenceGroup;
//   if (noneOfTheFieldsAreSelected) {
//     this.creditAccountHeads = this.accountManagerService.accountHeads.value.filter(x => x.lastLevel);
//     this.creditAccountReferences = this.accountManagerService.accountReferences.value.filter(x => x.isActive);
//     this.creditAccountReferenceGroups = this.accountManagerService.accountReferenceGroups.value;
//     this.form.controls['creditAccountHeadId']?.enable({emitEvent: false})
//     this.form.controls['creditAccountReferenceGroupId']?.disable({emitEvent: false})
//     this.form.controls['creditAccountReferenceId']?.enable({emitEvent: false})
//     return;
//   }


//   let onlyAccountHeadSelected = accountHead && !accountReference && !accountReferenceGroup;
//   let accountHeadAndGroupSelected = accountHead && accountReferenceGroup && !accountReference;

//   let onlyAccountReferenceSelected = accountReference && !accountHead && !accountReferenceGroup;
//   let accountReferenceAndGroupSelected = accountReference && !accountHead && accountReferenceGroup;


//   if (onlyAccountHeadSelected) {
//     this.creditAccountReferenceGroups = this.accountManagerService.getGroupsRelatedToAccountHead(accountHead.id);

//     this.form.controls['creditAccountReferenceId']?.disable({emitEvent: false})
//     if (this.creditAccountReferenceGroups.length > 0) this.form.controls['creditAccountReferenceGroupId']?.enable({emitEvent: false})
//     else this.form.controls['creditAccountReferenceGroupId']?.disable({emitEvent: false})
//   }

//   if (accountHeadAndGroupSelected) {
//     this.creditAccountReferences = this.accountManagerService.getAccountReferencesRelatedToGroup(accountReferenceGroup.id).filter(x => x.isActive)
//     this.form.controls['creditAccountReferenceId']?.enable({emitEvent: false});
//   }

//   if (onlyAccountReferenceSelected) {
//     this.creditAccountReferenceGroups = this.accountManagerService.getGroupsRelatedToAccountReference(accountReference.id)
//     this.form.controls['creditAccountReferenceGroupId']?.enable({emitEvent: false})
//     this.form.controls['creditAccountHeadId']?.disable({emitEvent: false})
//     if (this.creditAccountReferenceGroups.length === 0) this.form.controls['creditAccountReferenceGroupId']?.disable({emitEvent: false})
//   }

//   if (accountReferenceAndGroupSelected) {
//     this.creditAccountHeads = this.accountManagerService.getAccountHeadsRelatedToGroup(accountReferenceGroup.id).filter(x => x.lastLevel)
//     this.form.controls['creditAccountHeadId']?.enable({emitEvent: false});
//   }
// }
