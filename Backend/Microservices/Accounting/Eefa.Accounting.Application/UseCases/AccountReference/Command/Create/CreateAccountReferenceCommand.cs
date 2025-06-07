using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Eefa.Accounting.Application.UseCases.AccountReference.Model;

namespace Eefa.Accounting.Application.UseCases.AccountReference.Command.Create
{
    public class CreateAccountReferenceCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateAccountReferenceCommand>, ICommand
    {
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool IsActive { get; set; } = default!;

        public string Code { get; set; }
        public string? Description { get; set; }
        public int AccountReferenceTypeBaseId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateAccountReferenceCommand, Data.Entities.AccountReference>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateAccountReferenceCommandHandler : IRequestHandler<CreateAccountReferenceCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreateAccountReferenceCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateAccountReferenceCommand request, CancellationToken cancellationToken)
        {
            var entity = new Data.Entities.AccountReference()
            {
                Title = request.Title,
                IsActive = request.IsActive,
                Description = request.Description
            };


            if (string.IsNullOrEmpty(request.Code))
            {

                var accountReferencesType = await _repository.GetQuery<Data.Entities.BaseValue>().FirstOrDefaultAsync(x => x.Id == request.AccountReferenceTypeBaseId);

                var lastAccountReference = await _repository.GetQuery<Data.Entities.AccountReference>()
                    .Where(x => x.Code.Length == 6 && x.Code.StartsWith(accountReferencesType.Value))
                    .OrderByDescending(x => x.Code)
                    .FirstOrDefaultAsync();

                if (lastAccountReference.Code.EndsWith("9999")) throw new Exception("Maximum code limit has been reached for this accountReference Type");
                if (lastAccountReference == null) lastAccountReference = new Data.Entities.AccountReference { Code = accountReferencesType.Value + "0000" };

                entity.Code = (Convert.ToInt32(lastAccountReference.Code) + 1).ToString();
            }
            else
            {
                entity.Code = request.Code;
            }

            _repository.Insert(entity);

            if (request.SaveChanges)
            {
                if (await _repository.SaveChangesAsync(request.MenueId, cancellationToken) > 0)
                {
                    return ServiceResult.Success(_mapper.Map<AccountReferenceModel>(entity));
                }
            }
            else
            {
                return ServiceResult.Success(entity);
            }

            return ServiceResult.Failure();
        }
    }
}
