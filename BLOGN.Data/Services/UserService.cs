using BLOGN.Data.Repositories.IRepository;
using BLOGN.Data.Services.IService;
using BLOGN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLOGN.Data.Services
{
    public class UserService :  IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;
        public UserService(IUnitOfWork unitOfWork, IUserRepository repository) 
        {
            _unitOfWork = unitOfWork;
            _userRepository = repository;
        }

        public User Authenticate(string username, string password)
        {
            var user = _userRepository.Authenticate(username, password);
            return user;
        }

        public bool IsUniqueUser(string username)
        {
           return _userRepository.IsUniqueUser(username);
        }

        public User Register(string username, string password)
        {
            var user = _userRepository.Register(username, password);
            _unitOfWork.Save();
            return user;
        }
    }
}
