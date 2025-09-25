[Описание на русском](#rus)
|
[Description in english](#eng)

---

<a name="rus"></a>
# Описание на русском

## Задача
Написать простой web‑сервис обмена сообщениями.

Сервис состоит из трёх компонентов:
- Web‑сервер
- SQL БД (желательно PostgreSQL)
- 3 клиента (первый пишет сообщения, второй отображает их в реальном времени, третий позволяет просмотреть список сообщений за последнюю минуту).

Все три клиентские части можно реализовать как отдельными web‑приложениями, так и одним с разделением клиентов по URL.

## Формат сообщения
- Текст до 128 символов
- Метка даты/времени (устанавливается на сервере)
- Порядковый номер (приходит от клиента)

## Схема работы
- Первый клиент пишет поток сообщений (по одному вызову API на сообщение).
- Сервис сохраняет сообщение в БД и отправляет его второму клиенту по WebSocket.
- Второй клиент отображает входящие сообщения в порядке прихода (с меткой времени и номером).
- Третий клиент отображает историю сообщений за последние 10 минут.

## API (REST или GraphQL)
Методы:
1. Отправить одно сообщение
2. Получить список сообщений за диапазон дат

*Желательно: Swagger‑документация (для REST‑API).* 

## Архитектурные требования
- Языки: C# или Go
- Архитектура: MVC или подобная
- DAL: без ORM
- Логирование: достаточно детализированное, чтобы понимать состояние системы

## Развёртывание
- Приложение оформить в виде docker‑образов
- Выложить на GitHub
- Подготовить `docker-compose.yml` для запуска всех компонентов
- Дизайн не важен: главное, чтобы сообщения можно было читать

---

<a name="eng"></a>
# Description in english

## Task
Develop a simple messaging web service.

The service consists of three components:
- Web server
- SQL database (preferably PostgreSQL)
- 3 clients (first sends messages, second displays them in real time, third shows a list of messages for the last minute).

All three client parts can be implemented either as separate web applications or as a single one with URL‑based separation.

## Message format
- Text up to 128 characters
- Date/time stamp (set by the server)
- Sequence number (provided by the client)

## Workflow
- The first client sends messages (one API call per message).
- The service saves the message in the database and pushes it to the second client via WebSocket.
- The second client displays incoming messages in arrival order (with timestamp and number).
- The third client displays the message history for the last 10 minutes.

## API (REST or GraphQL)
Methods:
1. Send a single message
2. Retrieve messages for a date/time range

*Optional: generate Swagger documentation (for REST‑API).* 

## Architectural requirements
- Languages: C# or Go
- Architecture: MVC or similar
- DAL: without ORM
- Logging: detailed enough to understand system state

## Deployment
- Package the application as docker images
- Publish on GitHub
- Prepare `docker-compose.yml` to launch all components
- Design is not important: the main goal is that messages can be displayed and read