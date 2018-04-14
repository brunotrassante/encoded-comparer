using System;
using System.Collections.Generic;
using System.Text;

namespace EncodedComparer.Domain.Queries
{
    public class LeftRightSameIdQuery
    {
        public int Id { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }
    }
}
