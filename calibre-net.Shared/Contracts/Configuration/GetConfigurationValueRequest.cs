
namespace calibre_net.Shared.Contracts;

public record GetConfigurationValueRequest(string Value);

public record GetConfigurationValueResponse(string Key, string Value);