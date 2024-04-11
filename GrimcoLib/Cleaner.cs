#if NET8_0_WINDOWS
using System;
using System.Linq;
using Dalamud.Game.Text.Sanitizer;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;

namespace GrimcoLib;

public class Cleaner(ISanitizer sanitizer)
{
    private static readonly byte[] NewLinePayload = [0x02, 0x10, 0x01, 0x03];

    public SeString Convert(Lumina.Text.SeString lumina)
    {
        var se = (SeString)lumina;
        for (var i = 0; i < se.Payloads.Count; i++)
            switch (se.Payloads[i].Type)
            {
                case PayloadType.Unknown:
                    if (se.Payloads[i].Encode().SequenceEqual(NewLinePayload))
                    {
                        se.Payloads[i] = new TextPayload("\n");
                    }

                    break;
                case PayloadType.RawText:
                    if (se.Payloads[i] is TextPayload payload)
                    {
                        payload.Text = sanitizer.Sanitize(payload.Text);
                    }

                    break;
            }

        return se;
    }
}
#endif