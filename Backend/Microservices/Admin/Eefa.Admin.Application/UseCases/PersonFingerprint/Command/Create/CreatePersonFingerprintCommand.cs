using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace Eefa.Admin.Application.CommandQueries.PersonFingerprint.Command.Create
{
    public class CreatePersonFingerprintCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreatePersonFingerprintCommand>, ICommand
    {
        /// <summary>
        /// کد پرسنلی
        /// </summary>
        public int PersonId { get; set; } = default!;
        public IFormFile FingerPrintPhoto { get; set; }
        /// <summary>
        /// شماره انگشت
        /// </summary>
        public int FingerBaseId { get; set; } = default!;

        /// <summary>
        /// الگوی اثر انگشت
        /// </summary>
        public string FingerPrintTemplate { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreatePersonFingerprintCommand, Data.Databases.Entities.PersonFingerprint>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreatePersonFingerprintCommandHandler : IRequestHandler<CreatePersonFingerprintCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUpLoader _upLoader;

        public CreatePersonFingerprintCommandHandler(IRepository repository, IMapper mapper, IUpLoader upLoader)
        {
            _mapper = mapper;
            _upLoader = upLoader;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreatePersonFingerprintCommand request, CancellationToken cancellationToken)
        {
            var fingerPrint = _mapper.Map<Data.Databases.Entities.PersonFingerprint>(request);

            if (request.FingerPrintPhoto is not null)
            {
                var fingerPrintPhotoUrl = await _upLoader.UpLoadAsync(request.FingerPrintPhoto,
                    Guid.NewGuid().ToString(),
                    CustomPath.Temp, cancellationToken);

                fingerPrint.FingerPrintPhotoURL = fingerPrintPhotoUrl.ReletivePath;
            }

            var entity = _repository.Insert(fingerPrint);

            return await request.Save(_repository, entity.Entity, cancellationToken);

        }
    }
}
