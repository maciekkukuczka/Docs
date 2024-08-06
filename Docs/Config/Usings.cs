// SYSTEM
global using Microsoft.AspNetCore.Components.Authorization;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using System.ComponentModel.DataAnnotations;
global using System.Security.Claims;
global using System.Linq.Expressions;
global using System.Text.Json.Serialization;
global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Components.Forms;
global using Microsoft.AspNetCore.Components.Routing;
global using System.Text.Json;

// NAMESPACES
global using Docs.Config;
global using Docs.Data;
global using Docs.Components;
global using Docs.Components.Account;
global using Docs.Config.Exc;
/*global using Docs.Migrations.Sqlite;
global using Docs.Migrations.SqlServer;*/

// 3RD PARTY
global using MudBlazor;
global using MudBlazor.Services;
global using Serilog;
global using Serilog.Sinks.OpenTelemetry;
global using Serilog.Sinks.SystemConsole.Themes;

// MODULES
global using Mac.Modules.DataSeed;

global using Docs.Modules.Generic.Services;

global using Docs.Modules.Common.Models;
global using Docs.Modules.Common.Result;

global using Docs.Modules.Items.Models;
global using Docs.Modules.Items.Models.ViewModels;
global using Docs.Modules.Items.Services;

global using Docs.Modules.Subjects.Models;
global using Docs.Modules.Subjects.Models.ViewModels;
global using Docs.Modules.Subjects.Services;

global using Docs.Modules.Categories.Models;
global using Docs.Modules.Categories.Models.ViewModels;
global using Docs.Modules.Categories.Services;

global using Docs.Modules.Links.Models;
global using Docs.Modules.Links.Services;


