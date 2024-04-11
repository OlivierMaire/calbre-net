

using ATL;
using calibre_net.Shared.Contracts;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class AudioTrackMapper{
    public static partial AudioPlayerBlazor.AudioMetadata ToAudioMetadata(this Track obj);

}