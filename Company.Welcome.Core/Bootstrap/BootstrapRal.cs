using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Company.Welcome.Commons;
using Company.Welcome.Entities.GuestVisitor;
using Company.Welcome.Ral.GuestVisitor;

namespace Company.Welcome.Core.Bootstrap
{
    public class BootstrapRal
    {
        public static void Configure(UnityContainer container)
        {
            container.RegisterType<ITekGuestVisitorRepository<VisitorEntity>, TekGuestVisitorRepository>();
        }
    }
}
