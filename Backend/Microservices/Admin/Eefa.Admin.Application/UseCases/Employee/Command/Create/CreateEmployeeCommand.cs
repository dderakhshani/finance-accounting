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
 

namespace Eefa.Admin.Application.CommandQueries.Employee.Command.Create
{
    public class CreateEmployeeCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateEmployeeCommand>, ICommand
    {
        /// <summary>
        /// کد پرسنلی
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
        /// کد موقعیت واحد
        /// </summary>
        public int? UnitPositionId { get; set; } = default!;

        /// <summary>
        /// کد پرسنلی
        /// </summary>
        public string EmployeeCode { get; set; } = default!;

        /// <summary>
        /// تاریخ استخدام
        /// </summary>
        public DateTime? EmploymentDate { get; set; } = default!;



        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateEmployeeCommand, Data.Databases.Entities.Employee>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateEmployeeCommandHandler(IRepository repository, IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = _repository.Insert(_mapper.Map<Data.Databases.Entities.Employee>(request));
            await request.Save(_repository, entity.Entity, cancellationToken);
            return await _mediator.Send(new GetEmployeeQuery() { Id = entity.Entity.Id }, cancellationToken);
           
        }
    }
}
