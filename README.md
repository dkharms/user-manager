# UserManager
## Тестовое задание

Я реализовал все требования из ТЗ, все работает корректно.
Для реализации приложения я использовал паттерн MVC, так как это довольно понятный и проверенный временем паттерн, который дает нужный уровень абстракции.

Вероятно, в качестве улучшений можно было бы изменить логику взамодействия с БД. Так как я сразу инстанцирую все записи пользователей,
полученные из БД, приложение на большом количестве пользователей может тормозить, сильно забивать кучу, но я, к сожалению,
не знаю, как сделать некий аналог lazy-loader для БД. 

Во всем остальном мне кажется, что мой код довольно неплох.

## Взаимодействие

При открытии приложения впервые БД автоматически создается, дальше все взамодействие происходит через контекстное меню, которое вызывается
по нажатию на правую кнопку мыши.
