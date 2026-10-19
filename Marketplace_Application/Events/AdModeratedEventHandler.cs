using Marketplace.Domain.Repositories;

namespace Marketplace.Application.Events;

public record AdModeratedEvent(Guid EventId, Guid AdId, string NewStatus, string? RejectionReason);

public class AdModeratedEventHandler
{
    private readonly IAdRepository _adRepository;
    private readonly IProcessedEventRepository _processedEventRepository;

    public AdModeratedEventHandler(
        IAdRepository adRepository,
        IProcessedEventRepository processedEventRepository)
    {
        _adRepository = adRepository;
        _processedEventRepository = processedEventRepository;
    }

    public async Task HandleAsync(AdModeratedEvent @event, CancellationToken ct = default)
    {
        // 1. Перевірка ідемпотентності: чи оброблялася вже ця подія?
        if (await _processedEventRepository.HasBeenProcessedAsync(@event.EventId, ct))
        {
            // Подія вже була оброблена раніше — ігноруємо повтор без помилки
            return;
        }

        // 2. Виконання бізнес-логіки
        var ad = await _adRepository.GetByIdAsync(@event.AdId, ct);
        if (ad != null)
        {
            if (@event.NewStatus == "Approved")
            {
                ad.Approve();
            }
            else if (@event.NewStatus == "Rejected")
            {
                ad.Reject(@event.RejectionReason ?? "Причина не вказана");
            }

            await _adRepository.UpdateAsync(ad, ct);
        }

        // 3. Фіксація ідентифікатора події для забезпечення ідемпотентності
        await _processedEventRepository.MarkAsProcessedAsync(@event.EventId, ct);
    }
}