using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Admin.Application.CommandQueries.PersonFingerprint.Model;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
 

namespace Eefa.Admin.Application.CommandQueries.PersonFingerprint.Command.Update
{
    public class UpdatePersonFingerprintCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Databases.Entities.PersonFingerprint>, ICommand
    {
        public int Id { get; set; }
        /// <summary>
        /// کد پرسنلی
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
        /// شماره انگشت
        /// </summary>
        public int FingerBaseId { get; set; } = default!;
        public IFormFile FingerPrintPhoto { get; set; }
        /// <summary>
        /// الگوی اثر انگشت
        /// </summary>
        public string FingerprintTemplate { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatePersonFingerprintCommand, Data.Databases.Entities.PersonFingerprint>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdatePersonFingerprintCommandHandler : IRequestHandler<UpdatePersonFingerprintCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUpLoader _upLoader;

        public UpdatePersonFingerprintCommandHandler(IRepository repository, IMapper mapper, IUpLoader upLoader)
        {
            _mapper = mapper;
            _upLoader = upLoader;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdatePersonFingerprintCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Data.Databases.Entities.PersonFingerprint>(c =>
            c.ObjectId(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

            if (request.FingerPrintPhoto != null)
            {
                var fingerPrintPhotoUrl = await _upLoader.UpLoadAsync(request.FingerPrintPhoto,
                    Guid.NewGuid().ToString(),
                    CustomPath.PersonProfile, cancellationToken);

                entity.FingerPrintPhotoURL = fingerPrintPhotoUrl.ReletivePath;
            }

            entity.FingerBaseId = request.FingerBaseId;
            entity.FingerPrintPhotoURL = request.FingerprintTemplate;

            var updateEntity = _repository.Update(entity);
            return await request.Save<Data.Databases.Entities.PersonFingerprint,PersonFingerprintModel>(_repository, _mapper,updateEntity.Entity, cancellationToken);

        }
    }
}
