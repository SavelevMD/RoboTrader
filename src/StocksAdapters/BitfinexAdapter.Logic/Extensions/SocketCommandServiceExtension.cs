
using BitfinexAdapter.Logic.Extensions.Bitfinex.Net.Extensions.Enums;
using BitfinexAdapter.Logic.Services;

using Brocker.Model;

using Microsoft.Extensions.Logging;

namespace BitfinexAdapter.Logic.Extensions
{
    internal static class SocketCommandServiceExtension
    {
        public static void InfoMessageProcessing(this ISocketCommandService socketCommandService, InfoMessage message, ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(message.Message))
            {
                socketCommandService.IsMaintenanceMode = message.Platform.Status == 0;
            }
            else
            {
                switch (message.Code)
                {
                    case ErrorCodes.EVT_INFO:
                        logger.LogInformation($"Информационное сообщение: {message}");
                        break;
                    case ErrorCodes.EVT_RESYNC_START:
                        socketCommandService.IsMaintenanceMode = true;
                        logger.LogInformation($"Биржа перешела в режим технического обслуживания, " +
                            $"все операции в данный момент не доступны, но вы можете отправить таск на выполнение, " +
                            $"который будет обработан сразу после окончания техничесого обслуживания: {message}");
                        break;
                    case ErrorCodes.EVT_RESYNC_STOP:
                        logger.LogInformation($"Режим технического обслуживания закончился: {message}");
                        //20061 : Maintenance ended.You can resume normal activity. 
                        //It is advised to unsubscribe/subscribe again all channels.
                        socketCommandService.RefreshSubscriptions();
                        break;
                    case ErrorCodes.EVT_STOP:
                        //20051 : Stop / Restart Websocket Server(please reconnect)
                        socketCommandService.ReconnectCommand();
                        break;
                    default:
                        logger.LogError($"Данный код ошибки не определен в системе {message}");
                        break;
                }
            }
        }

        public static void ErrorMessageProcessing(this ISocketCommandService socketCommandService, ErrorMessage message, ILogger logger)
        {
            switch (message.Code)
            {
                case ErrorCodes.ERR_AUTH_FAIL: logger.LogError($"Не пройдена аутентификация: {message}"); break;
                case ErrorCodes.ERR_AUTH_HMAC: logger.LogError($"Ошибка в шифровании запроса аутентификации: {message}"); break;
                case ErrorCodes.ERR_AUTH_NONCE: logger.LogError($"Ошибка в запросе аутентификации: {message}"); break;
                case ErrorCodes.ERR_AUTH_PAYLOAD: logger.LogError($"Ошибка в запросе аутентификации: {message}"); break;
                case ErrorCodes.ERR_AUTH_SIG: logger.LogError($"Ошибка в подписи запроса аутентификации: {message}"); break;
                case ErrorCodes.ERR_CONCURRENCY: logger.LogError($"Ваще не понятно чо это такое: {message}"); break;
                case ErrorCodes.ERR_CONF_FAIL: logger.LogError($"Настройка конфигурации не удалась: {message}"); break;
                case ErrorCodes.ERR_GENERIC: logger.LogError($"ХЫ!! Общая ошибка: {message}"); break;
                case ErrorCodes.ERR_PARAMS: logger.LogError($"Ошибка параметров запроса: {message}"); break;
                case ErrorCodes.ERR_READY: socketCommandService.IsMaintenanceMode = true; break;//_logger.LogError($"Не готово; break; попробуйте позже: {message}"; break; true); break;
                case ErrorCodes.ERR_SUB_FAIL: logger.LogError($"Сбой подписки на канал: {message}"); break;
                case ErrorCodes.ERR_SUB_MULTI: logger.LogError($"Сбой подписки на канал: уже подписан: {message}"); break;
                case ErrorCodes.ERR_UNAUTH_FAIL: logger.LogError($"Ошибка в запросе отмены аутентификации: {message}"); break;
                case ErrorCodes.ERR_UNK: logger.LogError($"ТЫДЫЩ!!! Неизвестная ошибка: {message}"); break;
                default: logger.LogError("Данный код ошибки не определен в системе", true); break;
            };
        }
    }
}
