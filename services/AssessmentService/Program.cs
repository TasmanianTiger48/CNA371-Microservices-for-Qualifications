using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

//Services
builder.Services.AddScoped<IAssessmentService, AssessmentService.Infrastructure.Services.AssessmentService>();

//Repositories
builder.Services.AddScoped<IAssessmentRepository, AssessmentRepository>();

//Clients
builder.Services.AddScoped<IQualificationServiceClient, QualificationServiceClient>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
