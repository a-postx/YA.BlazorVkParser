namespace YA.WebClient.Application.Models.SaveModels;

/// <summary>
/// Арендатор, модель сохранения.
/// </summary>
public class TenantSm : ValueObject
{
    public TenantSm(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Название арендатора.
    /// </summary>
    public string Name { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Name;
    }
}
