using BraAutoDb.Models;
using BraAutoDb.Models.Search;
using SqlQueryBuilder.MySql;

namespace BraAutoDb.Dal
{
    public abstract class BaseDal<T> where T : BaseModel<uint>
    {
        protected readonly string _table;
        protected readonly string _idField;
        protected readonly string _sortField;
        protected readonly bool _sortDesc;
        public BaseDal(string table, string id, string sortField, bool sortDesc = false)
        {
            _table = table;
            _idField = id;
            _sortField = sortField;
            _sortDesc = sortDesc;
        }

        public T GetById(uint id)
        {
            string sql = $@"
                SELECT * 
                FROM `{_table}`
                WHERE `{_idField}` = @id";

            return Db.Mapper.Query<T>(sql, param: new { id }).FirstOrDefault();
        }

        public List<T> GetByIds(IEnumerable<uint> ids, int? size = null, int? offset = null)
        {
            var query = new Query()
            {
                Select = new List<string>() { "*" },
                From = _table,
                Where = new List<string> { $"`{_idField}` IN @ids" },
                Limit = new Limit(offset, size)
            };

            return Db.Mapper.Query<T>(query.ToString(), param: new { ids }).ToList();
        }

        public List<T> GetAll(int? size = null, int? offset = null)
        {
            var query = new Query()
            {
                Select = new List<string>() { "*" },
                From = _table,
                Limit = new Limit(offset, size),
                OrderBys = new List<OrderBy> { new OrderBy(_sortField, descending: _sortDesc) }
            };

            return Db.Mapper.Query<T>(query.ToString()).ToList();
        }

        protected List<T> GetByFieldValues<TFieldType>(string field, IEnumerable<TFieldType> values)
        {
            string sql = $@"
                SELECT * 
                FROM `{_table}`
                WHERE `{field}` IN @values
                ORDER BY `{_sortField}` {(_sortDesc ? "DESC" : "")}";

            return Db.Mapper.Query<T>(sql, param: new { values }).ToList();
        }

        protected void Delete(uint id) => Delete(new uint[] { id });

        protected void Delete(IEnumerable<uint> ids) => DeleteByField(ids, _idField);

        protected void DeleteByField(IEnumerable<uint> ids, string field)
        {
            string sql = $@"
                DELETE FROM `{_table}` 
                WHERE `{field}` IN @ids";

            Db.Mapper.Execute(sql, new { ids });
        }

        protected TResponse Search<TResponse>(IBaseRequest request, Action<Query> prepareQuery, Func<dynamic> getQueryParams, string tableAlias) where TResponse : BaseResponse<T>, new()
        {
            if (string.IsNullOrEmpty(request.SortColumn))
            {
                request.SortColumn = $"{tableAlias}.{_sortField}";
                request.SortDesc = _sortDesc;
            }

            var query = new Query();

            query.Select = new List<string>() { "*" };
            query.From = $"{_table} {tableAlias}";
            query.Joins = new List<string>();
            query.Where = new List<string>() { "1 = 1" };
            query.OrderBys = new List<OrderBy>() { new OrderBy(request.SortColumn, request.SortDesc) };
            query.Limit = new Limit(request.Offset, request.RowCount);

            prepareQuery(query);

            var response = new TResponse();

            using (var connection = Db.Mapper.GetConnection())
            {
                var queryParams = getQueryParams();

                //get TotalRecords
                if (request.ReturnTotalRecords)
                {
                    response.TotalRecords = Db.QueryCount(connection, query, param: queryParams);

                    if (response.TotalRecords <= 0) { return response; }
                }

                if (request.ReturnRecords)
                {
                    response.Records = Db.Mapper.Query<T>(connection, query.Build(), param: queryParams);
                }
            }

            return response;
        }
    }
}
