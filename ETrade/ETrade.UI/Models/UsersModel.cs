using ETrade.Entity.Concretes;

namespace ETrade.UI.Models
{
    public class UsersModel
    {
        public Users Users { get; set; }
        public List<County> Counties { get; set; }
        public string Error { get; set; }
    }
}
