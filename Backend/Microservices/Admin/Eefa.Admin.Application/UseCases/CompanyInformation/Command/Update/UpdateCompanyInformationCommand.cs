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
using Microsoft.EntityFrameworkCore;
 

namespace Eefa.Admin.Application.CommandQueries.CompanyInformation.Command.Update
{
    public class UpdateCompanyInformationCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.CompanyInformation>, ICommand
    {
        public int Id { get; set; }
        /// <summary>
        /// ?????
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// ??? ???????
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// ????? ??????
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// ?????? ????? ??????
        /// </summary>
        public int MaxNumOfUsers { get; set; } = default!;

        /// <summary>
        /// ????
        /// </summary>
        public string? Logo { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateCompanyInformationCommand, Data.Databases.Entities.CompanyInformation>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateCompanyInformationCommandHandler : IRequestHandler<UpdateCompanyInformationCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdateCompanyInformationCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateCompanyInformationCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.CompanyInformation>(c =>
            c.ObjectId(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

            entity.Title = request.Title;
            entity.ExpireDate = request.ExpireDate;
            entity.UniqueName = request.UniqueName;
            entity.MaxNumOfUsers = request.MaxNumOfUsers;
            entity.Logo = request.Logo;

            var updateEntity = _repository.Update(entity);

            if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
            {
                return ServiceResult.Success(_mapper.Map<CompanyInformationModel>(updateEntity.Entity));
            }
            return ServiceResult.Failure();
        }
    }
}
