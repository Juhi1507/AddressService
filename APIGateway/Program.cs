
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //load ocelot configuration
            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
            //Add ocelot
            builder.Services.AddOcelot(builder.Configuration);


            builder.Services.AddControllers().AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAngularApp",
            //        policy =>
            //        {
            //            policy.WithOrigins("http://localhost:4200") // Allow Angular frontend
            //                  .AllowAnyMethod()
            //                  .AllowAnyHeader();
            //        });
            //});
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });




            var app = builder.Build();
            app.UseCors("AllowAll");

            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next();
            });


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();


            app.UseAuthorization();


            app.MapControllers();

            app.UseOcelot();

            app.Run();
        }
    }
}
