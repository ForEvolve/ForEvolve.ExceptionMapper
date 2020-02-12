using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForEvolve.ExceptionFilters
{
    public class ExceptionMapManager : IExceptionMapManager
    {
        public Dictionary<string, ExceptionMap> _maps = new Dictionary<string, ExceptionMap>();

        /// <summary>
        /// Adds or updates the specified <see cref="ExceptionMap"/>.
        /// </summary>
        /// <param name="exceptionMap">The map to be added or updated.</param>
        /// <exception cref="DuplicateExceptionMapException">Thrown when trying to add an existing, user-generated, map.</exception>
        public Task AddOrUpdateAsync(ExceptionMap exceptionMap)
        {
            if (_maps.ContainsKey(exceptionMap.ExceptionName))
            {
                var existingMap = _maps[exceptionMap.ExceptionName];
                if (existingMap.IsUserGenerated)
                {
                    throw new DuplicateExceptionMapException(exceptionMap, existingMap);
                }
                else
                {
                    _maps[exceptionMap.ExceptionName] = exceptionMap;
                }
            }
            else
            {
                _maps.Add(exceptionMap.ExceptionName, exceptionMap);
            }
            return Task.CompletedTask;
        }

        public async Task<bool> HasMapForExceptionAsync(Type exceptionType)
        {
            var initialTypeName = exceptionType.FullName;
            var currentType = exceptionType;
            do
            {
                var name = currentType.FullName;
                if (_maps.ContainsKey(name))
                {
                    if (!_maps.ContainsKey(initialTypeName))
                    {
                        var parentMap = _maps[name];
                        await AddOrUpdateAsync(new HttpExceptionMap(initialTypeName, parentMap.ExecuteAsync, isUserGenerated: false));
                    }
                    return true;
                }
                currentType = currentType.BaseType;
            }
            while (currentType != null);
            return false;
        }

        public async Task<ExceptionMap> GetMapForExceptionAsync(Type exceptionType)
        {
            if (!await HasMapForExceptionAsync(exceptionType))
            {
                throw new MapNotFoundException(exceptionType);
            }
            return _maps[exceptionType.FullName];
        }
    }
}
