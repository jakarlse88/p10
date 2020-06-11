// ReSharper disable InconsistentNaming
namespace Abarnathy.BlazorClient.Client.Models
{
    public enum PatientSingleOperationStatusEnum
    {
        Initial = 0,
        GET_Pending,
        GET_Success,
        GET_Error,
        PUT_Pending,
        PUT_Success,
        PUT_Error
    }
}