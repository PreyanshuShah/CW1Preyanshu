using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CW1Preyanshu.Models;
using CW1Preyanshu.Models;

namespace CW1Preyanshu.Components.Model
{
    public class AppData
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Transactions> Transactions { get; set; } = new List<Transactions>();
        public List<Debts> Debts { get; set; } = new List<Debts>();
    }
}