using MISA.AMIS.BL;
using MISA.AMIS.BL.BaseBL;
using MISA.AMIS.DL;
using MISA.AMIS.DL.BaseDL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Dêpndency Injection
builder.Services.AddScoped( typeof(IBaseDL<>), typeof(BaseDL<>));
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));
builder.Services.AddScoped<IEmployeeBL, EmployeeBL>();
builder.Services.AddScoped<IEmployeeDL, EmployeeDL>();

//Lấy dữ liệu ConnectionString từ file AppSetting
DatabaseContext.ConnectionString = builder.Configuration.GetConnectionString("MySql");

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

builder.Services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
