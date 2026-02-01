# Project Architecture Agreement

Este documento define la arquitectura acordada para el proyecto.
El objetivo es mantener una estructura clara, coherente y escalable.

---

## Principios

1. Organización por responsabilidad primero, temática después.
2. Vertical slices sobre carpetas globales.
3. Separación entre source art y assets de Unity.
4. Consistencia sobre perfección.

---

## Estructura base

```txt
/Assets
  /Kits
```

Todo el contenido del proyecto vive bajo `Assets/Kits`.

---

## Kits

```txt
/Kits
  /Characters
  /World
  /Common
  /UI
```

---

## Characters

```txt
/Characters
  /Common
    /Scripts
    /Sprites
  /Player
    /Scripts
    /Sprites
    /Animations
    /AnimatorControllers
    /Prefabs
  /Enemies
    /Skeleton
      /Scripts
      /Sprites
      /Animations
      /AnimatorControllers
      /Prefabs
```

Reglas:

- Si se mueve y actúa autónomamente, es un Character.
- Lógica compartida en `Characters/Common`.

---

## World

```txt
/World
  /Tilesets
  /Props
  /Scenes
  /Common
```

---

## Tilesets

```txt
/World/Tilesets
  /Dungeon
    /Raw
    /Tiles
    /TilePalettes
```

### Raw

- Source art.
- Puede estar sliceado.
- No contiene assets creados por Unity.

### Tiles

- Tile assets (`.asset`).

---

## Props

```txt
/World/Props
  /Dungeon
    /Barrel
      /Prefabs
      /Sprites
      /Scripts
```

- Tilemap → Tileset.
- GameObject → Prop.
- Si tiene AI → Character.

---

## Scenes

```txt
/World/Scenes
```

Las escenas representan la composición del mundo.

---

## Common

```txt
/Common
  /Scripts
  /Materials
  /Shaders
  /Textures
  /Input
```

Aquí viven los assets compartidos entre kits.

---

## Input

- Deben ubicarse en `Common/Input`.
- Un asset, múltiples Action Maps.

---

## Reglas clave

- Characters se mueven.
- World se recorre.
- Common se comparte.
- Dominio primero, tema después.
- Repetir nombres de biomas es intencional (Props/Tilesets).
