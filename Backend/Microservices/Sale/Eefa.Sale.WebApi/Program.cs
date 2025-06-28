using Eefa.Sale.Infrastructure.Data.Context;
using SharedCode;

var builder = WebApplication.CreateBuilder(args);

SharedProgram.SetupRun<SaleUnitOfWork>(builder, builder.Configuration);
