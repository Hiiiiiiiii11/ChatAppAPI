
using ChatAppAPI.Jwt;
using CloudinaryDotNet;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Share.Services;
using System.Text;
using UserRepository.Admin;
using UserRepository.Data;
using UserRepository.Models;
using UserRepository.Repositories;
using UserRepository.VerifyEmail;
using UserService.Cloudinaries;
using UserService.Repositories;
using UserService.Services;

namespace ChatAppAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<UserDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("UserDbConnection")));
            //builder.Services.AddDbContext<ChatDbContext>(options =>
            //options.UseSqlServer(builder.Configuration.GetConnectionString("ChatDbConnection")));
            //builder.Services.AddDbContext<NotificationDbContext>(options =>
            //options.UseSqlServer(builder.Configuration.GetConnectionString("NotificationDbConnection")));


            builder.Services.Configure<CloudinarySettings>(
            builder.Configuration.GetSection("CloudinarySettings"));
            builder.Services.Configure<AdminAccountSettings>(
            builder.Configuration.GetSection("AdminAccountSettings"));
            builder.Services.Configure<EmailSettings>(
            builder.Configuration.GetSection("EmailSettings"));

            builder.Services.AddScoped<EmailSettings>(sp =>
                sp.GetRequiredService<IOptions<EmailSettings>>().Value);
            builder.Services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<AdminAccountSettings>>().Value);

            builder.Services.AddSingleton(provider =>
            {
            var config = builder.Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
            var account = new Account(config.CloudName, config.ApiKey, config.ApiSecret);
            return new Cloudinary(account);
            });

            // Add services to the container.
            //builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            //builder.Services.AddScoped<INotificationService, NotificationService.Services.NotificationService>();
            builder.Services.AddScoped<IUserRepository, UserRepository.Repositories.UserRepository>();
            builder.Services.AddScoped<IUserService,UserService.Services.UserService>();
            builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IUploadPhotoService, UploadPhotoService>();
            builder.Services.AddScoped<IEmailVerificationRepository, EmailVerificationRepository>();
            builder.Services.AddScoped<IEmailVerificationService, EmailVerificationService>();
            builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
            builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();
            //builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            //builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
            //builder.Services.AddScoped<IConversationService, ConversationService>();
            //builder.Services.AddScoped<IMessageService, MessageService>();
            //builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
            //builder.Services.AddScoped<IParticipantService, ParticipantService>();
            //builder.Services.AddScoped<UserGrpcClientService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Thêm gRPC
            builder.Services.AddGrpc();
            builder.WebHost.ConfigureKestrel(options =>
            {
                // Port cho REST API (Swagger, Controllers) → HTTP/1.1
                options.ListenAnyIP(5000, o => o.Protocols = HttpProtocols.Http1);

                // Port cho gRPC → HTTP/2
                options.ListenAnyIP(5001, o => o.Protocols = HttpProtocols.Http2);
                // HTTPS (dùng dev certificate của ASP.NET)
                options.ListenAnyIP(7216, o =>
                {
                    o.Protocols = HttpProtocols.Http1AndHttp2;
                    o.UseHttps();
                });
            });


            // Configure Swagger to generate API documentation
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "ChatApp API",
                    Version = "v1",
                    Description = "API for Chat Application"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http, 
                    Scheme = "bearer",               
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            // CORS policy to allow all origins, methods, and headers
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policyBuilder =>
                {
                    policyBuilder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader();
                });
            });


            //dang ký Jwt
            builder.Services.Configure<JwtSettings>(
            builder.Configuration.GetSection("Jwt")
            );

            var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
            var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            // Ngăn ASP.NET Core tự gửi 401 mặc định
                            context.HandleResponse();

                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            await context.Response.WriteAsync(
                                "{\"message\":\"Unauthorized - Token is missing or invalid.\"}");
                        },
                        OnForbidden = async context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";

                            await context.Response.WriteAsync(
                                "{\"message\":\"Forbidden - You do not have permission to access this resource.\"}");
                        },
                    };

                });

            //dang ký Cloudinary
            builder.Services.Configure<CloudinarySettings>(
            builder.Configuration.GetSection("CloudinarySettings")
            );

            builder.Services.AddSingleton(provider =>
            {
                var config = provider.GetRequiredService<IOptions<CloudinarySettings>>().Value;
                return new CloudinaryDotNet.Cloudinary(new Account(
                    config.CloudName,
                    config.ApiKey,
                    config.ApiSecret
                ));
            });


            var app = builder.Build();

            app.MapGrpcService<UserGrpcServiceImpl>();
            app.MapGet("/", () => "User gRPC Service is running");

            //seeding admin accouunt
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
                var adminSettings = scope.ServiceProvider
                                         .GetRequiredService<IOptions<AdminAccountSettings>>()
                                         .Value;

                // Kiểm tra nếu chưa có admin
                if (!context.Users.Any(u => u.Email == adminSettings.Email))
                {
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(adminSettings.Password);
                    var adminUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = adminSettings.Email,
                        PasswordHash = hashedPassword,
                        DisplayName = adminSettings.DisplayName,
                        IsActive = true
                    };

                    context.Users.Add(adminUser);
                    context.SaveChanges();
                }
            }

            //




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
        }
    }
}
