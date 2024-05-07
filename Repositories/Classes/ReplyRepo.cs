using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;
using Org.BouncyCastle.Tls;

namespace OnlineShoppingApp.Repositories.Classes
{
    public class ReplyRepo : IReplyRepo
    {
        ShoppingContext shoppingContext;

        public ReplyRepo(ShoppingContext _shoppingContext)
        {
            shoppingContext = _shoppingContext; 
        }
        public void AddReply(Replies reply)
        {
            shoppingContext.Replies.Add(reply);
            shoppingContext.SaveChanges();  
        }

        public List<Replies> GetAll()
        { 
            return shoppingContext.Replies.Include(r=>r.AppUser).Include(r=>r.comment).ToList();    
        }

        public Replies GetById(int id)
        {
            return shoppingContext.Replies.Include(r => r.AppUser).Include(r => r.comment).FirstOrDefault(r => r.Id == id);
        }
    }
}
