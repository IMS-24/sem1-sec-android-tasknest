namespace net.mstoegerer.TaskNest.Api.Domain.DTOs;

public class MetaDataDto
{
    // public Guid Id { get; set; }
    // public int Order { get; set; }
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;

    public string ToJson()
    {
        return $"{{\"Key\":\"{Key}\",\"Value\":\"{Value}\"}}";
    }
}

public class CreateUserMetaDataDto
{
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public List<MetaDataDto> MetaData { get; set; } = [];
    public PointDto? Location { get; set; }

    // public string ToCsv(bool includeHeader, char? separator = ',')
    // {
    //     var sepCharacter = separator ?? CultureInfo.CurrentCulture.TextInfo.ListSeparator[0];
    //     var sb = new StringBuilder();
    //     if (includeHeader)
    //         sb.Append(
    //             $"UserId{sepCharacter}CreatedUtc{sepCharacter}Data{sepCharacter}LocationX{sepCharacter}LocationY{Environment.NewLine}");
    //     sb.Append(
    //         $"{UserId}{sepCharacter}{CreatedUtc}{sepCharacter}\"{MetaData.ToJson()}\"{sepCharacter}{Location?.X}{sepCharacter}{Location?.Y}{Environment.NewLine}");
    //     return sb.ToString();
    // }
}