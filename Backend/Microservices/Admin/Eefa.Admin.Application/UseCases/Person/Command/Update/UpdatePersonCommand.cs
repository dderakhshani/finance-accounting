using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Application.CommandQueries.Person.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Admin.Application.CommandQueries.Person.Command.Update
{
    public class UpdatePersonCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.Person>, ICommand
    {
        public int Id { get; set; }

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
        //  public IList<PhoneNumber> Mobiles { get; set; }
        public string? Email { get; set; }
        //public int? AccountReferenceId { get; set; }

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
        public bool TaxIncluded { get; set; } = default!;

        public string ProfileImageReletiveAddress { get; set; }
        public string SignatureImageReletiveAddress { get; set; }

        public string? WorkshopCode { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatePersonCommand, Data.Databases.Entities.Person>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUpLoader _upLoader;

        public UpdatePersonCommandHandler(IRepository repository, IMapper mapper, IUpLoader upLoader)
        {
            _mapper = mapper;
            _upLoader = upLoader;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Person>(c =>
            c.ObjectId(request.Id))
                .Include(x => x.AccountReference)
            .FirstOrDefaultAsync(cancellationToken);

            _mapper.Map(request, entity);
            entity.AccountReference.Title = ((entity.FirstName ?? "") + " " + entity.LastName).Trim();


            var updateEntity = _repository.Update(entity);
            _repository.Update(entity.AccountReference);
            await _repository.SaveChangesAsync(request.MenueId, cancellationToken);

            return ServiceResult.Success(_mapper.Map<PersonModel>(entity));

        }



    }
}
