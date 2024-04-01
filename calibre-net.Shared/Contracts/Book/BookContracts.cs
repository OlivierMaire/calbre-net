
using System.Drawing;

namespace calibre_net.Shared.Contracts;

public record GetBookRequest(int Id);
public record DownloadBookRequest(int Id, string Format);

public record SetBookmarkRequest (int BookId, string BookFormat, string Position);
public record GetBookmarkRequest (int BookId, string BookFormat);
public record GetBookmarkResponse (int BookId, string BookFormat, string Position);

public record GetPageRequest(int BookId, string BookFormat, int PageId);
public record GetPageResponse(byte[] PageContent, string ContentType, Size? Size, string? LeftColor, string? RightColor);
public record GetBookDataResponse(byte[] Data, string contentType);

public record GetCustomColumnListResponse(List<CustomColumnDto> CustomColumns);
