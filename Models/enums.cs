using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyPay.Models
{
    public enum PropsType
    {
        Text=1,
        Bool,
        Number
    }
    public enum CompanyType
    {
        My=0,
        Bbuyer,
        Shipper
    }
    public enum sobType
    {
        ООО = 0,
        ОАО = 1,
        ИП = 2,
        ЗАО = 3,
        ГБОУ = 4, ГБОУ_ДОД = 5
    }
    public enum NdsValue
    {
        Нет = 0,
        NDS10 = 10,
        NDS18 = 18
    }
    public enum DocumentType
    {
        Out = 2,
        In = 1
    }
}
