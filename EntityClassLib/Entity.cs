using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityClassLib;

/// <summary>
/// 实体主要生成类, 用于生成实体类
/// </summary>
public class LongEntity : EntityBase<long>
{

}

public interface IEntity
{
    int Version { get; set; }
    long CreatedAt { get; set; }
    long UpdatedAt { get; set; }
    long DeletedAt { get; set; }
    bool IsDeleted { get; set; }
    void UpdateFileTimeStamp();
    void DeleteEntity();
    void UnDelete();
}
public class EntityBase : IEntity
{
    public int Version { get; set; }
    public long CreatedAt { get; set; }
    public long UpdatedAt { get; set; }
    public long DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    public EntityBase()
    {
        CreatedAt = GetEpochTime();
        UpdatedAt = CreatedAt;
    }
    public static long GetEpochTime() => SysTime.EpochSecondsNow();

    /// <summary>
    /// 用于数据更新调用. 更新时间戳和版本号
    /// </summary>
    public void UpdateFileTimeStamp()
    {
        UpdatedAt = GetEpochTime();
        Version++;
    }

    /// <summary>
    /// 软删除调用. 更新删除时间戳和版本号
    /// </summary>
    public void DeleteEntity()
    {
        IsDeleted = true;
        DeletedAt = GetEpochTime();
    }

    /// <summary>
    /// 取消软删除调用. 更新删除时间戳和版本号
    /// </summary>
    public void UnDelete()
    {
        IsDeleted = false;
        UpdatedAt = GetEpochTime();
        UpdateFileTimeStamp();
    }

    public static void EntityUpdateBeforeSaveChange(ChangeTracker changeTracker)
    {
        // 获取所有已更改的实体
        var modifiedEntries = changeTracker.Entries()
            .Where(entry => entry.State == EntityState.Modified);

        foreach (var entry in modifiedEntries)
        {
            if (entry.Entity is IEntity entityBase)
            {
                // 调用UpdateFileTimeStamp()方法
                entityBase.UpdateFileTimeStamp();
            }
        }
    }

}

/// <summary>
/// 实体父类, 主要实现:<br/>
/// 1. 软删除 ,<br/>
/// 2. 更新,创建 时间戳, - 用于基本的记录信息<br/>
/// 3. 版本号 - 用于记录当前数据的纪录量和版本对比(乐观锁)<br/>
/// 在调用构造函数的时候会有基本的数据记录
/// </summary>
/// <typeparam name="TId"></typeparam>
public class EntityBase<TId> : EntityBase where TId : IConvertible
{
    [Key]
    public virtual TId Id { get; set; } = default!;
}

public class GuidEntity : EntityBase<string>
{
    public override string Id { get; set; } = Guid.NewGuid().ToString();
}

public class IntEntity : EntityBase<int>
{
}