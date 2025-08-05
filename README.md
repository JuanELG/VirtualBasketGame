# BasketGame Prototipo (Unity3D con Codex)

Este repositorio contiene tu prototipo de juego de basketball en Unity 6, preparado para trabajar con OpenAI Codex como asistente de desarrollo. El objetivo es crear un juego estilo arcade: lanzas balones hacia una cesta con joystick virtual y diferentes niveles de dificultad (cesta fija, cesta en movimiento, etc.).

## 🎮 Idea del Juego

- Máquina de arcade donde se lanzan balones a una cesta.
- Niveles con mecánicas crecientes de dificultad (cesta fija, cesta móvil, etc.).
- Controles responsivos con joystick virtual: dirección, fuerza y ángulo de tiro.
- Trayectoria en parábola basada en cuánto se mueve el joystick hacia abajo.

## 🔧 Implementación actual

- **Input System v1.13.0**: Prefab con `On-Screen Stick`, control path asignado, acciones generadas en `GameControls`.
- `InputManager` implementa `IInputManager`, con eventos `AimStarted`, `AimPerformed(Vector2)` y `AimCanceled`, y propiedad `AimVector`.
- `PlayerController` se suscribe a dichos eventos, almacena el último valor útil del joystick, y aplica un impulso (componente horizontal, vertical y forward) para generar tiro parabólico.
- Ajustes físicos recomendados: masa ≈ 0.6 kg, drag bajo, physic material con bounciness ≈ 0.7–0.8, fricción baja (≈ 0.2).

## 🧩 Arquitectura y Buenas Prácticas

- Patrón **Service Locator** usado para desacoplar el input y facilitar tests o cambios de implementación:
  - `ServiceLocatorProject`, `ServiceLocatorScene`, `Installer`, `Bootstrapper`.
  - Registro de `IInputManager` mediante `InputSystemInstaller`.
- Principios **SOLID** observados en el diseño:
  - **S**ingle Responsibility: cada clase hace una sola cosa (InputManager, BallController, PlayerController).
  - **O**pen/Closed: ampliable sin modificar clases existentes (niveles, otros inputs).
  - **D**ependency Inversion: el controlador depende de la abstracción `IInputManager`, no de una implementación concreta.

## 🧪 Tests y CI/CD

- Estructura de pruebas: EditMode y PlayMode en Unity Test Framework.
- Generación de resultados en `test-results/` en tu pipeline.
- El script `setup.sh` prepara el entorno incluyendo:
  - Unity Hub, editor 2025.1.0f1, módulos Android (SDK, NDK, OpenJDK).
  - .NET SDK, Git LFS, libs para UI.
  - Verificación de carga del proyecto, restauración de .NET, ejecución de tests.

## 🚀 Cómo usar con Codex

1. Conecta tu repositorio a Codex (CLI o interfaz).
2. Ejecuta `setup.sh` como Script de Setup (asegúrate que `.env` tenga tu API key).
3. Codex leerá `AGENTS.md` para entender estilo, patrones y estructura.
4. Si hay tests, Codex debe ejecutarlos tras cada modificación.
5. Usa prompts que indiquen claramente el cambio deseado, el archivo afectado y el contexto del juego.