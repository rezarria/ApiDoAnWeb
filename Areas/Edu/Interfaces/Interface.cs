namespace Api.Areas.Edu.Interfaces;

public interface IMetadata : IMetadataKey, IMetadataConcurrency
{
}

public interface IMetadataKey
{
    Guid Id { get; set; }
}

public interface IMetadataConcurrency
{
    byte[]? RowVersion { get; set; }
}