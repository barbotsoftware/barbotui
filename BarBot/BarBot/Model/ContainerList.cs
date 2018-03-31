using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.Core.Model
{
    class ContainerList : JsonModelObject
    {
        List<Container> containers;

        public List<Container> Containers
        {
            get
            {
                return containers;
            }
            set
            {
                containers = value;
            }
        }

        public ContainerList() { }

        public ContainerList(String json)
        {
            Containers = (List<Container>)parseJSON(json, typeof(List<Container>));
        }
    }
}
