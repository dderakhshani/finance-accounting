using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;

namespace Eefa.Accounting.Application.UseCases.CodeVoucherExtendType.Command.Create
{
    public class CreateCodeVoucherExtendTypeCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateCodeVoucherExtendTypeCommand>, ICommand
    {
        public string Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCodeVoucherExtendTypeCommand, Data.Entities.CodeVoucherExtendType>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateCodeVoucherExtendTypeCommandHandler : IRequestHandler<CreateCodeVoucherExtendTypeCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreateCodeVoucherExtendTypeCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateCodeVoucherExtendTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = _repository.Insert(_mapper.Map<Data.Entities.CodeVoucherExtendType>(request));

            if (request.SaveChanges)
            {
                if (await _repository.SaveChangesAsync(request.MenueId,cancellationToken) > 0)
                {
                    return ServiceResult.Success(entity.Entity);
                }
            }
            else
            {
                return ServiceResult.Success(entity.Entity);
            }

            return ServiceResult.Failure();
        }
    }
}
