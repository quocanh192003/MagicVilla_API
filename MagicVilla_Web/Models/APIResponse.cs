﻿using System.Net;

namespace MagicVilla_Web.Models
{
    public class APIResponse
    {
        public HttpStatusCode Status { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public Object Result { get; set; }
    }
}
