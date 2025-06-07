using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Application.CommandQueries.Person.Model;
using Eefa.Admin.Application.CommandQueries.PersonAddress.Command.Create;
using Eefa.Admin.Data.Databases.Entities;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace Eefa.Admin.Application.CommandQueries.Person.Command.Create
{
    public class CreatePersonCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreatePersonCommand>, ICommand
    {
        /// <summary>
        /// نام
        /// </summary>
        public string FirstName { get; set; } = ""!;

        /// <summary>
        /// نام خانوادگی
        /// </summary>
        public string LastName { get; set; } = default!;

        /// <summary>
        /// نام پدر
        /// </summary>
        public string? FatherName { get; set; } = default!;

        /// <summary>
        /// شماره ملی
        /// </summary>
        public string NationalNumber { get; set; } = default!;
        public string EconomicCode { get; set; } = default!;

        /// <summary>
        /// شماره شناسنامه
        /// </summary>
        public string? IdentityNumber { get; set; }

        /// <summary>
        /// شماره بیمه
        /// </summary>
        public string? InsuranceNumber { get; set; }

        public string? Email { get; set; }
        public int? AccountReferenceId { get; set; }
        public string AccountReferenceCode { get; set; }

        /// <summary>
        /// تاریخ تولد
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// محل تولد
        /// </summary>
        public int? BirthPlaceCountryDivisionId { get; set; }

        /// <summary>
        /// جنسیت
        /// </summary>
        public int GenderBaseId { get; set; } = default!;

        /// <summary>
        /// حقیقی/ حقوقی
        /// </summary>
        public int? LegalBaseId { get; set; }

        /// <summary>
        /// دولتی/ غیر دولتی
        /// </summary>
        public int? GovernmentalBaseId { get; set; }

        public string ProfileImageReletiveAddress { get; set; }
        public bool TaxIncluded { get; set; } = default!;

        public int AccountReferenceGroupId { get; set; }
        public string? AccountReferenceTitle { get; set; }

        public string? WorkshopCode { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePersonCommand, Data.Databases.Entities.Person>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUpLoader _upLoader;
        private readonly ITejaratBankServices _tjaratBankServices;

        public CreatePersonCommandHandler(IRepository repository, IMapper mapper, IMediator mediator, IUpLoader upLoader, ITejaratBankServices tjaratBankServices)
        {
            _mapper = mapper;
            _mediator = mediator;
            _upLoader = upLoader;
            _repository = repository;
            _tjaratBankServices = tjaratBankServices;
        }


        public async Task<ServiceResult> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            var person = _mapper.Map<Data.Databases.Entities.Person>(request);

       

            var entity = _repository.Insert(person);

            //var accountReferenceGroup = await _repository.GetQuery<AccountReferencesGroup>()
            //    .FirstOrDefaultAsync(x => x.Id == request.AccountReferenceGroupId, cancellationToken: cancellationToken);

            var accountReference = new AccountReference()
            {
                Person = entity.Entity,
                Title = request.AccountReferenceTitle ?? ((person.FirstName ?? "") + " " + person.LastName).Trim(),
                IsActive = true
            };

        
            if (string.IsNullOrEmpty(request.AccountReferenceCode))
            {

                var accountReferenceGroupCode = await _repository.GetQuery<Eefa.Admin.Data.Databases.Entities.AccountReferencesGroup>().Where(x => x.Id == request.AccountReferenceGroupId).Select(x => x.Code).FirstOrDefaultAsync();

                var lastAccountReference = await _repository.GetQuery<Eefa.Admin.Data.Databases.Entities.AccountReference>()
                    .Where(x => x.Code.Length == 6 && x.Code.StartsWith(accountReferenceGroupCode))
                    .OrderByDescending(x => x.Code)
                    .FirstOrDefaultAsync();

                if (lastAccountReference != null && lastAccountReference.Code.EndsWith("9999")) throw new Exception("Maximum code limit has been reached for this accountReference Type");
                if (lastAccountReference == null) lastAccountReference = new AccountReference { Code = accountReferenceGroupCode + "0000" };

                accountReference.Code = (Convert.ToInt32(lastAccountReference.Code) + 1).ToString();
            }
            else
            {
                accountReference.Code = request.AccountReferenceCode;
            }



            // 2024-12-18 14:07 FS 
          var code = accountReference.Code;
            if (accountReference.Code.Length == 6)
                code = accountReference.Code + "0";
          
            accountReference.DepositId = _tjaratBankServices.SetDepositId(code);
            //

            var insertedAccountReference = _repository.Insert(accountReference);


            _repository.Insert(new AccountReferencesRelReferencesGroup()
            {
                ReferenceGroupId = request.AccountReferenceGroupId,
                Reference = insertedAccountReference.Entity
            });



            await _repository.SaveChangesAsync(request.MenueId, cancellationToken);
            return ServiceResult.Success(_mapper.Map<PersonModel>(entity.Entity));

        }

 




    }
}
