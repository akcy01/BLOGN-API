using BLOGN.Data;
using BLOGN.Data.Repositories.IRepository;
using BLOGN.Data.Repositories.Repository;
using BLOGN.Data.Services;
using BLOGN.Data.Services.IService;
using BLOGN.SharedTools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Services<>));
//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add services to the container.
var connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connString)
);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appSettings = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettings); //appSettingsi AppSettings sýnýfýna dönüþtürüoz.

var appSettingsB = appSettings.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettingsB.Secret); //Secret içerisindeki metni yakaladýk burda.
builder.Services.AddAuthentication(x =>
{
   x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
   x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
   x.RequireHttpsMetadata = false;
   x.SaveToken = true;
   x.TokenValidationParameters = new TokenValidationParameters
   {
       ValidateIssuerSigningKey = true,
       IssuerSigningKey = new SymmetricSecurityKey(key),
       ValidateIssuer = false,
       ValidateAudience = false
   };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
