using System.Text.Json.Serialization;
using Fido2NetLib;
using Fido2NetLib.Objects;

namespace calibre_net.Models;

public class PublicKeyCredentialDescriptorModel
{
 //
    // Summary:
    //     This member contains the type of the public key credential the caller is referring
    //     to.
    [JsonPropertyName("type")]
    public PublicKeyCredentialType Type { get; }

    //
    // Summary:
    //     This member contains the credential ID of the public key credential the caller
    //     is referring to.
    [JsonConverter(typeof(Base64UrlConverter))]
    [JsonPropertyName("id")]
    public byte[] Id { get; }

    //
    // Summary:
    //     This OPTIONAL member contains a hint as to how the client might communicate with
    //     the managing authenticator of the public key credential the caller is referring
    //     to.
    [JsonPropertyName("transports")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AuthenticatorTransport[]? Transports { get; }

    public PublicKeyCredentialDescriptorModel(){
        Id = [];
    }

    public PublicKeyCredentialDescriptorModel(byte[] id)
        : this(PublicKeyCredentialType.PublicKey, id)
    {
    }

    [JsonConstructor]
    public PublicKeyCredentialDescriptorModel(PublicKeyCredentialType type, byte[] id, AuthenticatorTransport[]? transports = null)
    {
        ArgumentNullException.ThrowIfNull(id, "id");
        Type = type;
        Id = id;
        Transports = transports;
    }
}