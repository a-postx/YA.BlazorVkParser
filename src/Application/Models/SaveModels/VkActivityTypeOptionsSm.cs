namespace YA.WebClient.Application.Models.SaveModels;

/// <summary>
/// Настройки типов активностей
/// </summary>
public class VkActivityTypeOptionsSm : ValueObject
{
    private VkActivityTypeOptionsSm() { }

    public VkActivityTypeOptionsSm(bool likes,
        bool likesInComments,
        bool comments,
        bool reposts,
        bool postAuthors)
    {
        Likes = likes;
        LikesInComments = likesInComments;
        Comments = comments;
        Reposts = reposts;
        PostAuthors = postAuthors;
    }

    /// <summary>
    /// Лайки
    /// </summary>
    public bool Likes { get; private set; }

    /// <summary>
    /// Необходимо зачислить в Активные Пользователи лайкающих в комментариях
    /// </summary>
    public bool LikesInComments { get; set; }

    /// <summary>
    /// Комментарии
    /// </summary>
    public bool Comments { get; private set; }

    /// <summary>
    /// Репосты
    /// </summary>
    public bool Reposts { get; private set; }

    /// <summary>
    /// Авторы постов
    /// </summary>
    public bool PostAuthors { get; private set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Likes;
        yield return LikesInComments;
        yield return Comments;
        yield return Reposts;
        yield return PostAuthors;
    }
}
