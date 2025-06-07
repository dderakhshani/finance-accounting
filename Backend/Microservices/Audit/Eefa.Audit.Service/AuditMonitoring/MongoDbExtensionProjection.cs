using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Eefa.Audit.Service.AuditMonitoring
{
    public static class MongoDbExtensionProjection
    {
        public static IFindFluent<BsonDocument, BsonDocument> MyProject(this IFindFluent<BsonDocument, BsonDocument> findFluent, Action<IMyProject> config)
        {
            var projectConfigurator = new MyProjectConfigurator();
            config.Invoke(projectConfigurator);

            var projectionJson = "{";
            foreach (var property in projectConfigurator.GetType().GetProperties())
            {
                if (property?.PropertyType == typeof(bool))
                {
                    var iOe = (bool) (property.GetValue(projectConfigurator) ?? false);
                    if (iOe)
                    {
                        projectionJson += $"{property.Name}:{1},";
                    }
                }
            }

            projectionJson = projectionJson.Remove(projectionJson.LastIndexOf(','), 1);
            projectionJson += "}";
            findFluent.Options.Projection = projectionJson;
            return findFluent;
        }
    }
}