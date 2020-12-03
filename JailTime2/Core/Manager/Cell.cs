using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JailTime2.Core.Manager
{
    [Serializable]
    public class Cell
    {
        public int Id { get; set; }
        public Vector3SE Position { get; set; }

        public Cell()
        {

        }
        public Cell(int id, Vector3SE position)
        {
            Id = id;
            Position = position;
        }
        
    }
}
