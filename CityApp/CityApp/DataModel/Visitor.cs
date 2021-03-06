﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityApp.DataModel
{
    public class Visitor : User
    {
        public ICollection<Company> Subscriptions { get; set; }
        public Visitor(string name, string firstName, DateTime birthDate, String email, String password, UserType userType, List<Company> subscriptions) : base(name, firstName, birthDate, email, password, userType)
        {
            Subscriptions = subscriptions;
        }

        public Visitor():base()
        {
            Subscriptions = new List<Company>();
        }
    }
}
