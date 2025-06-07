using Eefa.Common.CommandQuery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Admin.Application.Refactor.UseCases.Person.Command.Update
{
    public class UpdateDepositPersonCommand : CommandBase , IRequest<ServiceResult<bool>> , ICommand
    {
        public List<string> AccountReferences { get; set; }

        public class UpdateDepositPersonCommandHandler : IRequestHandler<UpdateDepositPersonCommand, ServiceResult<bool>>
        {
            private readonly IUnitOfWork _unitOfWork;
            public UpdateDepositPersonCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ServiceResult<bool>> Handle(UpdateDepositPersonCommand request, CancellationToken cancellationToken)
            {

                var references = await _unitOfWork.AccountReferences.GetListAsync(x=>request.AccountReferences.Contains(x.Code));

                foreach (var reference in references)
                {
                    reference.DepositId = SetDepositId(reference.Code);
                    _unitOfWork.AccountReferences.Update(reference);
                }
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                return ServiceResult.Success(true);

            }




            //TODO : Put it into Lib Fs
            private string SetDepositId(string inputNumber)
            {
                int[] digits = Array.ConvertAll(inputNumber.ToCharArray(), c => (int)char.GetNumericValue(c));

                int sum1 = 0;
                for (int i = 0; i < digits.Length; i++)
                {
                    sum1 += digits[i] * (i + 1);
                }

                int n1 = (sum1 + 11) % 11;
                int checkDigit = (n1 < 9) ? n1 : 0;

                string result = inputNumber + checkDigit;

                return result;
            }


        }

    }
}
