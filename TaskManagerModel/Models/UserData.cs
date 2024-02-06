using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtTaskWebServer.Model
{
    public class UserData
    {
        public Guid SessionId { get; set; }
        public string UserName { get; set; }
        public Role Role { get; set; }
        public UserData(string userName, Guid sessionId, Role role)
        {
            UserName=userName;
            SessionId=sessionId;
            Role=role;
        }

        public override string ToString()
        {
            return SessionId.ToString()+" "+UserName+" "+Role.ToString();
        }
    }
}
