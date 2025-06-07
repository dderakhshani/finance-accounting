using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Contract.AdminWorkflow;
using Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities;
using Eefa.WorkflowAdmin.WebApi.Domain.Databases.SqlServer.Context;
using Library.ConfigurationAccessor.Mappings;
using Library.Interfaces;
using Library.Models;
using Library.Resources;
using LinqToDB;
using ServiceStack;
using static LinqToDB.Common.Configuration;

namespace Eefa.WorkflowAdmin.WebApi.Application
{
    public class CreateCorrectionRequestModel : IMapFrom<CreateCorrectionRequestModel>, IMapFrom<CreateCorrectionRequestEvent>
    {
        public short Status { get; set; } = default!;
        public short Type { get; set; } = default!;
        public int? DocumentId { get; set; } 
        public string Changes { get; set; }
        public int VerifierUserId { get; set; } = default!;
        public int MenuId { get; set; } = default!;
        public int? CorrectedId { get; set; }
        public string? Discription { get; set; }
        public string PayLoad { get; set; }
        public string CallBack { get; set; }
        public string CommandName { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCorrectionRequestModel, CorrectionRequest>()
                .IgnoreAllNonExisting();
            profile.CreateMap<CreateCorrectionRequestEvent, CreateCorrectionRequestModel>()
                .IgnoreAllNonExisting();
        }
    }

    public interface ICorrectionRequestService
    {
        Task Create(CreateCorrectionRequestModel createCorrectionRequestModel);
        Task UpdateCorrectionRequest(int correctionRequestId, short status, string description);
        Task<List<CorrectionRequest>> GetUserTasks(int id);

    }

    public class CorrectionRequestService : ICorrectionRequestService
    {
        private readonly IRepository _repository;
        private readonly IResourceFactory _resourceFactory;
        private readonly IMapper _mapper;
        private readonly IMessageBus _messageBus;
        public CorrectionRequestService(IRepository repository, IResourceFactory resourceFactory, IMapper mapper, IMessageBus messageBus)
        {
            _repository = repository;
            _resourceFactory = resourceFactory;
            _mapper = mapper;
            _messageBus = messageBus;
        }

        public async Task<List<CorrectionRequest>> GetUserTasks(int id)
        {
            return await _repository.GetQuery<CorrectionRequest>().Where(x => x.VerifierUserId == id).ToListAsync();
        }

        public async Task Create(CreateCorrectionRequestModel createCorrectionRequestModel)
        {
            var correctionRequest = _mapper.Map<CorrectionRequest>(createCorrectionRequestModel);

            _repository.Insert(correctionRequest);
            await _repository.SaveChangesAsync();
        }



        public async Task UpdateCorrectionRequest(int correctionRequestId, short status, string description)
        {
            var request = await _repository.GetQuery<CorrectionRequest>().FirstAsync(x => x.Id == correctionRequestId);
            request.Status = status;
            request.Description = description;
            _repository.Update(request);
            await _repository.SaveChangesAsync();


            var contractAssembly = AppDomain.CurrentDomain.GetAssemblies().Single(x => x.FullName != null && x.FullName.Contains("Eefa.Contract"));

            var callBack = contractAssembly.GetTypes().Single(x => x.FullName != null
                                                                   && x.FullName.Contains(
                                                                       "UpdateVoucherHeadCorrectionRequestCallBackRequestEvent")
                                                                   && x.IsClass
                                                                   && (typeof(IEvent)).IsAssignableFrom(x));
            var c = Activator.CreateInstance(callBack)!;
            c.GetType().GetProperty("Status")?.SetValue(c, 1);
            c.GetType().GetProperty("PayLoad")?.SetValue(c, request.PayLoad);

            var o = new object[]{
                c as IEvent,null
            };

            var method = _messageBus.GetType().GetMethod("Publish")?.MakeGenericMethod(new Type[] { c.GetType() });
            ((Task)method.Invoke(_messageBus, o)).GetAwaiter().GetResult();
        }
    }
}