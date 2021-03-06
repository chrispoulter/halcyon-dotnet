﻿namespace Halcyon.Web.Models.User
{
    public class ListUsersModel
    {
        public string Search { get; set; }

        public UserSort? Sort { get; set; }

        public int? Page { get; set; }

        public int? Size { get; set; }
    }
}
