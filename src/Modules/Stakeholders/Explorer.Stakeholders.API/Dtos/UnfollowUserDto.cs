using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class UnfollowUserDto
    {
        public string userId { get; set; }
        public string userToUnfollowId { get; set; }
    }
}
