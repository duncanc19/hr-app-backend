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

        var duncanInfo = new UserInfo { FirstName = "Duncan", Surname = "Carter", Role = "Employee", PermissionLevel = "Default",
                        Telephone = "0771333333333", Email = "duncan@dc.com", Location = "Paris", NextOfKin = "Mother", Address = "Champs Elysee",
                        Salary = "£56000", DoB = new DateTime(1912,01,01)};
        var azlinaInfo = new UserInfo { FirstName = "Azlina", Surname = "Yeo", Role = "Employee", PermissionLevel = "Default",
                        Telephone = "0771333546433", Email = "azlina@happy.com", Location = "Singapore", NextOfKin = "Father", Address = "Bedok Reservoir Road",
                        Salary = "£29000", DoB = new DateTime(1979,01,01)};
        
        var joannaInfo = new UserInfo { FirstName = "Joanna", Surname = "Fawl", Role = "Employee", PermissionLevel = "Default",
                        Telephone = "07713344333", Email = "joanna@jf.com", Location = "Seattle", NextOfKin = "Brother", Address = "The Tower",
                        Salary = "£75000", DoB = new DateTime(1917,05,08) };

        users.Add(new User(duncan, duncanInfo, new Guid("c0a68046-617e-4927-bd9b-c14ce8f497e1")));
        users.Add(new User(azlina, azlinaInfo, new Guid("18712a4f-744e-4e7c-a191-395fa832518b")));
        users.Add(new User(joanna, joannaInfo, new Guid("6d56e5bd-bba7-4026-9e3d-383f2c2f8d4d")));
    }
}