# Third-party notices

This project contains only the original plugin source, its compiled plugin, and ManosabaLoader mode metadata. It does not redistribute game scripts, game assets, save files, BepInEx, Il2CppInterop, ManosabaLoader, HarmonyX, or Naninovel/game assemblies.

- **ManosabaMod / ManosabaLoader (v2.0.4)** — consulted to understand the debug workflow and mode metadata shape. No source code is embedded in this plugin. Upstream project: https://github.com/IrisuM/ManosabaMod ; upstream license: LGPL-2.1.
- **BepInEx 6 and Il2CppInterop** — runtime/build dependencies, not bundled. BepInEx project: https://github.com/BepInEx/BepInEx ; Il2CppInterop project: https://github.com/BepInEx/Il2CppInterop (LGPL-3.0).
- **Naninovel** — game-provided API types used as local build references; proprietary game/runtime assemblies are not included.
- **ILSpy** — used as a local inspection tool; no ILSpy code or binaries are included. Project/license: https://github.com/icsharpcode/ILSpy (MIT).
- **HarmonyX** — explored in an earlier discarded prototype only. The shipped plugin does not reference or bundle HarmonyX.

The plugin's own source code is licensed under MIT; see `LICENSE`. Third-party tools and dependencies remain under their respective licenses.
