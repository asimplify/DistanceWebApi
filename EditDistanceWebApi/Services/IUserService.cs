using System;
using System.Collections.Generic;
using System.Linq;
using DistanceWebApi.Models;
using DistanceWebApi.Helpers;
using System.Threading.Tasks;

namespace DistanceWebApi.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
       Task<User> GetById(int id);
        User Create(User user, string password);
    }
}