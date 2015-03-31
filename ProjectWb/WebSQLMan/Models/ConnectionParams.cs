using System.ComponentModel.DataAnnotations;

namespace WebSQLMan.Models
{
    public class ConnectionParams
    {
        [Required(ErrorMessage = "Укажите имя сервера")]
        public string ServerName { get; set; }
        [Required(ErrorMessage = "Укажите базу данных")]
        public string DataBase { get; set; }
        [Required(ErrorMessage = "Выберите способ аутентификации")]
        public Auth Authentification { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public enum Auth
        {
            SqlAuthentication,
            WindowsAuthentication
        }
    }
}
