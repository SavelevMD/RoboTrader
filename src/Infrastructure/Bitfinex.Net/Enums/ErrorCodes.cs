namespace Bitfinex.Net.Enums
{
    /// <summary>
    /// Error response codes enum
    /// </summary>
    public enum ErrorCodes
    {
        /// <summary>
        /// Unknown error
        /// </summary>
        ERR_UNK = 10000,
        /// <summary>
        /// Generic error
        /// </summary>
        ERR_GENERIC = 10001,
        /// <summary>
        /// Concurrency error
        /// </summary>
        ERR_CONCURRENCY = 10008,
        /// <summary>
        /// Request parameters error
        /// </summary>
        ERR_PARAMS = 10020,
        /// <summary>
        /// Configuration setup failed
        /// </summary>
        ERR_CONF_FAIL = 10050,
        /// <summary>
        /// Failed authentication
        /// </summary>
        ERR_AUTH_FAIL = 10100,
        /// <summary>
        /// Error in authentication request payload
        /// </summary>
        ERR_AUTH_PAYLOAD = 10111,
        /// <summary>
        /// Error in authentication request signature
        /// </summary>
        ERR_AUTH_SIG = 10112,
        /// <summary>
        /// Error in authentication request encryption
        /// </summary>
        ERR_AUTH_HMAC = 10113,
        /// <summary>
        /// Error in authentication request nonce
        /// </summary>
        ERR_AUTH_NONCE = 10114,
        /// <summary>
        /// Error in un-authentication request
        /// </summary>
        ERR_UNAUTH_FAIL = 10200,
        /// <summary>
        /// Failed channel subscription
        /// </summary>
        ERR_SUB_FAIL = 10300,
        /// <summary>
        /// Failed channel subscription: already subscribed
        /// </summary>
        ERR_SUB_MULTI = 10301,
        /// <summary>
        /// Failed channel un-subscription: channel not found
        /// </summary>
        ERR_UNSUB_FAIL = 10400,
        /// <summary>
        /// Not ready, try again later
        /// </summary>
        ERR_READY = 11000,
        /// <summary>
        /// Websocket server stopping... please reconnect later
        /// </summary>
        EVT_STOP = 20051,
        /// <summary>
        /// Websocket server resyncing... please reconnect later
        /// </summary>
        EVT_RESYNC_START = 20060,
        /// <summary>
        /// Websocket server resync complete. please reconnect
        /// </summary>
        EVT_RESYNC_STOP = 20061,
        /// <summary>
        /// Info message
        /// </summary>
        EVT_INFO = 5000
    }
}
