using DapperMySqlMapper;
using SqlQueryBuilder.MySql;
using System.Data;
using BraAuto.Helpers.Extensions;

namespace BraAutoDb.Dal
{
    public static class Db
    {
        public static MySqlMapper Mapper { get; set; }

        public static void SetupConnection(string connectionString, int? unableToConnectToHostErrorRetryInterval = null, bool unableToConnectToHostErrorRetryTillConnect = false)
        {
            Db.Mapper = new MySqlMapper(connectionString, unableToConnectToHostErrorRetryInterval: unableToConnectToHostErrorRetryInterval, unableToConnectToHostErrorRetryTillConnect: unableToConnectToHostErrorRetryTillConnect);
        }

        public static void LoadEntities<TEntity, TPKeyEntity>(IEnumerable<TEntity> entities,
           Func<TEntity, uint> pKeySelector,
           Func<IEnumerable<uint>, ICollection<TPKeyEntity>> pKeyEntitiesSelector,
           Action<TEntity, ICollection<TPKeyEntity>> map)
           where TEntity : class
           where TPKeyEntity : class
        {
            if (entities.IsNullOrEmpty()) { return; }

            var pKeyIds = entities.Select(pKeySelector).Distinct().ToList();

            if (pKeyIds.IsNullOrEmpty()) { return; }

            var pKeyEntities = pKeyEntitiesSelector(pKeyIds);

            foreach (var entity in entities)
            {
                map(entity, pKeyEntities);
            }
        }
        public static void LoadEntities<TEntity, TPKeyEntity>(IEnumerable<TEntity> entities,
            Func<TEntity, int> pKeySelector,
            Func<IEnumerable<int>, ICollection<TPKeyEntity>> pKeyEntitiesSelector,
            Action<TEntity, ICollection<TPKeyEntity>> map)
            where TEntity : class
            where TPKeyEntity : class
        {
            if (entities.IsNullOrEmpty()) { return; }

            var pKeyIds = entities.Select(pKeySelector).Distinct().ToList();

            if (pKeyIds.IsNullOrEmpty()) { return; }

            var pKeyEntities = pKeyEntitiesSelector(pKeyIds);

            foreach (var entity in entities)
            {
                map(entity, pKeyEntities);
            }
        }

        public static long QueryCount(IDbConnection connection, Query query, object param = null)
        {
            var totalsQuery = new Query(new string[] { "COUNT(*)" }, query.From, joins: query.Joins, where: query.Where, groupBy: query.GroupBy);
            var count = Db.Mapper.Query<long?>(connection, totalsQuery.Build(), param: param).FirstOrDefault();

            return count == null ? 0 : count.Value;
        }

        internal static bool ExistsByField(string tableName, string fieldName, string fieldValue, string idFieldName, object ignoreId = null, bool trimFieldValue = true)
        {
            if (trimFieldValue) { fieldValue = fieldValue?.Trim(); }

            string sql = $@"
                SELECT 1
                FROM {tableName} 
                WHERE {fieldName} = @fieldValue";

            if (ignoreId != null) { sql += $" AND {idFieldName} <> @ignoreId"; }

            return Db.Mapper.Query<int>(sql, param: new { fieldValue, ignoreId }).FirstOrDefault() == 1;
        }
    }
}
