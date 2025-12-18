using CourierManagementSystem.Api;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
Startup startup = new(builder);

WebApplication app = startup.Initialize();
app.Run();


// Make Program class accessible to tests
public partial class Program 
{ 
}