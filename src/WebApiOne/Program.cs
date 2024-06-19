using Microsoft.OpenApi.Models;
using WebApiOne;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<InMemoryProjectRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

const string swaggerDocName = "web-api-one";
const string title = "Web Api One";
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        swaggerDocName,
        new OpenApiInfo
        {
            Version = "v1",
            Title = title,
            Description = "Sample Web Api One which publishes an OpenApi document"
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint($"/swagger/{swaggerDocName}/swagger.json", title));
}

app.UseHttpsRedirection();

app.MapPost("/projects", (Project project, InMemoryProjectRepository repository) =>
    {
        project.Id ??= Guid.NewGuid().ToString();
        repository.Add(project);
        return Results.Created($"/projects/{project.Id}", project);
    })
    .WithName("CreateProject")
    .WithOpenApi();

app.MapGet("/projects", (InMemoryProjectRepository repository) => repository.GetAll())
    .WithName("GetProjects")
    .WithOpenApi();

app.MapGet("/projects/{id}", (string id, InMemoryProjectRepository repository) =>
    {
        var project = repository.GetById(id);
        return project == null ? Results.NotFound() : Results.Ok(project);
    })
    .WithName("GetProjectById")
    .WithOpenApi();

app.Run();