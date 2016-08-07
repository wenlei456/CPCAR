using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOP
{
    //[Aop]
    public class BaseBusiness//:ContextBoundObject
    {
        //public static CarPartsEntities db = null;
         public  CarPartsEntities db = new CarPartsEntities();

    }
}
