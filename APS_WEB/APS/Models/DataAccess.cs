using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace APS.Models
{
    public class DataAccess
    {
        public MongoClient _client;
        public MongoServer _server;
        public MongoDatabase _db;

        public DataAccess()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _server = _client.GetServer();
            _db = _server.GetDatabase("APS");
        }
#region Sections
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
#region Classifieds
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
                S_description = c.S_description,
                S_viewsCount = c.Viewers.Count,
                S_pictures = c.S_pictures,
                U_email = user.Email,
                U_number = user.PhoneNumber,
                U_FullName = userDetails.FullName,
                U_Image = userDetails.sm_image,
                S_Path = section.Path,
                U_Id = user.Id

            };
            return result;
        }
        public void UpdateClassifiedStatus(string id, Status status)
        {
            var res = Query<ClassifieldModel>.EQ(pd => pd.Id, ObjectId.Parse(id));
            var update = Update<ClassifieldModel>.Set(p => p.Status, status);
            _db.GetCollection<ClassifieldModel>("Classifields").Update(res, update);
        }
        public PClassifiedGroupModel GetPublicedClassifieds()
        {
            var res = Query<ClassifieldModel>.EQ(c => c.Status, Status.Public);
            var classifieds = _db.GetCollection<ClassifieldModel>("Classifields").Find(res);

            var response = new PClassifiedGroupModel
            {
                NotApproved = classifieds,//.Where(c => c.Approved == false),
                Approved = classifieds//.Where(c => c.Approved == true)
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

            var comment = classified.Comments.Where(c=>c.Id == newL.CommentId).First();
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
            #endregion
#region Users
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
        #endregion
        #region Chat
        public List<ChatMessageModel> GetHistoryMessages(string userId, string toUserId) {
            var messages = _db.GetCollection<ChatMessageModel>("chatMessages").FindAll()
                .Where(i=> (i.UserId == userId && i.ToUserId == toUserId) || (i.UserId == toUserId && i.ToUserId == userId)).OrderBy(i => i.Created).ToList();

            return messages;
        }
        public void AddHistoryMessages(ChatMessageModel m)
        {
            _db.GetCollection<ChatMessageModel>("chatMessages").Save(m);
        }
        #endregion
#region StaticData
        public StaticData GetStaticData()
        {
            return _db.GetCollection<StaticData>("StaticData").FindOne();
        }
#endregion
#region Notifications
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
    }
}