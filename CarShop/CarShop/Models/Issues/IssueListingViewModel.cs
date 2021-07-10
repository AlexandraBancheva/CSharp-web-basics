using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Models.Issues
{
    public class IssueListingViewModel
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public bool IsFixed { get; set; }
    }
}
