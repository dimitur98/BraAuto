using DapperMySqlMapper;
using SqlQueryBuilder.MySql;
using System.Data;
using BraAuto.Helpers.Extensions;

namespace BraAutoDb.Dal
{
    public class Db
    {
        public static MySqlMapper Mapper { get; set; }

        public static void SetupConnection(string connectionString, int? unableToConnectToHostErrorRetryInterval = null, bool unableToConnectToHostErrorRetryTillConnect = false)
        {
            Db.Mapper = new MySqlMapper(connectionString, unableToConnectToHostErrorRetryInterval: unableToConnectToHostErrorRetryInterval, unableToConnectToHostErrorRetryTillConnect: unableToConnectToHostErrorRetryTillConnect);
        }

        public static Articles Articles = new Articles();
        public static BodyTypes BodyTypes = new BodyTypes();
        public static CarInCarServices CarInCarServices = new CarInCarServices();
        public static Cars Cars = new Cars();
        public static CarImgs CarImgs = new CarImgs();
        public static CarViews CarViews = new CarViews();
        public static CarServices CarServices = new CarServices();
        public static Categories Categories = new Categories();
        public static Colors Colors = new Colors();
        public static Conditions Conditions = new Conditions();
        public static DoorNumbers DoorNumbers = new DoorNumbers();
        public static EuroStandarts EuroStandarts = new EuroStandarts();
        public static FuelTypes FuelTypes = new FuelTypes();
        public static GearboxTypes GearboxTypes = new GearboxTypes();
        public static Locations Locations = new Locations();
        public static Makes Makes = new Makes();
        public static Models Models = new Models();
        public static Stages Stages = new Stages();
        public static UserInRoles UserInRoles = new UserInRoles();
        public static UserRoles UserRoles = new UserRoles();
        public static Users Users = new Users();
        public static UserCars UserCars = new UserCars();
        public static UserCarTypes UserCarTypes = new UserCarTypes();
        public static UserTypes UserTypes = new UserTypes();
        public static VehicleTypes VehicleTypes = new VehicleTypes();
        public static Years Years = new Years();


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
