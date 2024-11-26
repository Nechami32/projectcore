


// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.OpenApi.Models;
// using OurApi;
// using OurApi.Interfaces;
// using OurApi.Services;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// builder.Services
//         .AddAuthentication(options =>
//         {
//             options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//         })
//         .AddJwtBearer(cfg =>
//         {
//             cfg.RequireHttpsMetadata = false;
//             cfg.TokenValidationParameters = TasksTokenService.GetTokenValidationParameters();
//         });

// builder.Services.AddAuthorization(cfg =>
// {
//     cfg.AddPolicy("Admin", policy => policy.RequireClaim("type", "Admin", "User"));
//     cfg.AddPolicy("User", policy => policy.RequireClaim("type", "User"));
// });

// // Swagger configuration
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tasks", Version = "v1" });
//     c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         In = ParameterLocation.Header,
//         Description = "Please enter JWT with Bearer into field",
//         Name = "Authorization",
//         Type = SecuritySchemeType.ApiKey
//     });
//     c.AddSecurityRequirement(new OpenApiSecurityRequirement {
//         { new OpenApiSecurityScheme
//             {
//                 Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
//             },
//             new string[] {}
//         }
//     });
// });

// builder.Services.AddControllers();
// builder.Services.AddSingleton<Iglass, GlassesService>();
// builder.Services.AddSingleton<Iuser, userService>();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseDefaultFiles();
// app.UseStaticFiles();
// app.UseHttpsRedirection();

// app.UseAuthentication(); // מפעיל אימות באמצעות הטוקן
// app.UseAuthorization();  // מפעיל בדיקות הרשאות

// // הוספת TokenExpirationMiddleware
// app.UseTokenExpMiddleware(); // הוספת הבדיקה אם הטוקן פג תוקף

// app.MapControllers();

// app.Run();

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using OurApi;
using OurApi.Interfaces;
using OurApi.Services;

var builder = WebApplication.CreateBuilder(args);

// הוספת שירותים למיכל
builder.Services
        .AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.TokenValidationParameters = TasksTokenService.GetTokenValidationParameters();
        });

builder.Services.AddAuthorization(cfg =>
{
    cfg.AddPolicy("Admin", policy => policy.RequireClaim("type", "Admin"));
    cfg.AddPolicy("User", policy => policy.RequireClaim("type", "User"));
                        cfg.AddPolicy("ClearanceLevel1", policy => policy.RequireClaim("ClearanceLevel", "1", "2"));
                    cfg.AddPolicy("ClearanceLevel2", policy => policy.RequireClaim("ClearanceLevel", "2"));
});




// הגדרת Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tasks API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "הכנס 'Bearer [מרווח] your_token' בשדה למטה.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

builder.Services.AddControllers();
builder.Services.AddSingleton<Iglass, GlassesService>();
builder.Services.AddSingleton<Iuser, userService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// קונפיגורציה לפייפליין של בקשות HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
 app.UseRouting();
 app.UseAuthentication();
app.UseAuthorization(); 
 // הפעלת אימות טוקן
 // הפעלת הרשאות

//app.TokenExtensions(); // הוספת המידלוואר לפייפליין
// מותאם אישית לבדיקת פג תוקף של הטוקן

app.MapControllers();

app.Run();

