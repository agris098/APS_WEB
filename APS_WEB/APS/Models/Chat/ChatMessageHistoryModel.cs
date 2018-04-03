using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    public class ChatMessageHistoryModel
    {
        public string Id { get; set; }
        public List<ChatMessageModel> Messages { get; set; }
    }
}