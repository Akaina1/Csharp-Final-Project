namespace FinalProject
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public enum AdminLevel { Admin, Manager, Employee }
        public AdminLevel adminLevel { get; set; }
    }
}
