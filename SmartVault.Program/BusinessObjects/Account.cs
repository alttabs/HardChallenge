using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartVault.Program.BusinessObjects
{
    public partial class Account
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
