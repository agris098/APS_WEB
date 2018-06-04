using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APS.Models
{
    [BsonIgnoreExtraElements]
    public class ClassifieldModel
    {
        public ObjectId Id { get; set; }
        [BsonElement("SectionId")]
        public string SectionId { get; set; }
        [BsonElement("S_userId")]
        public string S_userId { get; set; }

        [Required]
        [Display(Name = "Price")]
        [BsonElement("S_price")]
        public decimal S_price { get; set; }
        [BsonElement("S_dateCreated")]
        public DateTime S_dateCreated { get; set; }
        [Display(Name = "Pictures")]
        [BsonElement("S_pictures")]
        public string[] S_pictures { get; set; }

        [BsonElement("S_mpicture")]
        public string S_mpicture { get; set; }

        [Required]
        [Display(Name = "Year")]
        [BsonElement("S_year")]
        public int S_year { get; set; }

        [Required]
        [Display(Name = "Description")]
        [BsonElement("S_description")]
        public string S_description { get; set; }

        [Required]
        [Display(Name = "Condition")]
        [BsonElement("S_condition")]
        public string S_condition { get; set; }
        [BsonElement("S_endDate")]
        public DateTime S_endDate { get; set; }
        [BsonElement("Deleted")]
        public bool? Deleted { get; set; }
        [BsonElement("Status")]
        public Status Status { get; set; }
        [BsonElement("Approved")]
        public bool Approved { get; set; } 
        [BsonElement("OverWatch")]
        public string OverWatch { get; set; }
        [BsonElement("Viewers")]
        public List<string> Viewers { get; set; }
        [BsonElement("Comments")]
        public List<Comment> Comments { get; set; }
        [BsonElement("Marks")]
        public List<string> Marks { get; set; }

        [Display(Name = "Video")]
        [BsonElement("S_video")]
        public string S_video { get; set; }
        

        public string Path { get; set; }
        public string IDS{ get {return Id.ToString(); } }
    }
    public class CommentNew
    {
        public string ClassifiedId { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
    }
    public class Comment
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public List<string> Likes { get; set; }
        public List<string> DisLikes { get; set; }
    }
    public class CommentModel {
        public string Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public List<string> Likes { get; set; }
        public List<string> DisLikes { get; set; }
        public string UserPicture { get; set; }
        public string DateTime { get { return Date.ToString("dd/MM/yyyy HH:mm:ss"); } }
    }
    public class CommentLikeModel
    {
        public string ClassifiedId { get; set; }
        public string CommentId { get; set; }
        public string UserId { get; set; }
        public bool Like { get; set; }

    }
    public class CommentLike {
        public string ClassifiedId { get; set; } 
        public string CommentId { get; set; }
        public string UserId { get; set; }
        public bool Like { get; set; }
    }
    public class CommentResponse {
        public string CommentId { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
    }
}