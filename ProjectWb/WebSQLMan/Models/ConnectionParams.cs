using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebSQLMan.Models
{
    public class ConnectionParams
    {
        [Required(ErrorMessage = "The field must be field")]
        public string ServerName { get; set; }
        [Required]
        public string DataBase { get; set; }
        [Required]
        public string Authentication { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}