using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Company.Welcome.Business.GuestVisitor;
using Company.Welcome.Commons;
using Company.Welcome.Entities.GuestVisitor;
using Company.Welcome.Ral.GuestVisitor;

namespace Company.Welcome.Core.Bootstrap
{
    public class BootstrapBusiness
    {
        public static void Configure(UnityContainer container)
        {
            container.RegisterType<ITekGuestVisitorBusinessService, TekGuestVisitorBusinessService>(
                new InjectionConstructor(
                    new ResolvedParameter<ITekGuestVisitorRepository<VisitorEntity>>(),
                    new ResolvedParameter<IEncodingHelper>(),
                    new ResolvedParameter<IProvideDateTime>()
                    ));
        }
    }
}
