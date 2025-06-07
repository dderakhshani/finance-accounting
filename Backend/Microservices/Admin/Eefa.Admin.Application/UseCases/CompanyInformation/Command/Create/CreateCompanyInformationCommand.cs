using System;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.CompanyInformation.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;


namespace Eefa.Admin.Application.CommandQueries.CompanyInformation.Command.Create
{
    public class CreateCompanyInformationCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateCompanyInformationCommand>, ICommand
    {
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// تاریخ انقضاء
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// حداکثر تعداد کابران
        /// </summary>
        public int MaxNumOfUsers { get; set; } = default!;

        /// <summary>
        /// لوگو
        /// </summary>
        public string? Logo { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCompanyInformationCommand, Data.Databases.Entities.CompanyInformation>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateCompanyInformationCommandHandler : IRequestHandler<CreateCompanyInformationCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreateCompanyInformationCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateCompanyInformationCommand request, CancellationToken cancellationToken)
        {
            var entity = _repository.Insert(_mapper.Map<Data.Databases.Entities.CompanyInformation>(request));

            return await request.Save<Data.Databases.Entities.CompanyInformation,CompanyInformationModel>(_repository,_mapper, entity.Entity, cancellationToken);

        }
    }
}
