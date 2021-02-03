using System;
using System.Collections.Generic;
using System.Text;

namespace ShippingApp.Domain.Models
{
    public class ContentEmailModel: CommonEmailModel
    {
        public string MsgDearText { get; set; }
        public string MsgConcernText { get; set; }
        public string MsgPhoneContactText { get; set; }
        public string MsgEmailContactText { get; set; }
        public string MsgRemark { get; set; }
        public string MsgRegards { get; set; }
        public string MsgSignature { get; set; }

    }
}
