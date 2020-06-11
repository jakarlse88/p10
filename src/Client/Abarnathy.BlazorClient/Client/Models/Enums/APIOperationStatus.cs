// ReSharper disable InconsistentNaming
namespace Abarnathy.BlazorClient.Client.Models
{
    // ReSharper disable once InconsistentNaming
    public enum APIOperationStatus
    {
        Initial,
        GET_Pending,
        GET_Success,
        GET_Error,
        POST_Pending,
        POST_Success,
        POST_Error,
        PUT_Pending,
        PUT_Success,
        PUT_Error
    }
}