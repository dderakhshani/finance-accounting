using Eefa.Sale.Infrastructure.Data.Context;
using SharedCode;

var builder = WebApplication.CreateBuilder(args);

SharedProgram.SetupRun<SaleDbContext>(builder, builder.Configuration);
