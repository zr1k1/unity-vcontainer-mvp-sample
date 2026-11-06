# Unity Posts Demo — VContainer · UniTask · UniRx · MVP

A small Unity sample that creates posts through a REST API
([JSONPlaceholder](https://jsonplaceholder.typicode.com)) and shows a
clean, testable architecture for a mobile UI feature.

## What it shows

- **MVP** — dumb `MonoBehaviour` views, pure C# presenters with constructor injection.
- **VContainer** — a single `GameLifetimeScope`, entry points (`IStartable`, `IDisposable`),
  configs registered as instances, `IApiClient` swapped between real and fake implementations.
- **UniTask** — async requests with cancellation (`GetCancellationTokenOnDestroy`)
  and a timeout via a linked `CancellationTokenSource`.
- **REST** — `UnityWebRequest` + Newtonsoft.Json, generic `SendAsync<T>`,
  separate request/response DTOs, HTTP and network errors mapped to `ApiError` / `ApiException`.
- **UniRx** — `ReactiveProperty` state, `CombineLatest` for button availability,
  `ThrottleFirst` against double clicks, `CompositeDisposable` cleanup.
- **Assembly Definitions** — one-way dependencies: `Core → Features → UI / Network`.

## Project structure

```
Assets/
├── _Project/
│   ├── Configs/          ScriptableObject configs (API, posts limit, error messages)
│   ├── Scenes/           MainScene
│   └── Scripts/
│       ├── Core/         GameLifetimeScope (composition root)
│       ├── Features/     Model, Presenters, Views
│       ├── Network/      Client, Config, Dto, Errors
│       └── UI/           Error messages config
└── ThirdParty/           SerializableDictionary (MIT)
```

## How to run

1. Open the project in Unity 6000.6.0f1 (URP).
2. Open `Assets/_Project/Scenes/MainScene.unity` and press Play.
3. On the `GameLifetimeScope` object you can switch to the fake API
   (`Use Fake Api`) and force an error (`Fake Should Fail`, `Fake Error`)
   to check timeout / network / server error handling without the internet.

## Dependencies

- [VContainer](https://github.com/hadashiA/VContainer)
- [UniTask](https://github.com/Cysharp/UniTask)
- [UniRx](https://github.com/neuecc/UniRx)
- Newtonsoft.Json (`com.unity.nuget.newtonsoft-json`)
- TextMeshPro

## Third-party code

- [SerializableDictionary](https://github.com/azixMcAze/Unity-SerializableDictionary)
  by Mathieu Le Ber (azixMcAze), MIT — see `Assets/ThirdParty/LICENSE`.
