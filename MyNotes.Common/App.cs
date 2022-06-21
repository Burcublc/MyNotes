using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.Common
{
    public static class App
    {
        //STATİC BİR SINIFIN METODLARIDA STATIC OLMALIDIR
        public static ICommon Common=new DefaultCommon();
    }
}
