using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationFacturas.Context;
using WebApplicationFacturas.Models;
using WebApplicationFacturas.DTO;
using WebApplicationFacturas.DTO.Requests;

namespace WebApplicationFacturas
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
            services.AddAutoMapper(Configuration =>
            {
                Configuration.CreateMap<Empresas, EmpresasDTO>().ReverseMap();
                Configuration.CreateMap<EmpresasCreacionDTO, Empresas>();
                Configuration.CreateMap<Empresas, RequestedEmpresasDTO>();

                Configuration.CreateMap<Empleados, EmpleadosBase>().ReverseMap();
                Configuration.CreateMap<EmpleadosUpdateRequestDTO, Empleados>();
                Configuration.CreateMap<Empleados, EmpleadosRequestDTO>();

                Configuration.CreateMap<Cargos, CargosDTO>().ReverseMap();
                Configuration.CreateMap<CreacioncargosDTO, Cargos>();
                Configuration.CreateMap<Cargos, RequestedCargosDTO>();

                Configuration.CreateMap<Categorias, CategoriasDTO>().ReverseMap();
                Configuration.CreateMap<Categorias, RequestedCategoriasDTO>();
                Configuration.CreateMap<CreacionCategoriasDTO, Categorias>();

                Configuration.CreateMap<Productos, ProductosDTO>().ReverseMap();
                Configuration.CreateMap<CreacionProductosDTO, Productos>();
                Configuration.CreateMap<Productos, RequestedProductosDTO>();

                Configuration.CreateMap<FacturasProductos, FacturasProductosBase>().ReverseMap();
                Configuration.CreateMap<FacturasProductos, FacturasProductosRequestDTO>();
                Configuration.CreateMap<FacturasProductosUpdateRequestDTO, FacturasProductos>();

                Configuration.CreateMap<Facturas, FacturasBase>().ReverseMap();
                Configuration.CreateMap<Facturas, FacturasRequestsDTO>();
                Configuration.CreateMap<FacturasUpdateRequestDTO, Facturas>();

                Configuration.CreateMap<Clientes, ClientesDTO>().ReverseMap();
                Configuration.CreateMap<CreacionClientesDTO, Clientes>();
                Configuration.CreateMap<Clientes, RequestedClientesDTO>();

                Configuration.CreateMap<TipoClientes, TipoClientesDTO>().ReverseMap();
                Configuration.CreateMap<CreaciontipoclientesDTO, TipoClientes>();
                Configuration.CreateMap<TipoClientes, RequestedTipoClientesDTO>();

            },typeof(Startup));

            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplicationFacturas", Version = "v1" });
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger(); // para yutilizar el Swagger

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            // para el funcionamiento de Swagger
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplicationFacturas V1");
            });
            app.UseCors(builder =>{
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
