using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using WUM.Lib.Extensions;
using WUM.Lib.Interfaces;
using WUM.Lib.Models.DB_Context;
using WUM.Lib.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<JWTBase, JWTServices>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserInfoService, UserInfoService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IRoleService, RoleService>();
//builder.Services.AddScoped<ApiLogFilter>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//編碼器將基本拉丁字元與中日韓字元納入允許範圍不做轉碼
builder.Services.AddSingleton(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
builder.Services.AddDbContext<ManagementContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Management"));
});
builder.Services.SetDapper(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        // You need to import package as follow
        // using Microsoft.IdentityModel.Tokens;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            #region  配置驗證發行者

            ValidateIssuer = true, // 是否要啟用驗證發行者
            ValidIssuer = builder.Configuration.GetSection("JWTConfig").GetValue<string>("Issuer"),

            #endregion

            #region 配置驗證接收方

            ValidateAudience = false, // 是否要啟用驗證接收者
            // ValidAudience = "" // 如果不需要驗證接收者可以註解

            #endregion

            #region 配置驗證Token有效期間

            ValidateLifetime = true, // 是否要啟用驗證有效時間

            #endregion

            #region 配置驗證金鑰

            ValidateIssuerSigningKey = false, // 是否要啟用驗證金鑰，一般不需要去驗證，因為通常Token內只會有簽章

            #endregion

            #region 配置簽章驗證用金鑰

            // 這裡配置是用來解Http Request內Token加密
            // 如果Secret Key跟當初建立Token所使用的Secret Key不一樣的話會導致驗證失敗
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration.GetSection("JWTConfig").GetValue<string>("SignKey")
                )
            )

            #endregion
        };
    });

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
    app.UseWelldoneExceptHandler(loggerFactory);
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
