using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

//Services
builder.Services.AddScoped<IEligibilityAssessmentService, AssessmentService.Infrastructure.Services.EligibilityAssessmentService>();

//Repositories
builder.Services.AddScoped<IEligibilityAssessmentRepository, EligibilityAssessmentRepository>();

//Clients
builder.Services.AddScoped<IQualificationServiceClient, QualificationServiceClient>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddHttpClient<IQualificationServiceClient, QualificationServiceClient>(client =>
{
    var baseUrl = builder.Configuration["Services:QualificationService"];

    client.BaseAddress = new Uri(
        baseUrl ?? throw new InvalidOperationException(
            "Qualification Service URL is not configured."));
});

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
