using DoctorAppointment.Infrastructure.Application;
using DoctorAppointment.Persistence.EF;
using DoctorAppointment.Persistence.EF.Appointments;
using DoctorAppointment.Persistence.EF.Doctors;
using DoctorAppointment.Persistence.EF.Patients;
using DoctorAppointment.Services.Appointments;
using DoctorAppointment.Services.Appointments.Contracts;
using DoctorAppointment.Services.Doctors;
using DoctorAppointment.Services.Doctors.Contracts;
using DoctorAppointment.Services.Patients;
using DoctorAppointment.Services.Patients.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DoctorAppointment.RestAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<ApplicationDbContext>
                (_ => _.UseSqlServer(Configuration["ConnectionString"]));

            services.AddScoped<UnitOfWork, EFUnitOfWork>();

            services.AddScoped<DoctorRepository, EFDoctorRepository>();
            services.AddScoped<DoctorService, DoctorAppService>();

            services.AddScoped<PatientService, PatientAppService>();
            services.AddScoped<PatientRepository, EFPatientRepository>();

            services.AddScoped<AppointmentService, AppointmentAppService>();
            services.AddScoped<AppointmentRepository, EFAppointmentRepository>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DoctorAppointment.RestAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DoctorAppointment.RestAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
