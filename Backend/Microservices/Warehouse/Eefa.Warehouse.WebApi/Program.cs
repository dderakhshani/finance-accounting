using Eefa.Warehouse.Infrastructure.Data.Context;
using SharedCode;

var builder = WebApplication.CreateBuilder(args);

SharedProgram.SetupRun<WarehouseDbContext>(builder,builder.Configuration);
