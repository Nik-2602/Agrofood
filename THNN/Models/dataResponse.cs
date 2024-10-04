using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THNN.Models
{
    public class dataResponse
    {
        public int amount { get; set; }
        public string orderInfo { get; set; }

        public dataResponse() { }

        public dataResponse(int Amount , string OrderInfo) {
            this.amount = Amount;
            this.orderInfo = OrderInfo;
        }
    }
}