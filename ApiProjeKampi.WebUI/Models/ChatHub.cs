using ApiProjeKampi.WebUI.Services;
using Microsoft.AspNetCore.SignalR;

namespace ApiProjeKampi.WebUI.Models;

public sealed class ChatHub(AiTextService ai) : Hub
{
    public async Task SendMessage(string userMessage)
    {
        if (string.IsNullOrWhiteSpace(userMessage) || userMessage.Length > 2000)
            throw new HubException("Mesaj 1 ile 2000 karakter arasında olmalıdır.");
        if (!ai.IsConfigured)
            throw new HubException("Seçili AI sağlayıcısının API anahtarı henüz yapılandırılmadı.");
        var history = Context.Items.TryGetValue("History", out var value) ? (List<string>)value! : new List<string>();
        Context.Items["History"] = history;
        await Clients.Caller.SendAsync("ReceiveUserEcho", userMessage);
        try
        {
            var prompt = string.Join("\n", history.Append("Kullanıcı: " + userMessage));
            var answer = await ai.GenerateAsync("Mimmy restoranının Türkçe yardımcı asistanısın. Yemekler ve restoran deneyimi hakkında kısa yanıtlar ver. Güncel fiyat, çalışma saati, rezervasyon onayı gibi bilmediğin işletme bilgilerini uydurma. HTML üretme.", prompt, Context.ConnectionAborted);
            history.Add("Kullanıcı: " + userMessage);
            history.Add("Asistan: " + answer);
            if (history.Count > 12) history.RemoveRange(0, history.Count - 12);
            await Clients.Caller.SendAsync("CompleteMessage", answer);
        }
        catch (AiServiceException error) { throw new HubException(error.Message); }
    }
}
