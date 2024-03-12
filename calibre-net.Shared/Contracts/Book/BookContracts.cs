
namespace calibre_net.Shared.Contracts;

public record GetBookRequest(int Id);
public record DownloadBookRequest(int Id, string Format);

public record SetBookmarkRequest (int BookId, string BookFormat, string Position);
public record GetBookmarkRequest (int BookId, string BookFormat);
public record GetBookmarkResponse (int BookId, string BookFormat, string Position);