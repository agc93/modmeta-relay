using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ModMeta.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ModMetaRelay.Controllers
{
    [ApiController]
    [Route("")]
    public class MetaController : ControllerBase
    {
        public IEnumerable<IModMetaSource> Sources {get;set;}

        private readonly ILogger<MetaController> _logger;

        public MetaController(IEnumerable<IModMetaSource> sources, ILogger<MetaController> logger)
        {
            Sources = sources;
            if (!Sources.Any()) {
                logger.LogWarning("No metadata sources configured! If no plugins are loaded, queries will always return empty results");
            }
            _logger = logger;
        }

        private async Task<IEnumerable<ILookupResult>> GetAllSources(Func<IModMetaSource, Task<IEnumerable<ILookupResult>>> searchFunc) {
            var tasks = Sources.Select(s => searchFunc(s));
            try
            {
                await Task.WhenAny(Task.WhenAll(tasks), Task.Delay(5000));
                var completions = tasks
                .Where(t => t.Status == TaskStatus.RanToCompletion)
                .SelectMany(t => t.Result);
                return completions;
            }
            catch (NotImplementedException)
            {
                //ignored
            }
            return new List<ILookupResult>();
        }

        private async Task<IEnumerable<ILookupResult>> GetSources(Func<IModMetaSource, bool> sourceFilter, Func<IModMetaSource, Task<IEnumerable<ILookupResult>>> searchFunc) {
            var tasks = Sources.Where(sourceFilter).Select(s => searchFunc(s));
            try
            {
                await Task.WhenAny(Task.WhenAll(tasks), Task.Delay(5000));
                var completions = tasks
                .Where(t => t.Status == TaskStatus.RanToCompletion)
                .SelectMany(t => t.Result);
                return completions;
            }
            catch (NotImplementedException)
            {
                //ignored
            }
            // await Task.WhenAny(Task.WhenAll(tasks), Task.Delay(5000));
            
            // var allSources = await Task.WhenAll(Sources.Select(s => searchFunc(s)));
            // var matches = allSources.SelectMany(s => s.ToList());
            // return completions;
            return new List<ILookupResult>();
        }

        [HttpGet("by_key/{fileHash}")]
        public async Task<IActionResult> GetByFileHash([FromRoute]string fileHash) {
            _logger.LogDebug($"Hit GetByFileHash with {fileHash}");
            //TODO: This ⬇ is fucking horrible
            // Sources.SelectMany<IModMetaSource, ILookupResult>(s => s.GetByKey(fileHash).GetAwaiter().GetResult());
            var matches = await GetSources(sf => sf.Supports(LookupType.Hash), s => s.GetByKey(fileHash));
            // return NotFound();
            return Ok(matches);
        }

        [HttpGet("by_name/{fileName}/{fileVersion}")]
        public async Task<IActionResult> GetByFileName([FromRoute]string fileName, [FromRoute]string fileVersion) {
            _logger.LogDebug($"Hit GetByFileName with {fileName} and {fileVersion}");
            var matches = await GetSources(sf => sf.Supports(LookupType.LogicalName), s => s.GetByLogicalName(fileName, fileVersion));
            // return NotFound();
            return Ok(matches);
        }

        [HttpGet("by_expression/{expression}/{fileVersion}")]
        public async Task<IActionResult> GetByExpressionAsync([FromRoute]string expression, string fileVersion) {
            _logger.LogDebug($"Hit GetByExpression with {expression} and {fileVersion}");
            var matches = await GetSources(sf => sf.Supports(LookupType.FileExpression), s => s.GetByExpression(expression, fileVersion));
            return Ok(matches);
        }

        [HttpPost("describe")]
        public IActionResult DescribeMod([FromBody]IModInfo modInfo) {
            _logger.LogWarning($"Hit Describe with body: {modInfo}");
            return StatusCode(501);
        }
    }

    internal static class MetaExtensions {
        internal static bool Supports(this IModMetaSource s, LookupType type) {
            return s.SupportedTypes.HasFlag(type);
        }
    }
}