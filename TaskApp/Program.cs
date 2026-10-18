using TaskService;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITaskService,TaskControllerService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
