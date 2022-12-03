namespace Api.Models.Interfaces;

public interface IMetadata : IMetadataKey
{
    byte[]? RowVersion { get; set; }
}

public interface IMetadataKey
{
    Guid Id { get; set; }
}