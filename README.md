# 🧹 API Evaluación — Proyecto de Prueba Técnica

Este proyecto es una API RESTful desarrollada en **.NET 8** como parte de una prueba técnica.

La API permite:
- ✅ Registro de usuarios con validaciones avanzadas
- ✅ Inicio de sesión (Login) con JWT
- ✅ Consumo de API externa (JSONPlaceholder) asegurado con token JWT
- ✅ Pruebas unitarias con xUnit y Moq
- ✅ Documentación con Swagger



---

## 🚀 Tecnologías utilizadas

- **.NET 8**
- **Entity Framework Core InMemory**
- **JWT Bearer Authentication**
- **FluentValidation**
- **Swagger (OpenAPI)**
- **xUnit** y **Moq** para pruebas unitarias
- **HttpClient** para consumo de APIs externas

---

## ⚙️ Instalación y ejecución

### ✅ Opcion 1: Visual Studio (Recomendado)

1. Clona el repositorio.
2. Abre la solución `APIEvaluacion.sln`.
3. Visual Studio restaurará las dependencias automáticamente (NuGet).
4. Selecciona el proyecto de inicio `APIEvaluacion`.
5. Presiona **F5** o haz clic en **Iniciar**.

La API correrá en:
```
https://localhost:7060
http://localhost:5065
```

### ✅ Opcion 2: CLI / Terminal

Si prefieres usar la terminal, sigue estos pasos:

```bash
# Restaura las dependencias
dotnet restore

# Compila la solución
dotnet build

# Ejecuta la aplicación
dotnet run --project APIEvaluacion/APIEvaluacion.csproj
```

---

## 📖 Documentación de la API

Una vez que la API esté corriendo, accede a **Swagger** en:

```
https://localhost:7060/swagger
http://localhost:5065/swagger
```

Aquí podrás probar todos los endpoints de forma interactiva.

---

## 🧪 Pruebas unitarias

El proyecto incluye pruebas unitarias para los principales servicios:

Para ejecutarlas desde Visual Studio:
- Abre el **Explorador de pruebas**.
- Haz clic en **Ejecutar todas las pruebas**.

O desde la terminal:

```bash
dotnet test
```

---

## 📡 Uso de la API (Postman o Frontend)

1. **Registro de usuario**
   - Endpoint: `POST /api/Users/register`
   - Payload:
```json
{
  "name": "Daniel De Leon",
  "email": "daniel@example.com",
  "password": "Password123!"
}
```
- Obtendrás un **token JWT**.

2. **Login de usuario**
   - Endpoint: `POST /api/Users/login`
   - Obtendrás un **token JWT**.

3. **Consumo de API externa (requiere token)**
   - Endpoint GET: `GET /api/ExternalApi/get`
   - Endpoint POST: `POST /api/ExternalApi/posts`
   - En **Postman**, ve a la pestaña Headers y añade:
```
Key: Authorization
Value: Bearer <token_obtenido_del_login>
```

---

## 🔒 Seguridad

- Autenticación con JWT obligatoria para endpoints externos.
- Validación de entrada con FluentValidation.
- Base de datos en memoria (sin riesgo de SQL Injection).
- Uso de HTTPS en entornos de desarrollo.

---

## 📁 Estructura del proyecto

```
📆 APIEvaluacion
🔺️ Controllers
🔺️ Data
🔺️ DTOs
🔺️ Helpers
🔺️ Models
🔺️ Services
🔺️ Validators
🔺️ Program.cs

📆 TestAPIEvaluacion
🔺️ UserServiceTests.cs
🔺️ UserServiceAuthTests.cs
```

---

## 🙌 Contribuciones

Proyecto para fines educativos y de evaluación.  


---

## 🧑‍💻 Autor

Daniel De León

