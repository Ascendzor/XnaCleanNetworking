using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class Event
    {
        public Guid id;
        public int type;
        public object value;

        public Event(Guid id, int type, object value)
        {
            this.id = id;
            this.type = type;
            this.value = value;
        }
    }
}
