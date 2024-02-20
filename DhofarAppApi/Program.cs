using DhofarAppApi.Model;
using DhofarAppWeb.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Services;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Data;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using MediaBrowser.Model.Services;

namespace DhofarAppWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
          );

            // Add services to the container.
            string? connString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services
                .AddDbContext<AppDbContext>
                (option => option.UseSqlServer(connString));

            builder.Services.AddTransient<IIDentityUser, IdentityUserServices>();

            builder.Services.AddTransient<ICategory, CategoryServices>();
            builder.Services.AddTransient<ICommentSubject, CommentSubjectServices>();

            builder.Services.AddTransient<IComplaint, ComplaintServices>();
            builder.Services.AddScoped<JWTTokenServices>();
            builder.Services.AddTransient<ISubject, SubjectServices>();
            builder.Services.AddTransient<IGeneralSubject, GeneralSubjectTypeService>();
            builder.Services.AddTransient<IReplyComment, CommentRepliesServices>();
            builder.Services.AddScoped<IMain, MainTopicServices>();

            //builder.Services.AddTransient<ICountry, CountryService>();

            builder.Services.AddTransient<IRatingSubject, RatingSubjectServices>();

            builder.Services.AddTransient<INotification, NotificationServices>();

            builder.Services.AddTransient<IVisitor, VisitorServices>();
            builder.Services.AddTransient< IUploadFile, UploadFilesServices>();
            builder.Services.AddTransient<IUsers, UserServices>();
            builder.Services.AddTransient<IDepartmentType, DepartmentServices>();
            builder.Services.AddTransient<IAdmin, AdminServices>();
            builder.Services.AddTransient<IOnBoardScreen, OnBoardScreenServices>();
            builder.Services.AddTransient<ISearch, SearchService>();

            builder.Services.AddScoped<FileServices>();




            // Auth setup 
            builder.Services.AddIdentity<User, IdentityRole>
               (options =>
               {
                   options.User.RequireUniqueEmail = true;
               }).AddEntityFrameworkStores<AppDbContext>();

            // Add Services here 
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = JWTTokenServices.GetValidationParamerts(builder.Configuration);
            });


            // Permissions Setup
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Create", policy => policy.RequireClaim("Permissions", "Create"));
                options.AddPolicy("Read", policy => policy.RequireClaim("Permissions", "Read"));
                options.AddPolicy("Update", policy => policy.RequireClaim("Permissions", "Update"));
                options.AddPolicy("Delete", policy => policy.RequireClaim("Permissions", "Delete"));
            });

            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "dhofarAPI", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
            });

            //FirebaseApp.Create(new AppOptions
            //{
            //    Credential = GoogleCredential.FromFile("C:\\Users\\ammar\\source\\repos\\dhofarAPI\\dhofarAPI\\FireBaseKey.json")
            //});


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("ar")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ar"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });




            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
