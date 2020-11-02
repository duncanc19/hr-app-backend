using System;
using HRApp.API.Models;
using System.Collections.Generic;
public class UserList
{
    public List<User> users { get; set; } = new List<User>();
    public UserList() 
    {
        var duncan = new Login { Username = "Duncan", Password="abc" };
        var azlina = new Login { Username = "Azlina", Password="def" };
        var joanna = new Login { Username = "Joanna", Password="123" };

        var duncanInfo = new UserInfo { Id = new Guid("c0a68046-617e-4927-bd9b-c14ce8f497e1"), 
                                        FirstName = "Duncan", LastName = "Carter", Role = "Employee", Permission = "Default"};
        var azlinaInfo = new UserInfo { Id = new Guid("18712a4f-744e-4e7c-a191-395fa832518b"), 
                                        FirstName = "Azlina", LastName = "Yeo", Role = "Employee", Permission = "Default"};
        
        var joannaInfo = new UserInfo { Id = new Guid("6d56e5bd-bba7-4026-9e3d-383f2c2f8d4d"), 
                                        FirstName = "Joanna", LastName = "Fawl", Role = "Employee", Permission = "Default"};

        users.Add(new User(duncan, duncanInfo));
        users.Add(new User(azlina, azlinaInfo));
        users.Add(new User(joanna, joannaInfo));
    }
}