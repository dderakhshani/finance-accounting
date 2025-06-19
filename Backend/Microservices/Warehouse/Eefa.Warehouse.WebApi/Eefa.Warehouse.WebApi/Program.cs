using Eefa.Warehouse.Infrastructure.Data.Context;
using SharedCode;

var builder = WebApplication.CreateBuilder(args);

SharedProgram.SetupRun<WarehouseUnitOfWork>(builder,builder.Configuration);
