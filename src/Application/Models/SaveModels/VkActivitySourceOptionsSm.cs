namespace YA.WebClient.Application.Models.SaveModels;

/// <summary>
/// Настройки источников активностей.
/// </summary>
public class VkActivitySourceOptionsSm : ValueObject
{
    private VkActivitySourceOptionsSm() { }

    public VkActivitySourceOptionsSm(bool posts,
        bool discussions)
    {
        Posts = posts;
        Discussions = discussions;
    }

    /// <summary>
    /// Активности в постах
    /// </summary>
    public bool Posts { get; private set; }

    /// <summary>
    /// Активности в дискуссиях
    /// </summary>
    public bool Discussions { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Posts;
        yield return Discussions;
    }
}
