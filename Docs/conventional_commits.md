# Conventional Commits

Este proyecto utiliza el estándar **Conventional Commits** para mantener un historial de commits claro, consistente y fácil de analizar.

El objetivo es:

- Facilitar la revisión de cambios.
- Mejorar la trazabilidad del trabajo.
- Permitir generación automática de changelogs y versiones.
- Mantener coherencia en un proyecto con múltiples contribuidores.

---

## Formato del commit

Todos los commits deben seguir este formato:

```
<type>(<scope>): <short description>
```

Ejemplo:

```
feat(gameplay): add dash cooldown
```

### Reglas generales

- El mensaje debe estar **en inglés**.
- Usar **tiempo presente imperativo** (`add`, `fix`, `remove`).
- La descripción debe ser **clara y concisa**.
- Un commit debe representar **un solo cambio lógico**.
- No terminar la línea con punto (`.`).

---

## Types

### feat

Nueva funcionalidad jugable o técnica.

### fix

Corrección de un bug.

### perf

Mejoras de rendimiento.

### refactor

Reestructuración sin cambio de comportamiento.

### asset

Cambios de contenido (sprites, animaciones, audio, escenas).

### build

Configuración de proyecto, escenas, input, rendering.

### chore

Mantenimiento general del repositorio.

### docs

Documentación.

---

## Scopes comunes

- player
- enemy
- characters
- gameplay
- ai
- ui
- anim
- tileset
- world
- scene
- input
- rendering
- unity
- repo

---

## Ejemplos

```
asset(tileset): import and slice grass tileset (32x32)
build(scene): add and configure grid for tilemaps
feat(player): implement top-down movement
asset(player): add animator with idle and directional animations
build(rendering): configure sprite sorting layers
chore(scene): rename scene gameobject
```

---

## Breaking Changes

Para indicar cambios que rompen la compatibilidad con versiones anteriores se añade un símbolo de exclamación (`!`) después del scope (`<type>(<scope>)!:`) y se debe incluir una explicación en el cuerpo del commit que comience por `BREAKING CHANGE:`.

```
feat(save)!: change save file format

BREAKING CHANGE: old save files are not compatible
```
