# AttendanceMonitoring — Система учёта посещаемости

Backend-приложение на C# / ASP.NET Core 8 для учёта посещаемости студентов.

## Технологии

- C# / .NET 8 / ASP.NET Core Web API
- Entity Framework Core + SQLite
- JWT-аутентификация
- Serilog (логирование)
- MemoryCache (кэширование)
- Swagger / OpenAPI

## Структура проекта
AttendanceMonitoring/

├── AttendanceMonitoring.Api/          # Контроллеры, JWT, настройки

├── AttendanceMonitoring.Application/  # Сервисы, DTO, интерфейсы

├── AttendanceMonitoring.Domain/       # Сущности предметной области

└── AttendanceMonitoring.Infrastructure/ # EF Core, репозитории, генератор

## Запуск проекта

### 1. Восстановить пакеты
```bash
dotnet restore
```

### 2. Собрать проект
```bash
dotnet build
```

### 3. Применить миграции (создать БД)
```bash
dotnet ef database update --project AttendanceMonitoring.Infrastructure --startup-project AttendanceMonitoring.Api
```

### 4. Запустить приложение
```bash
dotnet run --project AttendanceMonitoring.Api
```

### 5. Открыть Swagger
http://localhost:5221/swagger

## Тестовые пользователи

| Логин | Пароль | Роль |
|-------|--------|------|
| admin | admin123 | Admin |
| analyst | analyst123 | Analyst |
| operator | operator123 | Operator |

## API эндпоинты

| Метод | URL | Доступ | Назначение |
|-------|-----|--------|-----------|
| POST | /api/auth/login | Все | Получить JWT-токен |
| GET | /api/attendance | Все | Список записей посещаемости |
| POST | /api/attendance | Все | Создать запись |
| DELETE | /api/attendance/{id} | Admin | Удалить запись |
| GET | /api/analytics | Все | Аналитика по дням и группам |
| GET | /api/analytics/export | Все | Скачать CSV-отчёт |

## Типы записей посещаемости

| Тип | Severity | Описание |
|-----|----------|---------|
| Посещение | Info | Студент присутствовал |
| Пропуск | Critical | Студент отсутствовал |
| Опоздание | Warning | Студент опоздал |
| Отметка входа | Info | Фиксация прохода |

## Предметная область

Система фиксирует посещаемость студентов учебных групп.
При старте автоматически генерируется 50 тестовых записей за последние 30 дней.
Логи пишутся в папку `logs/`. Аналитика кэшируется на 10 минут.

## Производственная практика

ПМ.01 Разработка модулей программного обеспечения для компьютерных систем