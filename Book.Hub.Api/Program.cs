using Books.Hub.Application.Interfaces.IServices.Comman;
using Books.Hub.Application.Services.Comman;
using Books.Hub.Appliction.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExecutionTimeFilter>(); // filter to log every request execution time  
    options.Filters.Add<PerformanceActionFilter>();
}); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.EnableAnnotations();


    c.SwaggerDoc("v1", new() { Title = "after.senatone API", Version = "v1" });

    // Add JWT bearer auth to Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token in the format: Bearer {your token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
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

// add ImageKit Service 
builder.Services.AddSingleton<IImageUploadService, ImageUploadService>();

// Add JWT authentication in program.cs
builder.Services.AddJwtAuthentication();

// Inject System Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IBaseService, BaseService>();

builder.Services.AddScoped<ISubscriberService, SubscriberService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

// Add Action Filter that calculate the time excution
builder.Services.AddScoped<ExecutionTimeFilter>();
builder.Services.AddScoped<PerformanceActionFilter>();

// Confiqurate Options
// Bind JwtOptions once and validate it
builder.Services.AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection("JWT"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

//builder.Services.Configure<ImagesOptions>(builder.Configuration.GetSection("ImageSettings"));
builder.Services.AddOptions<ImagesOptions>()
    .Bind(builder.Configuration.GetSection("ImageSettings"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Serilog Config
Log.Logger = new
LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();

// Replace default logging with Serilog
builder.Host.UseSerilog();

// CORS for Next.js frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("NextJsPolicy", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200"
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage(); // Enable developer exception page to surface issues clearly
}

//app.UseSwagger();
//app.UseSwaggerUI(options =>
//{
//    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Production A");
//    options.RoutePrefix = string.Empty;
//});

// registering the Global Exception Handling Middleware.
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseCors("NextJsPolicy");

app.UseDeveloperExceptionPage(); // Enable developer exception page to surface issues clearly

app.UseSerilogRequestLogging();  // Logs HTTP requests automatically

// Logs how long the request took (in milliseconds) using the ILogger
app.UseMiddleware<ProfilingMiddleware>();

// Rate Limiting Middleware 
app.UseMiddleware<RateLimitingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Seed Identity Default Roles and Users
await app.Services.SeedIdentityAsync();

app.Run();

