using System;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;


namespace Eefa.Admin.Application.CommandQueries.Year.Command.Create
{
    public class CreateYearCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateYearCommand>, ICommand
    {
        public int YearName { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }
        public bool? IsEditable { get; set; }
        public bool IsCalculable { get; set; }
        public DateTime? LastEditableDate { get; set; }
        public bool IsCurrentYear { get; set; } = default!;
        public int CompanyId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateYearCommand, Data.Databases.Entities.Year>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateYearCommandHandler : IRequestHandler<CreateYearCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public CreateYearCommandHandler(IRepository repository, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateYearCommand request, CancellationToken cancellationToken)
        {
            if (request.IsCurrentYear)
            {
                foreach (var year in _repository.GetQuery<Data.Databases.Entities.Year>())
                {
                    year.IsCurrentYear = false;
                    _repository.Update(year);
                }
            }


            var input = _mapper.Map<Data.Databases.Entities.Year>(request);

            input.LastDate = input.LastDate.AddDays(1).AddSeconds(-1);
            var entity = _repository.Insert(input);
            return await request.Save(_repository, entity.Entity, cancellationToken);

        }
    }
}
