
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


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
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage(); // Enable developer exception page to surface issues clearly
}


// registering the Global Exception Handling Middleware.
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Seed Identity Default Roles and Users
//await app.Services.IdentitySeeder();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
