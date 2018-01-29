using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ClientAddresses
    {
        public int? Id { get; set; }
        public string Owner { get; set; }

        public string Type { get; set; }

        public string Region { get; set; }

        public string Settlement { get; set; }

        public string Street { get; set; }

        public string House { get; set; }

        public string Flat { get; set; }

        public string Correct { get; set; }
    }
}
