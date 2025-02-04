using GoogleVoice.Services;
using GoogleVoice.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Adicionar acesso �s vari�veis de ambiente
builder.Configuration.AddEnvironmentVariables();

// Adiciona servi�os ao container de depend�ncia
builder.Services.AddScoped<IAudioServices, AudioServices>();
builder.Services.AddScoped<ISpeechServices, SpeechServices>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
