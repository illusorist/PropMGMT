using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PropertyManagement.API.Middleware;
using PropertyManagement.Application.Configuration;
using PropertyManagement.Application.Interfaces;
using PropertyManagement.Application.Services;
using PropertyManagement.Infrastructure.Data;
using PropertyManagement.Infrastructure.Repositories;
using PropertyManagement.Infrastructure.Storage;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<PropertyImageUploadOptions>(builder.Configuration.GetSection("PropertyImageUpload"));
builder.Services.Configure<LeadImageUploadOptions>(builder.Configuration.GetSection("LeadImageUpload"));

builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
builder.Services.AddScoped<IPropertyImageStorage, DiskPropertyImageStorage>();
builder.Services.AddScoped<ILeadImageRepository, LeadImageRepository>();
builder.Services.AddScoped<ILeadImageStorage, DiskLeadImageStorage>();
builder.Services.AddScoped<ILeadRepository, LeadRepository>();
builder.Services.AddScoped<IBuyerClientRepository, BuyerClientRepository>();
builder.Services.AddScoped<IPropertySaleRepository, PropertySaleRepository>();
builder.Services.AddScoped<IAmenityRepository, AmenityRepository>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<OwnerService>();
builder.Services.AddScoped<OwnerAccountService>();
builder.Services.AddScoped<OwnerStatsService>();
builder.Services.AddScoped<PropertyService>();
builder.Services.AddScoped<PropertyImageService>();
builder.Services.AddScoped<LeadService>();
builder.Services.AddScoped<BuyerClientService>();
builder.Services.AddScoped<PropertySaleService>();
builder.Services.AddScoped<AmenityService>();
builder.Services.AddScoped<TenantService>();
builder.Services.AddScoped<ContractService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserAccountService>();

var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:8080")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.MapOpenApi();
app.MapScalarApiReference();

app.UseMiddleware<ErrorHandlingMiddleware>();

var imageOptions = app.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<PropertyImageUploadOptions>>().Value;
var imageRoot = System.IO.Path.IsPathRooted(imageOptions.RootPath)
    ? imageOptions.RootPath
    : System.IO.Path.GetFullPath(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), imageOptions.RootPath));
System.IO.Directory.CreateDirectory(imageRoot);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imageRoot),
    RequestPath = imageOptions.PublicBasePath
});

// CORS
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


