using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AskFm.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string message { get; set; }
        public NotUrlType UrlType { get; set; }
        public Guid AnswerId { get; set; }//optional
        public string TriggerUserName { get; set; }//optional   ---> in fact it should hold the username not id

        //nav props
        public string UserId { get; set; }
        virtual public ApplicationUser User { get; set; }
    }

    public enum NotUrlType 
    {
        /*notification redirect type: determines the link should be returned when the not. is clicked*/
        ToAnswer,
        ToProfile
    }

}