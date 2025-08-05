# AGENTS.md – Guía para Codex

## Contexto del juego

- Juego estilo arcade: lanzamiento de balones a una cesta con joystick virtual.
- Mecánica de niveles con dificultad progresiva (cesta estática, en movimiento, etc.).
- El joystick define dirección, fuerza y ángulo; el tiro es parabólico según input.

## Convenciones de código y estilo

- Lenguaje: C# para Unity (v 6000.2.0b12), usando Input System v1.14.1.
- Clases en PascalCase, métodos en camelCase.
- Campos privados con `[SerializeField]` antes del tipo.
- Interfaces comienzan con `I` (como `IInputManager`).
- Código limpio que siga SOLID y patrón Service Locator.
- Estructura:
  - `VirtualBasketGame/Assets/_Scripts/Core/`: interfaces, servicios, ServiceLocator.
  - `VirtualBasketGame/Assets/_Scripts/InputSystem/`: InputManager y lógica de input.
  - `VirtualBasketGame/Assets/_Scripts/Dependencies/`: instaladores (Installer).
  - `VirtualBasketGame/Assets/_Scripts/Player/`: PlayerController, BallController, lógica de tiro.

## Patrones y diseño

- **Service Locator** para inyección de servicios (por ejemplo `IInputManager`).
- Principios SOLID:
  - **SRP**: una responsabilidad por clase.
  - **OCP**: agregar funcionalidad sin modificar clase existente.
  - **LSP**: funciones que usen interfaces respeten contrato.
  - **ISP & DIP**: usar interfaces específicas y depender de abstracciones.

## Implementaciones críticas

- `InputManager`:
  - Implementa `IInputManager`, con eventos `AimStarted`, `AimPerformed(Vector2)`, `AimCanceled` y propiedad `AimVector`.
- `PlayerController`:
  - Se suscribe a eventos del `IInputManager`, guarda el último `Vector2` útil y aplica fuerza al balón usando mass, drag y physic material acordes.
- Valores recomendados para física:
  - Rigidbody.mass ≈ 0.6
  - drag ≈ 0.1–0.2, angularDrag similar
  - Physic Material: bounciness ≈ 0.7–0.8, fricción baja ≈ 0.2

## Comportamiento de Codex

Cuando se solicite:

- Modificaciones de código: definir claramente qué archivo y qué funcionalidad (e.g. cambiar multiplicador de fuerza, ajustar physic material).
- PRs generados por Codex deben:
  - Título: `[Codex] <Descripción del cambio>`
  - Incluir sección `Testing Done:` actuando como reporte de verificación.
- Antes de aplicar mejoras, ejecutar tests si existen (`dotnet test`, `unity -runTests`).

## Consideraciones generales

- Divide tareas grandes en subtareas pequeñas.
- Revisa siempre diffs generados por el agente.
- Asegura que las modificaciones respeten la separación de capas: Input, dominio (lógica), física y presentación (UI).