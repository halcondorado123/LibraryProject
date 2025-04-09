# DESARROLLADOR: JHONNATTAN HALCON CASALLAS FELIPE


# Aplicación de Gestión de Usuarios y Libros

## Descripción

Esta es una aplicación web construida con **ASP.NET Core MVC** y utilizando un enfoque de **arquitectura hexagonal**. La aplicación permite la gestión de usuarios, roles de usuarios, y administración de libros, calificaciones y comentarios de usuarios. Además, se ha implementado una lógica de búsqueda avanzada con filtros inteligentes para mejorar la experiencia del usuario.

## Características

- **Autenticación y Autorización**: Usando **ASP.NET Core Identity** para gestionar roles de usuarios (Administrador, Usuario).
- **Gestión de Usuarios**: Crear, actualizar y eliminar usuarios.
- **Gestión de Roles**: Asignar roles a los usuarios.
- **Libros**: Agregar, editar y eliminar libros.
- **Calificaciones de Usuarios**: Los usuarios pueden calificar libros y ver las calificaciones.
- **Búsqueda Avanzada**: Filtros inteligentes y búsqueda de libros.
- **Interfaz de Usuario**: Desarrollada con **HTML**, **CSS**, **Bootstrap**, **JavaScript**, **AJAX** y **jQuery**.
- **Mapeo de Datos**: Uso de **AutoMapper** para mapear entre entidades y DTOs.
- **Paginación y Vistas de Filtros**: Vistas Razor para la visualización y filtrado de libros, con paginación.

## Estructura de la Arquitectura

La aplicación sigue una **arquitectura hexagonal**, separando las responsabilidades en diferentes capas para facilitar el mantenimiento y escalabilidad:

### Capas

1. **Application**: 
   - Contiene la lógica de negocio y las interfaces de servicio (IServicio).
   - Incluye los DTOs que se utilizan para comunicar datos entre capas.

2. **Domain**:
   - Contiene los modelos de dominio, que representan las entidades fundamentales de la aplicación (Usuario, Libro, Calificación, etc.).

3. **Infrastructure**:
   - Contiene la implementación de la base de datos y la configuración de **Entity Framework**.
   - Configuración de **Identity** para la gestión de usuarios y roles.
   - Implementación de repositorios y acceso a datos.

4. **WebAPI**:
   - Contiene los controladores y vistas **Razor** para la interacción con los usuarios.
   - Configuración de las rutas y vistas de la aplicación.

### Tecnologías Utilizadas

- **ASP.NET Core MVC**: Para la creación de la aplicación web.
- **Entity Framework Core**: Para la gestión de la base de datos SQL Server.
- **ASP.NET Core Identity**: Para la autenticación y autorización de usuarios.
- **AutoMapper**: Para mapear datos entre modelos de dominio y DTOs.
- **Bootstrap**: Para el diseño y maquetación de la interfaz.
- **AJAX, JavaScript y jQuery**: Para la gestión dinámica de las vistas y peticiones asincrónicas.

## Instalación

1. Clona el repositorio:

   ```bash
   git clone <repositorio>
   ```

2. Abre el proyecto en **Visual Studio Code**.

3. Restaura los paquetes NuGet:

   ```bash
   dotnet restore
   ```

4. Agrega las dependencias necesarias para la aplicación:

   ```bash
   dotnet add package AutoMapper --version 14.0.0
   dotnet add package Microsoft.EntityFrameworkCore --version 9.0.3
   dotnet add package Microsoft.EntityFrameworkCore.Relational --version 9.0.3
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.3
   dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.3
   dotnet add package Microsoft.AspNetCore.Authorization --version 8.0.14
   dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.14
   dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.14
   dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore --version 8.0.14
   dotnet add package Swashbuckle.AspNetCore --version 8.1.0
   dotnet add package Swashbuckle.AspNetCore.Annotations --version 8.1.0
   dotnet add package FluentValidation --version 11.11.0
   dotnet add package Microsoft.Extensions.Logging --version 9.0.3
   ```

5. Configura la cadena de conexión a la base de datos en el archivo `appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=NombreDeTuBaseDeDatos;Trusted_Connection=True;MultipleActiveResultSets=true;"
     }
   }
   ```

   Reemplaza `localhost` con el nombre de tu servidor SQL Server y `NombreDeTuBaseDeDatos` con el nombre de tu base de datos.

6. Crea el contexto de datos en la clase `ApplicationDbContext` y configura el acceso a la base de datos.

7. Registra el contexto en `Program.cs`:

   ```csharp
   public static IHostBuilder CreateHostBuilder(string[] args) =>
       Host.CreateDefaultBuilder(args)
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.ConfigureServices(services =>
               {
                   services.AddDbContext<ApplicationDbContext>(options =>
                       options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
               });
               webBuilder.UseStartup<Startup>();
           });
   ```

8. Ejecuta las migraciones para crear la base de datos:

   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

9. Ejecuta la aplicación:

   ```bash
   dotnet run
   ```

## Contribución

Si deseas contribuir a la aplicación, puedes hacer un **fork** del repositorio, realizar los cambios necesarios y enviar un **pull request**.

## Licencia

Este proyecto está bajo la **Licencia MIT**.
