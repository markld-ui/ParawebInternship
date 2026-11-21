# Paraweb Internship — LandingAPI

LandingAPI — серверная часть платформы для организации мероприятий, разработанная в рамках учебной стажировки в Paraweb в марте 2025 года. REST API позволяет публиковать мероприятия и новости, прикреплять к ним файлы и управлять учётными записями и ролями.

## Возможности

- Регистрация и вход, смена пароля, выдача JWT и обновление токена доступа через refresh token.
- Просмотр, поиск и управление мероприятиями и новостями. Списки поддерживают пагинацию и сортировку.
- Загрузка и скачивание вложений, в том числе изображений, привязка файлов к мероприятиям и новостям.
- Управление профилями пользователей, просмотр публикаций автора, назначение и снятие ролей. Изменение новостей и мероприятий и управление ролями доступны через административные маршруты с ролью `Admin`.

## Стек и структура

C#, .NET 9, ASP.NET Core Web API, Entity Framework Core 9, Microsoft SQL Server, AutoMapper, BCrypt и Swagger / OpenAPI.

Решение находится в [LandingAPI.sln](LandingAPI/LandingAPI.sln), веб-проект — в [LandingAPI.csproj](LandingAPI/LandingAPI/LandingAPI.csproj). Контроллеры и DTO описывают HTTP-интерфейс, репозитории работают с данными через `DataContext`, а сервисы отвечают за JWT, хеширование паролей и хранение файлов. Файлы сохраняются в `wwwroot/files`, их метаданные — в SQL Server.

## Локальный запуск

Понадобятся .NET 9 SDK и доступный экземпляр Microsoft SQL Server. Используйте отдельную базу для разработки. У пользователя базы должны быть права на создание таблиц, а при отсутствии базы — на её создание.

Из корня репозитория выполните команды в Bash или Zsh. Подставьте свои параметры SQL Server и локальный пароль.

```bash
cd LandingAPI/LandingAPI
mkdir -p wwwroot/files

export ConnectionStrings__DefaultConnection='Server=localhost,1433;Database=LandingApiDev;User Id=sa;Password=YOUR_LOCAL_PASSWORD;TrustServerCertificate=True'
export Jwt__Secret="$(openssl rand -hex 32)"
export Jwt__Issuer='LandingAPI'
export Jwt__Audience='LandingAPI'
export Jwt__ExpireMinutes='15'

dotnet run --launch-profile http
```

При старте приложение вызывает `EnsureCreated()` и пытается добавить демоданные. На пустой базе SQL Server заполнение может остановиться при вставке ролей, поскольку код задаёт `RoleId` явно, а столбец настроен как `IDENTITY`. Если API запустился, но демоданные отсутствуют, проверьте журнал запуска.

В профиле `http` интерфейс Swagger доступен по адресу [http://localhost:5209/swagger](http://localhost:5209/swagger). Он содержит спецификации `v1` и `v99`. Для защищённых операций получите токен через `POST /api/v1/auth/login` и передайте его в заголовке `Authorization: Bearer <access_token>`.

## Основные маршруты

| Маршрут | Назначение |
| --- | --- |
| `POST /api/v1/auth/{register,login,refresh,change-password}` | Учётные данные и токены. |
| `GET /api/v1/news`, `GET /api/v1/events` | Списки новостей и мероприятий. Для пагинации и сортировки используются `page`, `size`, `sort` и `asc`. |
| `POST /api/v1/files/upload` | Загрузка файла до 10 МБ через `multipart/form-data`. |
| `GET /api/v1/users/{id}` | Получение профиля пользователя. |
| `/api/v99/news`, `/api/v99/events` | Создание, редактирование и удаление публикаций и мероприятий с ролью `Admin`. |
| `/api/v99/roles` | Просмотр ролей пользователей и управление назначениями с ролью `Admin`. |

## Лицензия

[MIT](LICENSE).
