﻿namespace EpicAlbergo.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<string> Roles { get; set; }
    }
}
