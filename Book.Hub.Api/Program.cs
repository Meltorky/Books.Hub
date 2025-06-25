using Books.Hub.Api.Extentions;
using Books.Hub.Application.Comman;
using Books.Hub.Application.Interfaces.IService.Admin;
using Books.Hub.Application.Interfaces.IServices.Authentication;
using Books.Hub.Application.Services.Authentication;
using Books.Hub.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Books.Hub.Application.Service.Admin;
using Books.Hub.Application.Interfaces.IRepositories.Admin;
using Books.Hub.Infrastructure.Repositories.Admin;
using Books.Hub.Application.Options;
using Books.Hub.Application.Interfaces.IServices.Admin;
using Books.Hub.Application.Services.Admin;
using Books.Hub.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Database Connection Service
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));


// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(o =>
{
    o.Password.RequireDigit = true;
    o.Password.RequireLowercase = true;
    o.Password.RequireUppercase = true;
    o.Password.RequireNonAlphanumeric = true;
    o.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<AppDbContext>();

// Add JWT authentication in program.cs
builder.Services.AddJwtAuthentication(builder.Configuration);

// Inject System Services
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<ITokenService,TokenService>();

builder.Services.AddScoped<IAuthorService,AuthorService>();
builder.Services.AddScoped<IAuthorRepository,AuthorRepository>();

builder.Services.AddScoped<ICategoryService,CategoryService>();
builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IBaseService,BaseService>();

// Confiqurate Options
builder.Services.Configure<ImagesOptions>(builder.Configuration.GetSection("ImageSettings"));

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations(); // Required for SwaggerSchema to work
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Seed Identity Default Roles and Users
await app.Services.IdentitySeeder();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
