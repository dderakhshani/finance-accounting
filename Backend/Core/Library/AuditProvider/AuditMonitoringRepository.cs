//using Library.Interfaces;
//using MongoDB.Bson;
//using MongoDB.Driver;

//namespace Library.AuditProvider
//{
//    public class AuditMonitoringRepository : IAuditMontiroringRepository
//    {
//        public IMongoDataProvider MongoDataProvider
//        {
//            get; set;
//        }

//        public AuditMonitoringRepository(IMongoDataProvider mongoDataProvider)
//        {
//            MongoDataProvider = mongoDataProvider;
//        }
//        public IMongoCollection<BsonDocument> Table()
//        {
//            return MongoDataProvider.Table();
//        }
//    }
//}