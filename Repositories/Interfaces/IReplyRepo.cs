using OnlineShoppingApp.Models;

namespace OnlineShoppingApp.Repositories.Interfaces
{
    public interface IReplyRepo
    {
        public List<Replies> GetAll();

        public Replies GetById(int id);

        public void AddReply(Replies reply);
    }
}
