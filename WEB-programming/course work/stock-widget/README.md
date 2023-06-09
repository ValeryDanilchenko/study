# Виджет "Котировки акций на биржах" с использованием Vue.js и Webpack

![Widget](/WEB-programming/course%20work/stock-widget/public/images-cloud/report/widget.png)
## Техническое задание

### 1. Общие сведения

#### 1.1. Наименование программного продукта

Виджет "Котировки акций на биржах"

#### 1.2. Назначение и область применения

Виджет предназначен для отображения котировок акций на биржах на веб-страницах. Область применения - финансовые сайты, блоги, новостные порталы.

### 2. Требования к программному продукту

#### 2.1. Функциональные требования

- Отображение текущих котировок акций на биржах
- Возможность выбора акций для отображения
- Возможность настройки внешнего вида виджета (цвета, размеры, шрифты)
- Автоматическое обновление данных с заданным интервалом

#### 2.2. Требования к надежности

- Обеспечение корректной работы виджета при изменении размеров окна браузера
- Обработка ошибок при получении данных от сервера

#### 2.3. Требования к совместимости

- Работа с браузерами: Google Chrome, Mozilla Firefox, Safari, Microsoft Edge

### 3. Этапы работ

1. Разработка технического задания
2. Проектирование архитектуры приложения
3. Разработка компонентов виджета на Vue.js
4. Настройка сборки проекта с использованием Webpack
5. Тестирование и отладка
6. Развертывание и поддержка

### 4. Сдача и приемка работ

#### 4.1. Сдача работ осуществляется в виде исходного кода проекта на GitHub

#### 4.2. Приемка работ осуществляется заказчиком путем проверки корректности работы виджета на тестовой странице

### 5. Технические требования

#### 5.1. Используемые технологии и инструменты

- Язык программирования: JavaScript (ES6+)
- Фреймворк: Vue.js
- Сборщик проекта: Webpack
- Библиотека для HTTP-запросов: Axios

#### 5.2. Структура проекта

- src/
  - components/
    - StockWidget.vue
    - StockList.vue
    - StockItem.vue
  - services/
    - StockDataService.js
  - styles/
    - main.scss
  - App.vue
  - main.js
- public/
  - index.html
- package.json
- webpack.config.js

### 6. Описание компонентов

#### 6.1. StockWidget.vue

- Основной компонент виджета, отвечает за отображение и настройку виджета
- Содержит компоненты StockList и настройки внешнего вида

#### 6.2. StockList.vue

- Компонент, отображающий список акций
- Содержит компоненты StockItem и обрабатывает получение данных от сервера

#### 6.3. StockItem.vue

- Компонент, отображающий информацию об одной акции (название, котировка, изменение)

### 7. Описание сервисов

#### 7.1. StockDataService.js

- Сервис для получения данных о котировках акций с сервера
- Использует библиотеку Axios для отправки HTTP-запросов

### 8. Тестирование

### План тестирования

1. **Тестирование отображения компонентов**
   - Проверить корректное отображение компонентов StockWidget, StockList, StockItem и SearchBar на странице
   - Проверить стилизацию компонентов согласно заданию

2. **Тестирование функциональности слайдера**
   - Проверить корректное отображение слайдера с пагинацией
   - Проверить переключение между слайдами при нажатии на пагинацию
   - Проверить корректное отображение акций на каждом слайде

3. **Тестирование фильтрации акций по поисковому запросу**
   - Ввести поисковый запрос в поле SearchBar и проверить, что отображаются только акции, соответствующие запросу
   - Проверить корректность работы фильтрации при изменении поискового запроса
   - Проверить отображение всех акций при удалении поискового запроса

4. **Тестирование корректного отображения данных акций**
   - Проверить корректное отображение названия, символа и цены акции в компоненте StockItem
   - Проверить корректное обновление данных акций при изменении их значений (можно использовать мок-данные для тестирования)

5. **Тестирование совместимости с различными браузерами**
   - Проверить корректное отображение и работу виджета в различных браузерах: Google Chrome, Mozilla Firefox, Safari, Microsoft Edge



