using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class NewFollowingDto
    {
        public string userId { get; set; }
        public string username { get; set; }
        public string profileImage { get; set; }
        public string followingUserId { get; set; }
        public string followingUsername { get; set; }
        public string followingProfileImage { get; set; }
    }
}
