using System;

namespace HelloWorld.ViewModel
{
    public class ResponseStatsViewModel
    {
        public int ResponseId { get; set; }

        public string Message { get; set; }

        public string IpAddress { get; set; }

        public string ClientDetails { get; set; }

        private DateTime responseDateTime = DateTime.Now;
        public DateTime ResponseDateTime
        {
            get
            {
                return this.responseDateTime;
            }
        }

        public override string ToString()
        {
            return string.Format("ResponseId:{0}, ResponseDateTime:{1}, Message:{2}, IpAddress:{3}, ClientDetails:{4}",
                this.ResponseId, this.ResponseDateTime, this.Message, this.IpAddress, this.ClientDetails);
        }
    }
}