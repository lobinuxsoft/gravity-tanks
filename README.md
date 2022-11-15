# [Hyper Net Warrior]()

¡Entra en un mundo donde todos te quieren eliminar!
Eres hacker que atravez de un avatar eliminas virus que ponen en peligro a la red... eres el unico que puede mantener la estabilidad mundial.

Da un paso adelante, acumula asombrosas habilidades y manten la red estable, pues las interminables oleadas de virus nunca se rendirán. Y recuerda, una vez seas derrotado... ¡Tendrás que volver a empezar! ¡Asi que ten cuidado!
Disfruta creando incontables combinaciones de habilidades únicas, todas diseñadas para ayudarte a sobrevivir.

## **Características principales:**
- Habilidades únicas y aleatorias que te ayudan a avanzar en estas mazmorras.
- Miles de monstruos nunca vistos y originados para derrotar.
- Sube de nivel y mejora tu equipo.

---
## **Objetivo del juego:**
- Destruir oleadas de enemigos.
- Ganar dinero.
- Comprar mejoras.

---
## **Controles**
- Tactiles simples, solo hay que deslizar el dedo en la pantalla para mover el avatar.

---
## **Patrones de diseño**
 - [**Builder**](https://refactoring.guru/es/design-patterns/builder)
  > Este patron lo utilizo para la creacion de la nave, armas, chassis, motor...
    toda la creacion esta basada en este patron, se encuentra en la carpeta [**Assets/Scripts/Patterns/Builder**](https://github.com/lobinuxsoft/hyper-net-warrior/tree/development/Assets/Scripts/Patterns/Builder).

 - [**Abstract Factory**](https://refactoring.guru/es/design-patterns/abstract-factory)
  > Este patron lo utilizo para crear/instanciar diferentes tipos de enemigos, se encuentra en [**Assets/Scripts/Spawner.cs**](https://github.com/lobinuxsoft/hyper-net-warrior/blob/development/Assets/Scripts/Spawner.cs).

 - [**Object Pool**](https://en.wikipedia.org/wiki/Object_pool_pattern#:~:text=The%20object%20pool%20pattern%20is,operations%20on%20the%20returned%20object.)
  > Este patron lo utilizo para todos los objetos que tienen la posibilidad de reutilizarse (los enemigos implementan el patron abstract factory combinado con este patron), este se encuentra en [**Assets/Scripts/Utils/ObjectPool.cs**](https://github.com/lobinuxsoft/hyper-net-warrior/blob/development/Assets/Scripts/Utils/ObjectPool.cs).