using CSO.Core.DatabaseContext;
using CSO.Core.Repositories.BrandRepo;
using CSO.Core.Repositories.CategoryRepo;
using CSO.Core.Repositories.ComplaintTypeRepo;
using CSO.Core.Repositories.CSOClassRepo;
using CSO.Core.Repositories.CSOLogAnalysisRepo;
using CSO.Core.Repositories.CSOLogFileRepo;
using CSO.Core.Repositories.CSOLogHistoryRepo;
using CSO.Core.Repositories.CSOLogRepo;
using CSO.Core.Repositories.DivisionRepo;
using CSO.Core.Repositories.MailMatrixRepo;
using CSO.Core.Repositories.NearestPlantRepo;
using CSO.Core.Repositories.PlantRepo;
using CSO.Core.Repositories.ProductTypeRepo;
using CSO.Core.Repositories.RecycleDayRepo;
using CSO.Core.Repositories.SecurityActionRepo;
using CSO.Core.Repositories.UserRepo;
using CSO.Core.Repositories.UsersRoleRepo;
using CSO.Core.Services.SystemLogs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Configure database connection.
var connstring = builder.Configuration.GetConnectionString("DbConnectionString");
builder.Services.AddDbContext<CSOResponsiveDbContext>(Options => Options.UseSqlServer(connstring));
// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddTransient<ISystemLogService, SystemLogService>();
builder.Services.AddTransient<IUsersRoleRepository, UsersRoleRepository>();
builder.Services.AddTransient<IDivisionRepository, DivisionRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IPlantRepository, PlantRepository>();
builder.Services.AddTransient<IBrandRepository, BrandRepository>();
builder.Services.AddTransient<INearestPlantRepository, NearestPlantRepository>();
builder.Services.AddTransient<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IComplaintTypeRepository, ComplaintTypeRepository>();
builder.Services.AddTransient<ICSOClassRepository, CSOClassRepository>();
builder.Services.AddTransient<IBrandRepository, BrandRepository>();
builder.Services.AddTransient<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddTransient<IPlantRepository, PlantRepository>();
builder.Services.AddTransient<INearestPlantRepository, NearestPlantRepository>();
builder.Services.AddTransient<IRecycleDayRepository, RecycleDayRepository>();
builder.Services.AddTransient<ICSOLogRepository, CSOLogRepository>();
builder.Services.AddTransient<ICSOLogFileRepository, CSOLogFileRepository>();
builder.Services.AddTransient<ICSOLogAnalysisRepository, CSOLogAnalysisRepository>();
builder.Services.AddTransient<IMailMatrixRepository, MailMatrixRepository>();
builder.Services.AddTransient<ISecurityActionRepository, SecurityActionRepository>();
builder.Services.AddTransient<ICSOLogHistoryRepository, CSOLogHistoryRepository>();
builder.Services.AddScoped<IDbConnection>(db => new SqlConnection(connstring));



builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
//builder.Services.AddCors(opt =>
//{
//    opt.AddPolicy("CorsPolicy", builder => builder
//        .AllowAnyHeader()
//        .AllowAnyMethod()
//        .WithOrigins(Configuration.GetSection(Constants.CORS_ORIGINS).Get<string[]>())
//        .AllowCredentials());
//});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();
//app.UseCors("CorsPolicy");
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
