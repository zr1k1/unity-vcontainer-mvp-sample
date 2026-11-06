
namespace Isaev.Network {
    public enum ApiError {
        NoConnection,     // нет интернета
        Timeout,          // сервер не ответил вовремя
        NotFound,         // 404 — такого ресурса нет
        ServerError,      // 500 и выше — проблема на сервере
        InvalidResponse   // ответ пришёл, но JSON не разобрался
    }
}