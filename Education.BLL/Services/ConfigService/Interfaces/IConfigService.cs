using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.ConfigService.Interfaces
{
    public interface IConfigService
    {
        string ConnectionString { get; set; }
        string Name { get; set; }
        string IconPath { get; set; }
    }
}
