using BraAutoDb.Models;
using BraAutoDb.Models.UsersSearch;

namespace BraAutoDb.Dal
{
    public class Users : BaseDal<User>
    {
        public Users() : base("user", "id", "created_at") { }

        public Response Search(Request request)
        {
            return this.Search<Response>(request,
                (query) =>
                {
                    if (!string.IsNullOrEmpty(request.Keywords)) { query.Where.Add("AND (u.name LIKE @keywords)"); }
                    if (request.UserTypeId.HasValue) { query.Where.Add(" AND user_type_id = @userTypeId"); }
                    if (request.LocationId.HasValue) { query.Where.Add(" AND location_id = @locationId"); }
                },
                () =>
                {
                    return new 
                    {
                        keywords = string.Format("%{0}%", request.Keywords),
                        userTypeId = request.UserTypeId,
                        locationId = request.LocationId
                    };
                },
                "u");
        }

        public User GetByUsername(string username) => this.GetByFieldValues<string>("username", new string[] { username }).FirstOrDefault();

        public User GetByEmail(string email) => this.GetByFieldValues<string>("email", new string[] { email }).FirstOrDefault();

        public User GetByUsernameAndPassword(string username, string password)
        {
            var sql = @"
                SELECT *
                FROM user
                WHERE username = @username AND password = fn_password(@password)";

            return Db.Mapper.Query<User>(sql, new { username, password }).FirstOrDefault();
        }

        public List<User> GetByTypeId(uint userTypeId) => this.GetByFieldValues("user_type_id", new uint[] { userTypeId });

        public void Insert(User user)
        {
            var sql = @"INSERT INTO `user`
                        (
                            `username`,
                            `password`,
                            `name`,
                            `email`,
                            `birthday`,
                            `mobile`,
                            `description`,
                            `location_id`,
                            `specific_location`,
                            `is_active`,
                            `user_type_id`,
                            `photo_url`,
                            `booking_interval_hours`,
                            `start_working_time`,
                            `end_working_time`,
                            `max_bookings_per_day`,
                            `created_at`,
                            `edited_at`
                        )VALUES(
                            @username,
                            fn_password(@password),
                            @name,
                            @email,
                            @birthday,
                            @mobile,
                            @description,
                            @locationId,
                            @specificLocation,
                            @isActive,
                            @userTypeId,
                            @photoUrl,
                            @bookingIntervalHours,
                            @startWorkingTime,
                            @endWorkingTime,
                            @maxBookingsPerDay,
                            NOW(),
                            NOW()
                        );

                        SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                username = user.Username,
                password = user.Password,
                name = user.Name,
                email = user.Email,
                birthday = user.Birthday,
                mobile = user.Mobile,
                description = user.Description,
                locationId = user.LocationId,
                specificLocation = user.SpecificLocation,
                isActive = user.IsActive,
                userTypeId = user.UserTypeId,
                photoUrl = user.PhotoUrl,
                bookingIntervalHours = user.BookingIntervalHours,
                startWorkingTime = user.StartWorkingTime,
                endWorkingTime = user.EndWorkingTime,
                maxBookingsPerDay = user.MaxBookingsPerDay
            };

            user.Id = Db.Mapper.Query<uint>(sql, queryParams).FirstOrDefault();
        }

        public void Update(User user)
        {
            string sql = @"
                UPDATE `user`
                     SET name = @name,
                         email = @email,
                         birthday = @birthday,
                         mobile = @mobile,
                         description = @description,
                         location_id = @locationId,
                         specific_location = @specificLocation,
                         is_active = @isActive,
                         user_type_id = @userTypeId,
                         photo_url = @photoUrl,
                         booking_interval_hours = @bookingIntervalHours,
                         start_working_time = @startWorkingTime,
                         end_working_time = @endWorkingTime,
                         max_bookings_per_day = @maxBookingsPerDay,
                         editor_id = @editorId,
                         edited_at = NOW()
                WHERE id = @id";

            var queryParams = new
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                birthday = user.Birthday,
                mobile = user.Mobile,
                description = user.Description,
                locationId = user.LocationId,
                specificLocation = user.SpecificLocation,
                isActive = user.IsActive,
                userTypeId = user.UserTypeId,
                photoUrl = user.PhotoUrl,
                bookingIntervalHours = user.BookingIntervalHours,
                startWorkingTime = user.StartWorkingTime,
                endWorkingTime = user.EndWorkingTime,
                maxBookingsPerDay = user.MaxBookingsPerDay,
                editorId = user.EditorId
            };

            Db.Mapper.Execute(sql, queryParams);
        }

        public void SetPassword(uint id, string password)
        {
            string sql = @"
                UPDATE `user`
                    SET password = fn_password(@password)
                WHERE id = @id";

            Db.Mapper.Execute(sql, new { id, password });
        }


        public void LoadEditors(IEnumerable<User> users)
        {
            Db.LoadEntities(users, u => u.EditorId, editorIds => Db.Users.GetByIds(editorIds), (user, editors) => user.Editor = editors.FirstOrDefault(e => e.Id == user.EditorId));
        }
    }
}