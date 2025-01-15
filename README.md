# DragNDrop2D
<ins>Техническое задание для Playnera</ins>  
*APK Файл присутствует в Release*

### Видео с игровой механикой:
https://disk.yandex.ru/i/BICOdzuEUIIu3Q

### Основные технические моменты:

* Физика на объекте используется с помощью Rigidbody 2D.
* В коде устроена автоматическая проверка по фиксированным и свободным слотам,
при добавлении новых слотов в инспекторе Unity изменения в коде не требуются.
* Размер, ротация сбрасываются при помещении объекта в слот.
* Свободный слот - самая сложная механика среди остальных:
    - При помещении объекта на "полку" - он уменьшается пропорционально  
дистанции между нижней гранью яблока и нижней гранью полки.  
Чем дальше грань яблока от грани полки тем объект меньше.

* Объект имеет свободу позиции, однако при отпускании его  
возле одного из фиксированных слотов объект магнитится.
* Скроллинг реализован при помощи сравнения левого края  
видимости камеры и левого края заднего фона, аналогично  
с правым краем.
* Для слотов стоят скрипты-пустышки для обозначения их  
в адресации кода, а также возможности модификации.
* Границы помещения свободных слотов реализованы не с помощью  
SpriteRenderer, а с помощью BoxCollider2D. Но при этом  
их можно спокойно реализовать без него.

### Недочеты:
* Реализации глубины (Свободный слот) имеет свои недочеты:  
- По мере увеличивания размера слота без редактирования кода  
изменения размеров может пойти в обратную сторону.
- Возможна более детальная и индивидуальная настройка чисел  
для точного местоположения после изменения размера и позиции.
