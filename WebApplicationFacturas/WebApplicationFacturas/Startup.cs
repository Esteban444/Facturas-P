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
                Configuration.CreateMap<Empresas, EmpresasBase>().ReverseMap();
                Configuration.CreateMap<Empresas, EmpresasRequestDTO>();
                Configuration.CreateMap<EmpresasUpdateRequestDTO, Empresas>();

                Configuration.CreateMap<Empleados, EmpleadosBase>().ReverseMap();
                Configuration.CreateMap<EmpleadosUpdateRequestDTO, Empleados>();
                Configuration.CreateMap<Empleados, EmpleadosRequestDTO>();

                Configuration.CreateMap<Cargos, CargosBase>().ReverseMap();
                Configuration.CreateMap<Cargos, CargosRequestDTO>();
                Configuration.CreateMap<CargosUpdateRequestDTO, Cargos>();

                Configuration.CreateMap<Categorias, CategoriasBase>().ReverseMap();
                Configuration.CreateMap<Categorias, CategoriasRequestDTO>();
                Configuration.CreateMap<CategoriasUpdateRequestDTO, Categorias>();

                Configuration.CreateMap<Productos, ProductosBase>().ReverseMap();
                Configuration.CreateMap<ProductosUpdateRequestDTO, Productos>();
                Configuration.CreateMap<Productos, ProductosRequestDTO>();

                Configuration.CreateMap<FacturasProductos, FacturasProductosBase>().ReverseMap();
                Configuration.CreateMap<FacturasProductos, FacturasProductosRequestDTO>();
                Configuration.CreateMap<FacturasProductosUpdateRequestDTO, FacturasProductos>();

                Configuration.CreateMap<Facturas, FacturasBase>().ReverseMap();
                Configuration.CreateMap<Facturas, FacturasRequestsDTO>();
                Configuration.CreateMap<FacturasUpdateRequestDTO, Facturas>();

                Configuration.CreateMap<Clientes, ClientesBase>().ReverseMap();
                Configuration.CreateMap<ClientesUpdateRequestDTO, Clientes>();
                Configuration.CreateMap<Clientes, ClientesRequestDTO>();

                Configuration.CreateMap<TipoClientes, TipoClientesBase>().ReverseMap();
                Configuration.CreateMap<TipoClientesUpdateRequestDTO, TipoClientes>();
                Configuration.CreateMap<TipoClientes, TipoClientesRequestDTO>();

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
