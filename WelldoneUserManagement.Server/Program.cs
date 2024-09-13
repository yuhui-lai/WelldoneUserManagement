using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using WUM.Lib.Extensions;
using WUM.Lib.Filters;
using WUM.Lib.Interfaces;
using WUM.Lib.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<JWTBase, JWTServices>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserInfoService, UserInfoService>();
//builder.Services.AddScoped<ApiLogFilter>();

builder.Services.AddControllers(options =>
{
    //options.Filters.Add<ApiLogFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//�s�X���N�򥻩ԤB�r���P�������r���ǤJ���\�d�򤣰���X
builder.Services.AddSingleton(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
builder.Services.SetDapper(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        // You need to import package as follow
        // using Microsoft.IdentityModel.Tokens;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            #region  �t�m���ҵo���

            ValidateIssuer = true, // �O�_�n�ҥ����ҵo���
            ValidIssuer = builder.Configuration.GetSection("JWTConfig").GetValue<string>("Issuer"),

            #endregion

            #region �t�m���ұ�����

            ValidateAudience = false, // �O�_�n�ҥ����ұ�����
            // ValidAudience = "" // �p�G���ݭn���ұ����̥i�H����

            #endregion

            #region �t�m����Token���Ĵ���

            ValidateLifetime = true, // �O�_�n�ҥ����Ҧ��Įɶ�

            #endregion

            #region �t�m���Ҫ��_

            ValidateIssuerSigningKey = false, // �O�_�n�ҥ����Ҫ��_�A�@�뤣�ݭn�h���ҡA�]���q�`Token���u�|��ñ��

            #endregion

            #region �t�mñ�����ҥΪ��_

            // �o�̰t�m�O�ΨӸ�Http Request��Token�[�K
            // �p�GSecret Key����إ�Token�ҨϥΪ�Secret Key���@�˪��ܷ|�ɭP���ҥ���
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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
