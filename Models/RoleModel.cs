namespace WebApp.MemesMVC.Models
{
    public enum RoleTypes
    {
        USER,
        MODERATOR,
        ADMIN
    }

    public class RoleModel
    {
        public int Id { get; set; }
        
        public string RoleName { get; set; }
    }
}
