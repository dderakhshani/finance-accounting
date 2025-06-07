using Eefa.NotificationServices.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.NotificationServices.Services.SignalR
{

    public class UserConnectionManager : IUserConnectionManager
    {
        private static readonly ConcurrentDictionary<string, HashSet<string>> userConnectionMap = new();
        public List<string> GetUserConnections(string userId)
        {
            if (userConnectionMap.TryGetValue(userId, out var connections))
            {
                return connections.ToList(); // Return a copy to prevent external modification
            }
            return new List<string>();
        }
        public void RemoveUserConnection(string connectionId)
        {
            foreach (var kvp in userConnectionMap.ToList())
            {
                if (kvp.Value.Remove(connectionId) && !kvp.Value.Any())
                {
                    userConnectionMap.TryRemove(kvp.Key, out _); // Remove user if no connections remain
                }
            }
        }
        public void AddUserConnection(string userId, string connectionId)
        {
            userConnectionMap.AddOrUpdate(userId, _ => new HashSet<string> { connectionId }, // If user doesn't exist, create a new HashSet
            (_, connections) =>
               {
                   connections.Add(connectionId);
                   return connections;
               });
        }

    }
}
