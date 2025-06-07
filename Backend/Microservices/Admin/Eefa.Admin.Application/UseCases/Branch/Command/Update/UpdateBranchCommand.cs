using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Application.CommandQueries.Branch.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Admin.Application.CommandQueries.Branch.Command.Update
{
    public class UpdateBranchCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.Branch>, ICommand
    {
        public int Id { get; set; }
        
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateBranchCommand, Data.Databases.Entities.Branch>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateBranchCommandHandler : IRequestHandler<UpdateBranchCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdateBranchCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Branch>(c =>
            c.ObjectId(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

            entity.ParentId = request.ParentId;
            entity.Title = request.Title;
            entity.Lng = request.Lng;
            entity.Lat = request.Lat;

            var updateEntity = _repository.Update(entity);

            if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
            {
                return ServiceResult.Success(_mapper.Map<BranchModel>(updateEntity.Entity));
            }
            return ServiceResult.Failure();
        }
    }
}
