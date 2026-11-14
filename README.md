# Caso 1

**Isaac Sanchez Delgado**

Aplicación web en .NET 8 para consultar sismos de un país. La solución incluye una API, acceso a datos, modelos y una interfaz MVC. Los datos sísmicos provienen de USGS y la información del país de REST Countries.

## Requisitos

- Visual Studio con soporte para ASP.NET Core o el SDK de .NET 8.
- SQL Server Express con la instancia `localhost\SQLEXPRESS`.
- Una API key válida de REST Countries y conexión a Internet.

## Configuración

1. Abre `AndreySanchez.Caso1.API.slnx` en Visual Studio.
2. En `AndreySanchez.Caso1.API`, copia `appsettings.example.json` como `appsettings.json` si todavía no existe. En tu copia local, reemplaza `PEGA_AQUI_TU_API_KEY` por tu API key de REST Countries.
3. Revisa `ConnectionStrings:DefaultConnection` en ese archivo. La configuración incluida usa `localhost\SQLEXPRESS` y autenticación de Windows; cámbiala si tu instancia de SQL Server tiene otro nombre.
4. En Visual Studio, configura **varios proyectos de inicio**: `AndreySanchez.Caso1.API` y `Caso1.Web`, ambos con la acción **Iniciar** y el perfil **https**. Inicia la API antes de hacer una búsqueda en la web.
5. Ejecuta la solución. La API crea la base `Caso1EarthquakesDb` si no existe. En el primer inicio importa los registros de USGS desde 2020 hasta la fecha final definida en `Usgs:EndDate`; esto puede tardar varios minutos. Espera a que la API indique que escucha en `https://localhost:7220`.

## Uso

1. Abre la aplicación en `https://localhost:7230`.
2. Escribe un país, por ejemplo **Costa Rica**, y pulsa **Explorar**.
3. Consulta la información del país y los sismos. Usa los controles al final de la lista para cambiar de página (12 resultados por página).
4. Usa **Modo oscuro / Modo claro** para cambiar el fondo de los resultados. La elección se guarda en ese navegador.

La API se puede probar en `https://localhost:7220/swagger`. El endpoint de búsqueda es `GET /api/earthquakes/search?country=Costa%20Rica`. La API devuelve hasta 500 sismos por búsqueda; la paginación de la interfaz organiza esos resultados.

## Si aparece un error

- **No se pudo conectar con la API:** comprueba que el proyecto API está iniciado y escucha en el puerto 7220.
- **Puerto 7220 o 7230 en uso:** cierra otra ejecución de la API o la web antes de iniciar la solución de nuevo.
- **Error de SQL Server:** verifica que `SQLEXPRESS` está iniciado y que la cadena de conexión apunta a tu instancia.
- **No se encuentra el país:** revisa la API key y la conexión a REST Countries.

El script alternativo de creación de tablas está en `Database/Database.sql`.

## GitHub y la API key

El archivo `AndreySanchez.Caso1.API/appsettings.json` contiene tu configuración local y está excluido por `.gitignore`. Para compartir el proyecto, usa `appsettings.example.json` como plantilla. **No subas la API key real a GitHub**, incluso si el repositorio es privado.
