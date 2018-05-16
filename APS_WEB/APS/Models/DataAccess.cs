using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace APS.Models
{
    public class DataAccess
    {
        private static string _connectionString;
        private static string _databaseName;
        private MongoClient _client;
        private MongoServer _server;
        private MongoDatabase _db;

        public DataAccess()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MongoDB"].ToString();
            _databaseName = MongoUrl.Create(_connectionString).DatabaseName;
            _client = new MongoClient(_connectionString);
            _server = _client.GetServer();
            _db = _server.GetDatabase(_databaseName);
        }
        #region Sections  ------------------------------------------------------------------------------------------------------------------
        public IEnumerable<Section> GetSections()
        {
            return _db.GetCollection<Section>("Sections").FindAll();
        }
        public Section GetSection(ObjectId id)
        {
            var res = Query<Section>.EQ(p => p.Id, id);
            return _db.GetCollection<Section>("Sections").FindOne(res);
        }
        public Section CreateSection(Section p)
        {
            _db.GetCollection<Section>("Sections").Save(p);
            return p;
        }
        public void Update(ObjectId id, Section p)
        {
            var res = Query<Section>.EQ(pd => pd.Id, id);
            var operation = Update<Section>.Replace(p);
            _db.GetCollection<Section>("Sections").Update(res, operation);
        }
        public void Remove(ObjectId id)
        {
            var res = Query<Section>.EQ(e => e.Id, id);
            var operation = _db.GetCollection<Section>("Sections").Remove(res);
        }
        public bool ValidateSection(string path)
        {

            var res = Query<Section>.EQ(p => p.Path, path);
            var count = _db.GetCollection<Section>("Sections").Count(res);
            if (count > 0)
                return true;
            else
                return false;
        }
        public bool HasChildren(string path)
        {

            var res = Query<Section>.EQ(p => p.Path, path);
            var sections = _db.GetCollection<Section>("Sections").Find(res);

            foreach (Section s in sections)
            {
                //res = Query<Section>.EQ(p => p.Parent, s.Child);
                //var hasChild = _db.GetCollection<Section>("Sections").Count(res);
                if (s.Child == "none")
                    return false;
            }

            return true;
        }
        public IEnumerable<Section> GetSectionsByPath(string path)
        {
            var res = Query<Section>.EQ(p => p.Path, path);
            var sections = _db.GetCollection<Section>("Sections").Find(res);

            return sections;
        }
        public Section GetSectionByPath(string path)
        {
            var res = Query<Section>.EQ(p => p.Path, path);
            var sections = _db.GetCollection<Section>("Sections").FindOne(res);

            return sections;

        }
        public IEnumerable<Section> GetSectionsByParent(string parent)
        {
            var res = Query<Section>.EQ(p => p.Parent, parent);
            var sections = _db.GetCollection<Section>("Sections").Find(res);

            return sections;
        }
        public void AddSection(SectionNew s)
        {
            var section = GetSection(ObjectId.Parse(s.Id));
            var haschildren = HasChildren(section.Path + "/" + section.Child);
            Section newSection = new Section();
            if (haschildren)
            {
                newSection.Parent = section.Child;
                newSection.Child = s.Child;
                newSection.Path = section.Path == "" ? section.Child : section.Path + "/" + section.Child;
                CreateSection(newSection);

                var endSection = new Section()
                {
                    Parent = s.Child,
                    Child = "none",
                    Path = newSection.Path + "/" + s.Child,
                    Columns = s.Columns,
                    Fields = s.Fields
                };
                CreateSection(endSection);

            }
            else
            {
                newSection = GetSectionByPath(section.Path + "/" + section.Child);
                newSection.Child = s.Child;
                Update(newSection.Id, newSection);

                var endSection = new Section()
                {
                    Parent = s.Child,
                    Child = "none",
                    Path = newSection.Path + "/" + s.Child,
                    Columns = s.Columns,
                    Fields = s.Fields
                };
                CreateSection(endSection);
            }
        }
        public void DeleteSection(string Id)
        {
            var section = GetSection(ObjectId.Parse(Id));
            var allSections = GetSections().ToList();
            var sectionPath = section.Path + "/" + section.Child;
            var lowerSections = allSections.Where(s => s.Path.StartsWith(sectionPath));
            var length = allSections.Where(s => s.Path == section.Path);
            foreach (Section s in lowerSections)
            {
                Remove(s.Id);
            }
            if (length.Count() <= 1)
            {
                var endSection = new Section()
                {
                    Parent = section.Parent,
                    Child = "none",
                    Path = section.Path
                };
                CreateSection(endSection);
            }
            Remove(section.Id);
        }
        #endregion
        #region Classifieds  ------------------------------------------------------------------------------------------------------------------
        public void DeleteClassified(string id)
        {
            var res = Query<ClassifieldModel>.EQ(e => e.Id, ObjectId.Parse(id));
            var operation = _db.GetCollection<ClassifieldModel>("Classifields").Remove(res);
        }
        public IEnumerable<ClassifieldModel> GetClassifieldAll()
        {
            return _db.GetCollection<ClassifieldModel>("Classifields").FindAll();
        }
        public IEnumerable<ClassifieldModel> GetClassifieldsById(string id) {
            var res = Query<ClassifieldModel>.EQ(c => c.SectionId, id);
            var classifields = _db.GetCollection<ClassifieldModel>("Classifields").Find(res);

            return classifields;
        }
        public void ResolveExpiredClassifieds()
        {
            var res = Query<ClassifieldModel>.Where(c => c.Status == Status.Public && c.S_endDate < DateTime.Now);
            var classifieds = _db.GetCollection<ClassifieldModel>("Classifields").Find(res);
            foreach (var classified in classifieds)
            {
                NoficationManager nm = new NoficationManager(UserCulture(classified.S_userId));
                AddNotification(nm.ClassifiedRejected(classified.S_userId));
                var res2 = Query<ClassifieldModel>.EQ(c => c.Id, classified.Id);
                var update = Update<ClassifieldModel>.Set(c => c.Status, Status.Expired);
                _db.GetCollection<ClassifieldModel>("Classifields").Update(res, update);
            }

        }
        public IEnumerable<ClassifieldModel> GetClassifieldsPublishedById(string id)
        {
            var res = Query<ClassifieldModel>.EQ(c => c.SectionId, id);
            var classifields = _db.GetCollection<ClassifieldModel>("Classifields").Find(res).Where(c => c.Status == Status.Public);

            return classifields;
        }
        public ClassifieldModel GetClassifield(string id)
        {
            var res = Query<ClassifieldModel>.EQ(p => p.Id, ObjectId.Parse(id));
            return _db.GetCollection<ClassifieldModel>("Classifields").FindOne(res);
        }
        public void CreateSClassifield(ClassifieldModel c)
        {
            _db.GetCollection<ClassifieldModel>("Classifields").Save(c);
        }
        public IEnumerable<ClassifieldModel> GetClassifieldsByUserId(string userId)
        {
            var res = Query<ClassifieldModel>.EQ(p => p.S_userId, userId);
            var classifieds = _db.GetCollection<ClassifieldModel>("Classifields").Find(res);

            return classifieds;
        }
        public IEnumerable<ClassifieldModel> GetMarkedClassifiedsByUser(string userId)
        {
            var res = Query<ClassifieldModel>.EQ(p => p.Status, Status.Public);
            var classifieds = _db.GetCollection<ClassifieldModel>("Classifields").Find(res)
                                    .Where(c => c.Marks != null && c.Marks.Contains(userId));
            return classifieds;
        }
        public ClassifiedViewModel GetClassifiedViewModel(string id, string ipAdress) {
            ClassifieldModel c = GetClassifield(id);

            # region Update Viewers
            try
            {
                bool hasVisied = false;
                c.Viewers = c.Viewers == null ? new List<string>() : c.Viewers;
                foreach (string viewer in c.Viewers)
                {
                    if (viewer == ipAdress)
                    {
                        hasVisied = true;
                    }
                }
                if (!hasVisied)
                {
                    c.Viewers.Add(ipAdress);
                    var ress = Query<ClassifieldModel>.EQ(pd => pd.Id, ObjectId.Parse(id));
                    var update = Update<ClassifieldModel>.Set(p => p.Viewers, c.Viewers);
                    _db.GetCollection<ClassifieldModel>("Classifields").Update(ress, update);
                }
            }
            catch { }

            //var res = Query<ClassifieldModel>.EQ(p => p.S_userId, userId);
            # endregion
            var res = Query<ApplicationUser>.EQ(p => p.Id, c.S_userId);
            var res2 = Query<UserDetails>.EQ(p => p.UserId, c.S_userId);
            var user = _db.GetCollection<ApplicationUser>("users").FindOne(res);
            var userDetails = _db.GetCollection<UserDetails>("UserDetails").FindOne(res2);
            var section = GetSection(ObjectId.Parse(c.SectionId));
            ClassifiedViewModel result = new ClassifiedViewModel() {
                Id = c.Id.ToString(),
                S_price = c.S_price,
                S_dateCreated = c.S_dateCreated,
                S_endDate = c.S_endDate,
                S_description = c.S_description,
                S_viewsCount = c.Viewers.Count,
                S_pictures = c.S_pictures ?? (new string[] { }),
                U_email = user.Email,
                U_number = user.PhoneNumber,
                U_FullName = userDetails.FullName,
                U_Image = userDetails.sm_image,
                S_Path = section.Path,
                U_Id = user.Id,
                Marks = c.Marks ?? new List<string>()
            };
            return result;
        }
        public ClassifiedViewModel GetClassifiedViewModel(string id)
        {
            ClassifieldModel c = GetClassifield(id);

            var res = Query<ApplicationUser>.EQ(p => p.Id, c.S_userId);
            var res2 = Query<UserDetails>.EQ(p => p.UserId, c.S_userId);
            var user = _db.GetCollection<ApplicationUser>("users").FindOne(res);
            var userDetails = _db.GetCollection<UserDetails>("UserDetails").FindOne(res2);
            var section = GetSection(ObjectId.Parse(c.SectionId));
            ClassifiedViewModel result = new ClassifiedViewModel()
            {
                Id = c.Id.ToString(),
                S_price = c.S_price,
                S_dateCreated = c.S_dateCreated,
                S_endDate = c.S_endDate,
                S_description = c.S_description,
                S_viewsCount = c.Viewers.Count,
                S_pictures = c.S_pictures == null ? new string[] { } : c.S_pictures,
                U_email = user.Email,
                U_number = user.PhoneNumber,
                U_FullName = userDetails.FullName,
                U_Image = userDetails.sm_image,
                S_Path = section.Path,
                U_Id = user.Id
            };
            return result;
        }
        public void UpdateClassifiedStatus(string id, Status status, int? weeks)
        {
            var res = Query<ClassifieldModel>.EQ(pd => pd.Id, ObjectId.Parse(id));
            if (status == Status.Public) {
                DateTime timeStart = DateTime.Now;
                DateTime timeEnd = timeStart.AddDays(Convert.ToDouble(weeks * 7));
                var update = Update<ClassifieldModel>.Set(p => p.Status, status)
                                .Set(p => p.S_dateCreated, timeStart).Set(p => p.S_endDate, timeEnd).Set(p => p.Approved, false);
                _db.GetCollection<ClassifieldModel>("Classifields").Update(res, update);
            } else {
                var update = Update<ClassifieldModel>.Set(p => p.Status, status);
                _db.GetCollection<ClassifieldModel>("Classifields").Update(res, update);
            }
        }
        public PClassifiedCountModel GetPublicedClassifiedsCount()
        {
            var res = Query<ClassifieldModel>.EQ(c => c.Status, Status.Public);
            var classifieds = _db.GetCollection<ClassifieldModel>("Classifields").Find(res);

            var response = new PClassifiedCountModel
            {
                NotApproved = classifieds.Where(c => c.Approved == false).Count(),
                Approved = classifieds.Where(c => c.Approved == true).Count(),
                Assigned = classifieds.Where(c => !String.IsNullOrWhiteSpace(c.OverWatch) && c.Approved == false).Count(),
                NotAssigned = classifieds.Where(c => String.IsNullOrWhiteSpace(c.OverWatch) && c.Approved == false).Count()
            };

            return response;
        }
        public List<CommentModel> GetClassifiedComments(string id) {
            var classified = GetClassifield(id);
            if (classified.Comments == null)
            {
                return new List<CommentModel>();
            }

            var comments = classified.Comments;
            var users = GetUsers();

            var result = from c in comments
                         join u in users on c.UserId equals u.ID
                         select new CommentModel
                         {
                             Id = c.Id,
                             Text = c.Text,
                             UserId = u.ID,
                             Date = c.Date,
                             Likes = c.Likes,
                             DisLikes = c.DisLikes,
                             UserName = GetUserDetails(u.ID).FullName,
                             UserPicture = GetUserDetails(u.ID).sm_image
                         };
            return result.OrderBy(d => d.Date).ToList();
        }
        public CommentModel AddClassifiedComment(CommentNew newC)
        {
            var classified = GetClassifield(newC.ClassifiedId);
            // Add comment Id
            if (classified.Comments == null)
            {
                classified.Comments = new List<Comment>();
            }
            var comment = new Comment
            {
                Text = newC.Text,
                UserId = newC.UserId,
                Date = DateTime.Now,
                Id = Guid.NewGuid().ToString()
            };

            classified.Comments.Add(comment);
            // Update DataBase
            var res = Query<ClassifieldModel>.EQ(pd => pd.Id, classified.Id);
            var update = Update<ClassifieldModel>.Set(p => p.Comments, classified.Comments);
            _db.GetCollection<ClassifieldModel>("Classifields").Update(res, update);

            // -------------- return Commentmodel
            var userDetails = GetUserDetails(comment.UserId);

            var commentModel = new CommentModel()
            {
                Id = comment.Id,
                Text = comment.Text,
                UserId = comment.UserId,
                Likes = new List<string>(),
                DisLikes = new List<string>(),
                Date = comment.Date,
                UserPicture = userDetails.sm_image,
                UserName = userDetails.FullName,
            };

            return commentModel;//Cmodel;
        }
        public CommentResponse AddClassifiedCommentLike(CommentLike newL)
        {
            var classified = GetClassifield(newL.ClassifiedId);

            var comment = classified.Comments.Where(c => c.Id == newL.CommentId).First();
            int index = classified.Comments.IndexOf(comment);
            if (comment.DisLikes == null) { comment.DisLikes = new List<string>(); }
            if (comment.Likes == null) { comment.Likes = new List<string>(); }

            var likeList = comment.DisLikes.Concat(comment.Likes);

            var check = likeList.FirstOrDefault(u => u == newL.UserId);

            if (check == null && newL.UserId != comment.UserId) {
                if (newL.Like)
                {
                    comment.Likes.Add(newL.UserId);
                }
                else
                {
                    comment.DisLikes.Add(newL.UserId);
                }
                classified.Comments[index] = comment;
                var res = Query<ClassifieldModel>.EQ(pd => pd.Id, classified.Id);
                var update = Update<ClassifieldModel>.Set(p => p.Comments, classified.Comments);
                _db.GetCollection<ClassifieldModel>("Classifields").Update(res, update);
            }
            return new CommentResponse
            {
                Likes = comment.Likes.ToList().Count(),
                Dislikes = comment.DisLikes.ToList().Count(),
                CommentId = comment.Id
            };

        }
        public int AssignClassifieds(string id, int count)
        {
            var res = Query<ClassifieldModel>.EQ(c => c.Status, Status.Public);
            var classifieds = _db.GetCollection<ClassifieldModel>("Classifields").Find(res).Where(c => c.Approved == false && String.IsNullOrWhiteSpace(c.OverWatch))
                .OrderBy(c => c.S_dateCreated).Take(count);
            var result = classifieds.Count();
            foreach (var c in classifieds) {
                var res2 = Query<ClassifieldModel>.EQ(pd => pd.Id, c.Id);
                var update = Update<ClassifieldModel>.Set(p => p.OverWatch, id);
                _db.GetCollection<ClassifieldModel>("Classifields").Update(res2, update);
            }
            return result;
        }
        public void ClassifiedApproveWorkItem(string Id) {
            var res = Query<ClassifieldModel>.EQ(pd => pd.Id, ObjectId.Parse(Id));
            var update = Update<ClassifieldModel>.Set(p => p.Approved, true);
            _db.GetCollection<ClassifieldModel>("Classifields").Update(res, update);
        }
        public void ClassifiedRejectWorkItem(string Id)
        {
            // Send Notification
            var classified = GetClassifield(Id);
            NoficationManager nm = new NoficationManager(UserCulture(classified.S_userId));
            AddNotification(nm.ClassifiedRejected(classified.S_userId));

            var res = Query<ClassifieldModel>.EQ(pd => pd.Id, ObjectId.Parse(Id));
            var update = Update<ClassifieldModel>.Set(p => p.Status, Status.Rejected);
            _db.GetCollection<ClassifieldModel>("Classifields").Update(res, update);
        }
        public int ClassifieldCountByPath(string path) {
            var classifieds = GetClassifieldAll().Where(c=> c.Status == Status.Public);
            var count = classifieds.Where(c => c.Path.Contains(path)).Count();

            return count;
        }
        public MarkModel MarkClassified(string Id, string userId) {
            var result = new MarkModel {
                Id = Id,
                Status = false
            };

            var classified = GetClassifield(Id);
            var marks = classified.Marks;
            if (marks == null) {
                marks = new List<string>();
            }
            if (!marks.Contains(userId))
            {
                marks.Add(userId);
                result.Status = true;
            }
            else
            {
                marks.Remove(userId);
                result.Status = false;
            }
            var res = Query<ClassifieldModel>.EQ(pd => pd.Id, ObjectId.Parse(Id));
            var update = Update<ClassifieldModel>.Set(p => p.Marks, marks);
            _db.GetCollection<ClassifieldModel>("Classifields").Update(res, update);

            return result;
        }
        public IEnumerable<PClassifiedModel> GetPublicedClassifieds()
        {
            var res = Query<ClassifieldModel>.EQ(c => c.Status, Status.Public);
            var classifieds = _db.GetCollection<ClassifieldModel>("Classifields").Find(res).Select(c=> new PClassifiedModel(){
                Id = c.IDS,
                Section = GetSection(ObjectId.Parse(c.SectionId)).Parent,
                UserEmail = GetUser(c.S_userId).Email,
                UserFullName = GetUserDetails(c.S_userId).FullName
            });
            return classifieds;
        }
        #endregion
        #region Users ------------------------------------------------------------------------------------------------------------------
        public List<UserModel> GetUsers()
        {
            var users = _db.GetCollection<UserModel>("users").FindAll();

            return users.ToList();
        }
        public UserModel GetUser(string id)
        {
            var res = Query<UserModel>.EQ(p => p.Id, ObjectId.Parse(id));
            var user = _db.GetCollection<UserModel>("users").FindOne(res);

            return user;
        }
        public IEnumerable<UserWorkInfo> GetWorkerInfo()
        {
            var res = Query<ClassifieldModel>.EQ(c => c.Status, Status.Public);
            var classifieds = _db.GetCollection<ClassifieldModel>("Classifields").Find(res).Where(c => c.Approved == false && !String.IsNullOrWhiteSpace(c.OverWatch));
            var group = classifieds.GroupBy(c => c.OverWatch, c => c.Id, (key, g) => new UserWorkInfo() { Id = key, FullName = GetUserDetails(key).FullName, NotApproved = g.Count() });

            return group;
        }
        public WorkItem GetWorkerItem(string id)
        {
            try
            {
                var re = Query<ClassifieldModel>.EQ(s => s.Status, Status.Public);
                var c = _db.GetCollection<ClassifieldModel>("Classifields").Find(re).Where(s => s.Approved == false && s.OverWatch == id).FirstOrDefault();
                var section = GetSection(ObjectId.Parse(c.SectionId));
                WorkItem result = new WorkItem()
                {
                    Classified = c,
                    Section = section
                };
                return result;
            }
            catch {
                return new WorkItem();
            }


        }
        public IEnumerable<UserBaseInfo> GetWorkerList()
        {
            var users = _db.GetCollection<UserModel>("users").FindAll().Where(u => u.Roles.Contains("Support") || u.Roles.Contains("Admin")).Select(u => new UserBaseInfo
            {
                Id = u.ID,
                FullName = GetUserDetails(u.ID).FullName
            }
            );
            return users;
        }
        public UserDetails GetUserDetails(string id)
        {
            var res = Query<UserDetails>.EQ(p => p.UserId, id);
            var details = _db.GetCollection<UserDetails>("UserDetails").FindOne(res);

            return details;
        }
        public UserEditViewModel GetUserDetailsFull(string id)
        {
            var user = GetUser(id);
            var details = GetUserDetails(id);
            var result = new UserEditViewModel();
            result.UserId = user.ID;
            result.Email = user.Email;
            result.PhoneNumber = user.PhoneNumber;
            if (details != null)
            {
                result.FullName = details.FullName;
                result.City = details.City;
                result.Skype = details.Skype;
                result.sm_image = details.sm_image;
                result.lg_image = details.lg_image;
                result.DOB = details.DOB;
                result.WebAddress = details.WebAddress;
                result.Gender = details.Gender;
            }


            return result;
        }
        public void UpdateUserDetails(string id, string picture, UserDetails u)
        {
            var dbUser = GetUserDetails(id);

            if (dbUser == null)
            {
                u.UserId = id;
                _db.GetCollection<UserDetails>("UserDetails").Insert(u);
            }
            else
            {
                dbUser.Skype = u.Skype;
                dbUser.WebAddress = u.WebAddress;
                dbUser.FullName = u.FullName;
                dbUser.City = u.City;
                dbUser.DOB = u.DOB;
                dbUser.Gender = u.Gender;
                if (!string.IsNullOrEmpty(picture))
                {
                    dbUser.lg_image = picture;
                    dbUser.sm_image = picture;
                }
                var res = Query<UserDetails>.EQ(p => p.UserId, id);
                var operation = Update<UserDetails>.Replace(dbUser);
                _db.GetCollection<UserDetails>("UserDetails").Update(res, operation);
            }
        }
        public IEnumerable<ClassifieldModel> GetPublicedClassifiedsForUser(string id)
        {
            var res = Query<ClassifieldModel>.EQ(c => c.S_userId, id);
            var classifieds = _db.GetCollection<ClassifieldModel>("Classifields").Find(res).Where(c => c.Status == Status.Public);

            return classifieds;
        }
        public string UserCulture(string userId) {
            var user = GetUserDetails(userId);
            return user.NLn ?? "en";
        }
        #endregion
        #region Chat -------------------------------------------------------------------------------------------------------------------------
        public List<ChatMessageModel> GetHistoryMessages(string userId, string toUserId) {
            var messages = _db.GetCollection<ChatMessageModel>("chatMessages").FindAll()
                .Where(i => (i.UserId == userId && i.ToUserId == toUserId) || (i.UserId == toUserId && i.ToUserId == userId)).OrderBy(i => i.Created).ToList();

            return messages;
        }
        public IEnumerable<UserModel> GetHistoryUsers(string userId)
        {
            var messages = _db.GetCollection<ChatMessageModel>("chatMessages").FindAll()
                .Where(i => i.UserId == userId || i.ToUserId == userId).OrderBy(i => i.Created).ToList();

            var userIdList = messages.Select(m => userId == m.UserId ? m.ToUserId : m.UserId).Distinct();

            var users = GetUsers().Where(u => u.ID != userId && userIdList.Contains(u.ID)).AsEnumerable();

            return users;
        }
        public void AddHistoryMessages(ChatMessageModel m)
        {
            _db.GetCollection<ChatMessageModel>("chatMessages").Save(m);
        }
        #endregion
        #region StaticData -----------------------------------------------------------------------------------------------------------
        public StaticData GetStaticData()
        {
            return _db.GetCollection<StaticData>("StaticData").FindOne();
        }
        #endregion 
        #region Notifications -------------------------------------------------------------------------------------------------------
        public IEnumerable<NotificationModel> GetNotifications(string Userid)
        {
            var res = Query<NotificationModel>.EQ(p => p.UserId, Userid);
            var notifications = _db.GetCollection<NotificationModel>("Notifications").Find(res);
            return notifications.OrderBy(n=>n.DateTime);
        }
        public void AddNotification(NotificationModel notification)
        {
            _db.GetCollection<NotificationModel>("Notifications").Save(notification);
        }
        public bool NotificationUpdateStatus(string id)
        {

            var ress = Query<NotificationModel>.EQ(pd => pd.Id, ObjectId.Parse(id));
            var update = Update<NotificationModel>.Set(p => p.Active, false);
            _db.GetCollection<NotificationModel>("Notifications").Update(ress, update);
            return false;
        }
        #endregion
        #region Reports --------------------------------------------------------------------------------------------------------------------
        public IEnumerable<ReportGroupModel> ReportsGet() {
            var reports = _db.GetCollection<ReportModel>("Reports").FindAll();
            var reportGroup = reports.GroupBy(r => r.ClassifiedId, (key, g) => new ReportGroupModel { Id = key.ToString(), items = g.ToList() });
            return reportGroup;
        }
        public void ReportInsert(string Id,string title, string desc)
        {
            var report = new ReportModel()
            {
                ClassifiedId = Id,
                Description = desc,
                Active = true,
                Title = title,
                DateTime = DateTime.Now
            };
            _db.GetCollection<ReportModel>("Reports").Insert(report);
        }
        public void ReportSetActive(string id)
        {
            var res = Query<ReportModel>.EQ(c => c.ClassifiedId, id);
            var reports = _db.GetCollection<ReportModel>("Reports").Find(res);
            foreach (var r in reports) {
                var update = Update<ReportModel>.Set(u =>u.Active, false);
                var query = Query<ReportModel>.EQ(c => c.Id, r.Id);
                _db.GetCollection<ReportModel>("Reports").Update(query, update);
            }  
        }
        #endregion
    }
}