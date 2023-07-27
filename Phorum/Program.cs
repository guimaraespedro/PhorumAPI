using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Phorum.Entities;
using Phorum.Identity;
using System.Text;
using Microsoft.OpenApi.Models;
using Phorum.Services;
using Phorum.Helpers;
using Phorum.Repositories.UserRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options => options.AddPolicy("corsPolicy", policy =>
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod()
        ));

builder.Services.AddControllers();
builder.Services.AddDbContext<PhorumContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("connectionString")));


var jwtOptions = new JwtOptions();
builder.Configuration.GetSection("jwt").Bind(jwtOptions);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwtOptions.JwtIssuer,
            ValidAudience = jwtOptions.JwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey((Encoding.UTF8.GetBytes(jwtOptions.JwtKey))),
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = true
        };
    });

//Adding Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<HttpContextHelper>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Type = SecuritySchemeType.ApiKey,
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Scheme = "Bearer",

        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                },
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Name = "Authorization",
                Scheme = "Bearer",
            },
            new string[] {}
        }
    });


    }
 );

var app = builder.Build();

app.UseCors("corsPolicy");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

