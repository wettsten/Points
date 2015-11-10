using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Points.Data;

namespace Points.Api2.Models
{
    public interface IAuthRepository
    {
        Task<IdentityResult> RegisterUser(User userModel);
        Task<User> FindUser(string userName, string password);
    }
}