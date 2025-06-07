using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Application.CommandQueries.BaseValueType.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
 

namespace Eefa.Admin.Application.CommandQueries.BaseValueType.Command.Update
{
    public class UpdateBaseValueTypeCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.BaseValueType>, ICommand
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }

        /// <summary>
        /// ?????
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// ??? ???????
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// ??? ????
        /// </summary>
        public string? GroupName { get; set; }

        /// <summary>
        /// ??? ??? ???? ?????? ????
        /// </summary>
        public bool IsReadOnly { get; set; } = default!;

        /// <summary>
        /// ??? ?????
        /// </summary>
        public string? SubSystem { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateBaseValueTypeCommand, Data.Databases.Entities.BaseValueType>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateBaseValueTypeCommandHandler : IRequestHandler<UpdateBaseValueTypeCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdateBaseValueTypeCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateBaseValueTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.BaseValueType>(c =>
            c.ObjectId(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

            entity.UniqueName = request.UniqueName;
            entity.GroupName = request.GroupName;
            entity.IsReadOnly = request.IsReadOnly;
            entity.SubSystem = request.SubSystem;
            entity.Title = request.Title;

            var updateEntity = _repository.Update(entity);

            return await request.Save<Data.Databases.Entities.BaseValueType, BaseValueTypeModel>(_repository,_mapper, updateEntity.Entity, cancellationToken);

        }
    }
}
