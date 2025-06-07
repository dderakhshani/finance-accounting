using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Application.CommandQueries.Employee.Query.Get;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
 

namespace Eefa.Admin.Application.CommandQueries.Employee.Command.Update
{
    public class UpdateEmployeeCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.Employee>, ICommand
    {
        public int Id { get; set; }



        /// <summary>
        /// ?? ?????? ????
        /// </summary>
        public int? UnitPositionId { get; set; } = default!;

        /// <summary>
        /// ?? ??????
        /// </summary>
        public string EmployeeCode { get; set; } = default!;

        /// <summary>
        /// ????? ???????
        /// </summary>
        public DateTime? EmploymentDate { get; set; } = default!;

        /// <summary>
        /// ?????
        /// </summary>
        public bool Floating { get; set; } = default!;

        /// <summary>
        /// ????? ??? ???
        /// </summary>
        public DateTime? LeaveDate { get; set; }



        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateEmployeeCommand, Data.Databases.Entities.Employee>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public UpdateEmployeeCommandHandler(IRepository repository, IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Employee>(c =>
            c.ObjectId(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

            entity.UnitPositionId = request.UnitPositionId;
            entity.EmployeeCode = request.EmployeeCode;
            entity.LeaveDate = request.LeaveDate;
            entity.Floating = request.Floating;
            entity.EmploymentDate = request.EmploymentDate;

            var updateEntity = _repository.Update(entity);
            await request.Save(_repository, updateEntity.Entity, cancellationToken);
            return await _mediator.Send(new GetEmployeeQuery() { Id = entity.Id }, cancellationToken);
           
        }
    }
}
