using System;
using System.Collections.Generic;
using System.Text;

namespace Futuristic.Models
{
    public class LineUp
    {
        public double LocationLatitude { get; set; }
        public double LocationLongtitude { get; set; }
        public string LineType { get; set; }
        public Guid StoreId { get; set; }
        public int LineCount { get; set; }
        public DateTime Time { get; set; }
        public Guid ApplicationId { get; set; }
        public LineUp()
        {
            init();
        }
        private void init()
        {

        }
    }
}