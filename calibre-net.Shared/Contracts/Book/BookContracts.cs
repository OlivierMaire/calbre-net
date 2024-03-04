
namespace calibre_net.Shared.Contracts;

public record GetBookRequest(int Id);
public record DownloadBookRequest(int Id, string Format);

