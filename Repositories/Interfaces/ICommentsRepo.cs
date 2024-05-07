using OnlineShoppingApp.Models;

namespace OnlineShoppingApp.Repositories.Interfaces
{
    public interface ICommentsRepo
    {
        public List<Comment> GetAllComments(int ProdID);

        public Comment GetComment(int id);

        public List<Comment> GetCommentsPerUser(int UID, int ProdID);

        public void AddComment(Comment comment);

        public void UpdateComment(Comment comment);

        //public AppUser GetAppUser(int UID);

        public void DeleteComment(int id);
    }
}
