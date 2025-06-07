using System;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Admin.Application.CommandQueries.Year.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using ServiceStack;


namespace Eefa.Admin.Application.CommandQueries.Year.Command.Update
{
    public class UpdateYearCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.Year>, ICommand
    {
        public int Id { get; set; }
        public int YearName { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }
        public bool? IsEditable { get; set; }
        public bool IsCalculable { get; set; }
        public DateTime? LastEditableDate { get; set; }
        public bool IsCurrentYear { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateYearCommand, Data.Databases.Entities.Year>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateYearCommandHandler : IRequestHandler<UpdateYearCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdateYearCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateYearCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.Year>(c =>
            c.ObjectId(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

            if (request.IsCurrentYear)
            {
                if (entity.IsCurrentYear == false)
                {
                    foreach (var year in _repository.GetQuery<Data.Databases.Entities.Year>())
                    {
                        year.IsCurrentYear = false;
                        _repository.Update(year);
                    }
                }
            }

            entity.FirstDate = request.FirstDate;
            entity.LastEditableDate = request.LastEditableDate;
            entity.LastDate = entity.LastDate.AddDays(1).AddSeconds(-1);
            entity.IsEditable = request.IsEditable;
            entity.IsCalculable = request.IsCalculable;
            entity.IsCurrentYear = request.IsCurrentYear;
            entity.YearName = request.YearName;

            var updateEntity = _repository.Update(entity);

            return await request.Save<Data.Databases.Entities.Year,YearModel>(_repository,_mapper, updateEntity.Entity, cancellationToken);

        }
    }
}
