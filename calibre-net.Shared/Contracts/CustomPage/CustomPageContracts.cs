namespace Calibre_net.Shared.Contracts;

public record GetAllCustomPagesResponse(List<CustomPageDto> Pages);

public record PutCustomPageRequest(CustomPageDto Page);

public record GetCustomPageRequest(int Id);

public record DeleteCustomPageRequest(int Id);

public record GetCustomPageMarkupRequest(string Slug);
public record GetCustomPageMarkupResponse(string MarkupString);

public record GetCustomPagesLinksResponse(string Title, string Url);

