using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTS.Domain.Entities.Identity;

public class SystemConfig
{
    public virtual int ID { get; set; }
    public virtual int ComID { get; set; }
    public virtual string Code { get; set; }
    public virtual string Value { get; set; }
}
