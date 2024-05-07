using OnlineShoppingApp.ViewModels;

namespace OnlineShoppingApp.Repositories.Interfaces
{
    public interface IBuyerRepo
    {
        public UpdateProfileViewModel GetProfileData(int buyerId);
        public  bool UpdateProfile(UpdateProfileViewModel oldData, int buyerId);
    }
}
