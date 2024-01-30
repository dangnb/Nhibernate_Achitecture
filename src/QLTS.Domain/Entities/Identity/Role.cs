namespace QLTS.Domain.Entities.Identity;

public class Role
{
    public virtual int RoleID { get; set; }
    public virtual int ComID { get; set; }
    public virtual string Code { get; set; }
    public virtual string Name { get; set; }
    public virtual bool IsSysadmin { get; set; } = false;
    public virtual IList<Permission> Permissions { get; set; } = new List<Permission>();
}

public class RoleData
{
    public RoleData()
    {

    }
    public string Code { get; set; }
    public bool IsSysadmin { get; set; } = false;
    public List<string> Permissions { get; set; } = new List<string>();
}
