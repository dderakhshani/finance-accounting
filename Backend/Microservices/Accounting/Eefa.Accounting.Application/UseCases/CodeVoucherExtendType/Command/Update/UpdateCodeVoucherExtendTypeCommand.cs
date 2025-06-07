using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Accounting.Application.UseCases.CodeVoucherExtendType.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.CodeVoucherExtendType.Command.Update
{
    public class UpdateCodeVoucherExtendTypeCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Entities.CodeVoucherExtendType>, ICommand
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateCodeVoucherExtendTypeCommand, Data.Entities.CodeVoucherExtendType>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateCodeVoucherExtendTypeCommandHandler : IRequestHandler<UpdateCodeVoucherExtendTypeCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdateCodeVoucherExtendTypeCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateCodeVoucherExtendTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Entities.CodeVoucherExtendType>(c =>
            c.ObjectId(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

            entity.Title = request.Title;

            _repository.Update(entity);

            if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
            {
                return ServiceResult.Success(_mapper.Map<CodeVoucherExtendTypeModel>(entity));
            }
            return ServiceResult.Failure();
        }
    }
}
