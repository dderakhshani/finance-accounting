using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

    public class GetYearQuery : IRequest<ServiceResult<YearModel>>
    {
        public int Id { get; set; }
    }

    public class GetYearQueryHandler : IRequestHandler<GetYearQuery, ServiceResult<YearModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetYearQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<YearModel>> Handle(GetYearQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _unitOfWork.Years
                                .GetProjectedByIdAsync<YearModel>(request.Id));
        }
    }