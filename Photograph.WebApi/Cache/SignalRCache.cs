using System;
using System.Collections.Concurrent;

namespace Photograph.WebApi.Cache
{
    public class SignalRCache : ISignalRCache
    {
        private readonly ConcurrentDictionary<Guid, string> _userConnectionIdDictionary =
            new ConcurrentDictionary<Guid, string>();

        public string GetConnectionId(Guid key)
        {
            string value = string.Empty;
            _userConnectionIdDictionary.TryGetValue(key, out value);
            return value;
        }

        public void RemoveConnectionId(Guid id)
        {
            string value = string.Empty;
            _userConnectionIdDictionary.TryRemove(id, out value);
        }

        public void AddConnectionId(Guid id, string connectionId)
        {
            _userConnectionIdDictionary.AddOrUpdate(id, connectionId, (key, oldValue) => connectionId);
        }
    }
}