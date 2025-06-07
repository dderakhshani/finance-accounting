using Eefa.Admin.Data.Databases.Entities;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Admin.Application.UseCases.Employee.Command.UpdateEmployeesFromSina
{
    public class UpdateEmployeesFromSinaCommand : IRequest<ServiceResult>
    {
    }

    public class UpdateEmployeesFromSinaCommandHandler : IRequestHandler<UpdateEmployeesFromSinaCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public UpdateEmployeesFromSinaCommandHandler(IRepository repository, ICurrentUserAccessor currentUserAccessor)
        {
            _repository = repository;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<ServiceResult> Handle(UpdateEmployeesFromSinaCommand request, CancellationToken cancellationToken)
        {
            using var client = new HttpClient();

            var address = new Uri("https://sina.eefaceram.com/prime/Person/GetInfoPersonsFromSina?securityCode=2025123");

            var response = await client.GetAsync(address);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {


                var sinaEmployees = JsonConvert.DeserializeObject<List<SinaEmployee>>(await response.Content.ReadAsStringAsync());

                var mobileBaseValueId = await _repository.GetQuery<Data.Databases.Entities.BaseValue>().Where(x => x.UniqueName == "Mobile").Select(x => x.Id).FirstOrDefaultAsync();
                var workplaceBaseValueId = await _repository.GetQuery<Data.Databases.Entities.BaseValue>().Where(x => x.UniqueName == "workplace").Select(x => x.Id).FirstOrDefaultAsync();
                var genderBaseId = await _repository.GetQuery<Data.Databases.Entities.BaseValue>().Where(x => x.UniqueName == "Man").Select(x => x.Id).FirstOrDefaultAsync();
                var legalBaseId = await _repository.GetQuery<Data.Databases.Entities.BaseValue>().Where(x => x.UniqueName == "Haghighi").Select(x => x.Id).FirstOrDefaultAsync();
                var nonGovernmentalBaseId = await _repository.GetQuery<Data.Databases.Entities.BaseValue>().Where(x => x.UniqueName == "Non Governmental").Select(x => x.Id).FirstOrDefaultAsync();
                var countryDivisions = await _repository.GetQuery<Data.Databases.Entities.CountryDivision>().ToListAsync();
                var factoryReferenceGroup = await _repository.GetQuery<Data.Databases.Entities.AccountReferencesGroup>().FirstOrDefaultAsync(x => x.Code == "42");

                sinaEmployees = sinaEmployees.OrderByDescending(x => x.EmployeeCode).ToList();
                foreach (var item in sinaEmployees)
                {
                    var doesEmployeeExist = await _repository.GetQuery<Data.Databases.Entities.Employee>().AnyAsync(x => x.EmployeeCode == item.EmployeeCode);
                    if (!doesEmployeeExist)
                    {
                        var employee = new Data.Databases.Entities.Employee
                        {
                            EmployeeCode = item.EmployeeCode,
                            EmploymentDate = TimeZoneInfo.ConvertTimeToUtc(new PersianCalendar().ToDateTime(int.Parse(item.EmploymentDate.Split('/')[0]), int.Parse(item.EmploymentDate.Split('/')[1]), int.Parse(item.EmploymentDate.Split('/')[2]), 0, 0, 0, 0)),
                            CreatedById = _currentUserAccessor.GetId(),
                            CreatedAt = DateTime.UtcNow,
                            OwnerRoleId = _currentUserAccessor.GetRoleId()
                        };

                        var personId = await _repository.GetQuery<Data.Databases.Entities.Person>().Where(x => x.NationalNumber == item.NationalCode).Select(x => x.Id).FirstOrDefaultAsync();
                        if (personId != default) employee.PersonId = personId;
                        else
                        {
                            employee.Person = new Data.Databases.Entities.Person
                            {
                                NationalNumber = item.NationalCode,
                                LastName = item.FullName,
                                GenderBaseId = genderBaseId,
                                LegalBaseId = legalBaseId,
                                GovernmentalBaseId = nonGovernmentalBaseId,
                                CreatedById = _currentUserAccessor.GetId(),
                                CreatedAt = DateTime.UtcNow,
                                OwnerRoleId = _currentUserAccessor.GetRoleId(),
                                AccountReference = new Data.Databases.Entities.AccountReference
                                {
                                    Code = item.NationalCode,
                                    Title = item.FullName,
                                    IsActive = true,
                                    CreatedById = _currentUserAccessor.GetId(),
                                    CreatedAt = DateTime.UtcNow,
                                    OwnerRoleId = _currentUserAccessor.GetRoleId(),
                                    AccountReferencesRelReferencesGroups = new List<AccountReferencesRelReferencesGroup>()
                                    {
                                      new AccountReferencesRelReferencesGroup
                                      {
                                          ReferenceGroupId = factoryReferenceGroup.Id,
                                          CreatedById = _currentUserAccessor.GetId(),
                                          CreatedAt = DateTime.UtcNow,
                                          OwnerRoleId = _currentUserAccessor.GetRoleId(),
                                      }
                                    }

                                },
                                PersonPhones = new List<PersonPhone>()
                                {
                                    new PersonPhone
                                    {
                                        PhoneNumber = item.Mobile,
                                        PhoneTypeBaseId = mobileBaseValueId,
                                        CreatedById = _currentUserAccessor.GetId(),
                                        CreatedAt = DateTime.UtcNow,
                                        OwnerRoleId = _currentUserAccessor.GetRoleId()
                                    }
                                },
                                PersonAddresses = new List<PersonAddress>()
                                {
                                    new PersonAddress
                                    {
                                        Address = item.Address,
                                        CountryDivisionId = !string.IsNullOrEmpty(item.Address) ? countryDivisions.Where(x => (item.Address.Contains(x.OstanTitle) || item.Address.Contains(x.ShahrestanTitle)) || ( x.OstanTitle == "اردکان" ||  x.ShahrestanTitle == "اردکان")).Select(x => x.Id).FirstOrDefault() : countryDivisions.FirstOrDefault(x =>  x.OstanTitle == "اردکان" ||  x.ShahrestanTitle == "اردکان")?.Id,
                                        TypeBaseId = workplaceBaseValueId,
                                        CreatedById = _currentUserAccessor.GetId(),
                                        CreatedAt = DateTime.UtcNow,
                                        OwnerRoleId = _currentUserAccessor.GetRoleId()
                                    }
                                }
                            };
                        }

                        _repository.Insert(employee);
                    }
                    else continue;
                }
                await _repository.SaveChangesAsync();

            }

            return ServiceResult.Success();

        }
    }
}
