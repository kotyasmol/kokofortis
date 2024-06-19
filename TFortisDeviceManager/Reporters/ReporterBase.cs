using Serilog;
using System.Collections.Generic;

namespace TFortisDeviceManager.Reporters
{
    public abstract class ReporterBase
    {
        /*public IReadOnlyList<string> Errors => _errors;
        public bool HasErrors => _errors.Count > 0;

        private readonly List<string> _errors = new();
        private readonly ILogger _logger;

        protected void AddError(string errorText)
        {            
            _errors.Add(errorText);

            Log.Error(errorText);
        }

        protected ReporterBase(ILogger logger)
        {
            _logger = logger;
        }*/
    }
}
