using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
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
        public ClassifiedViewModel GetClassifiedViewModel(string id) {
            ClassifieldModel c = GetClassifield(id);

            //var res = Query<ClassifieldModel>.EQ(p => p.S_userId, userId);
            var res = Query<ApplicationUser>.EQ(p => p.Id, c.S_userId);
            var user = _db.GetCollection<ApplicationUser>("users").FindOne(res);

            ClassifiedViewModel result = new ClassifiedViewModel() {
                S_price = c.S_price,
                S_dateCreated = c.S_dateCreated,
                S_description = c.S_description,
                U_email = user.Email,
                U_number = user.PhoneNumber,
                U_location = user.UserName
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
        #endregion
        #region Users
        public List<UserModel> GetUsers()
        {
            var users = _db.GetCollection<UserModel>("users").FindAll();

            return users.ToList();
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
    }
}