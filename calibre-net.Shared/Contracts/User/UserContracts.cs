namespace calibre_net.Shared.Contracts;

public record SetReadStatusRequest (int BookId, bool Status);

