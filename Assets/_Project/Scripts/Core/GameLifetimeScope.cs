
using VContainer;
using VContainer.Unity;
using UnityEngine;
using Isaev.Features;
using Isaev.Network;
using Isaev.UI;

namespace Isaev.Core {
    public class GameLifetimeScope : LifetimeScope {
        [SerializeField] PostsConfig _postsConfig = default;
        [SerializeField] ApiConfig _apiConfig = default;
        [SerializeField] ErrorMessagesConfig _errorMessages = default;
        [SerializeField] PostView _postView = default;
        [SerializeField] PostsCounterView _postsCounterView = default;
        [SerializeField] bool _useFakeApi = false;
        [SerializeField] bool _fakeShouldFail = false;
        [SerializeField] ApiError _fakeError = ApiError.Timeout;

        protected override void Configure(IContainerBuilder builder) {
            builder.RegisterInstance(_apiConfig);
            builder.RegisterInstance(_errorMessages);
            builder.RegisterInstance(_postsConfig);
            builder.RegisterComponent(_postView);
            builder.RegisterComponent(_postsCounterView);
            builder.RegisterEntryPoint<PostPresenter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<PostsCounterPresenter>(Lifetime.Singleton);
            builder.Register<PostsModel>(Lifetime.Singleton);

            if (_useFakeApi) {
                ApiError? error = _fakeShouldFail ? _fakeError : null;
                builder.RegisterInstance<IApiClient>(new FakeApiClient(error));
            }
            else {
                builder.Register<IApiClient, ApiClient>(Lifetime.Singleton);
            }
        }
    }
}