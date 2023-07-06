﻿using BusinessObjects;
using DataAccessObjects;
using System;
using System.Collections.Generic;

namespace BussinessObjects
{
    public class UserRepository : IUserRepository
    {
        public List<User> GetAll() => UserDAO.Instance.GetAll();

        public User Get(string usernameOrEmail) => UserDAO.Instance.Get(usernameOrEmail);

        public bool Exist(string usernameOrEmail) => UserDAO.Instance.Exist(usernameOrEmail);

        public void Create(User entity) => UserDAO.Instance.Create(entity);

        public void Update(User entity) => UserDAO.Instance.Update(entity);

        public void Save(User entity) => UserDAO.Instance.Save(entity);

        public void Delete(string usernameOrEmail) => UserDAO.Instance.Delete(usernameOrEmail);

        public User Authenticate(string usernameOrEmail, string password)
        {
            User user = Get(usernameOrEmail);
            if (user == null || user.Password.Equals(password) == false)
                return null;
            return user;
        }
    }
}