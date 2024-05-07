using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;

namespace OnlineShoppingApp.Repositories.Classes
{
    public class CommentsRepo : ICommentsRepo
    {
        private readonly ShoppingContext context;

        public CommentsRepo(ShoppingContext _context)
        {
            context = _context;
        }
        public void AddComment(Comment comment)
        {

            context.Comments.Add(comment);
            context.SaveChanges();
        }

        public void DeleteComment(int id)
        {
            Comment comment = GetComment(id);

            context.Comments.Remove(comment);
            context.SaveChanges();

        }

        public List<Comment> GetAllComments(int ProdID)
        {
           // var comments = context.Comments.Include(c => c.Product).Include(c => c.AppUser).ToList();
          
           return context.Comments.Include(c=>c.Product).Include(c=>c.AppUser).Include(c=>c.Replies).Where(c=>c.ProductId==ProdID).ToList();
        }

       

        //public AppUser GetAppUser(int UID)
        //{
        //    return context.Users.FirstOrDefault(u => u.Id == UID);
        //}

        public Comment GetComment(int id)
        {
            return context.Comments.Include(c => c.Product).Include(c => c.AppUser).Include(c=>c.Replies).FirstOrDefault(c => c.Id == id);
        }

        public List<Comment> GetCommentsPerUser(int UID, int ProdID)
        {
           return context.Comments.Include(c => c.Replies).Where(c=>c.AppUserId == UID && c.ProductId==ProdID).ToList();
        }

        public void UpdateComment(Comment newComm)
        {
            Comment oldComment = GetComment(newComm.Id);

            oldComment.AppUserId = newComm.AppUserId;
            oldComment.Text = newComm.Text;
            oldComment.Date = newComm.Date;
            oldComment.ProductId = newComm.ProductId;

            context.SaveChanges();
        }
    }
}
