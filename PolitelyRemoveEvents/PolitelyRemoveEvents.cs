using System.Reflection;
using System.Text.Json;
using JetBrains.Annotations;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;

namespace PolitelyRemoveEvents;

[UsedImplicitly]
[Injectable(TypePriority = OnLoadOrder.Database)]
public class PolitelyRemoveEvents(ConfigServer cfgServer, ModHelper modHelper, ISptLogger<PolitelyRemoveEvents> logger) : IOnLoad
{
    private PREConfig? _config;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };
    public Task OnLoad()
    {
        var questCfg = cfgServer.GetConfig<SeasonalEventConfig>();
        
        var modFolder = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        var cfgPath = Path.Combine(modFolder, "config.json");
        
        if (!File.Exists(cfgPath))
        {
            var json = JsonSerializer.Serialize(new PREConfig(), _jsonSerializerOptions);
            File.WriteAllText(cfgPath, json);
        }
        
        _config = JsonSerializer.Deserialize<PREConfig>(File.ReadAllText(cfgPath), _jsonSerializerOptions);
        
        foreach (var seasonalEvent in questCfg.Events)
        {
            switch (seasonalEvent.Type)
            {
                case SeasonalEventType.None: break;
                
                case SeasonalEventType.Christmas:
                {
                    if (_config is { DisableChristmas: true })
                        seasonalEvent.Enabled = false;
                    break;
                }
                
                case SeasonalEventType.Halloween:
                {
                    if (_config is { DisableHalloween: true })
                        seasonalEvent.Enabled = false;
                    break;
                }

                case SeasonalEventType.NewYears:
                {
                    if(_config is { DisableNewYears: true }) 
                        seasonalEvent.Enabled = false;
                    break;
                }
                
                case SeasonalEventType.Promo:
                {
                    if(_config is { DisablePromo: true })
                        seasonalEvent.Enabled = false;
                    break;
                }

                case SeasonalEventType.AprilFools:
                {
                    if(_config is { DisableAprilFools: true }) 
                        seasonalEvent.Enabled = false;
                    break;
                }
                
                // ReSharper disable once RedundantEmptySwitchSection
                default: { break; }
            }
        }
        
        logger.Info("Disabled task:");
        logger.Info($"Halloween: {_config is { DisableHalloween: true }}");
        logger.Info($"Christmas: {_config is { DisableChristmas: true }}");
        logger.Info($"New Years: {_config is { DisableNewYears: true }}");
        logger.Info($"Promo: {_config is { DisablePromo: true }}");
        logger.Info($"April Fools: {_config is { DisableAprilFools: true }}");
        
        return Task.CompletedTask;
    }
}