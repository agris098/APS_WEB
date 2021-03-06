﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APS.Models
{
    public class MyClassifiedsModel
    {
        public string Id { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Status Status { get; set; }
        public List<string> Marks { get; set; }
    }
}