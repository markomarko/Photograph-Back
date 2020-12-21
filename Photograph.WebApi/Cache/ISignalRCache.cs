using System;

namespace Photograph.WebApi.Cache
{
    public interface ISignalRCache
    {
        string GetConnectionId(Guid key);
        void RemoveConnectionId(Guid id);
        void AddConnectionId(Guid id, string connectionId);
    }
}