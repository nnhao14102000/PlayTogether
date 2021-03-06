namespace PlayTogether.Api.Helpers
{
    public class AppSettings
    {
        public string KeyVaultName { get; set; }
        public bool ByPassKeyVault { get; set; }
    }

    /*
    
    {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Information",
      "Microsoft.AspNetCore": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ConnectionStrings": {
    "PlayTogetherDbConnection": "Server=(local);Database=PlayTogetherDb;Trusted_Connection=True"
  },
  "AppSettings": {
    "KeyVaultName": "capstone-kv-dev-haonn",
    "ByPassKeyVault": "false"
  },
  "Jwt": {
    "SecretKey": "XeZTAT93AkuUbX3P4wnYbBxCCpcZCkHqKxxLXgkz",
    "Expired": 1
  }
}

    */

    /*
    
    {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "AppSettings": {
    "KeyVaultName": "capstone-kv-dev-haonn",
    "ByPassKeyVault": "false"
  },
  "StorageAccountSecret": "replace from key vault",

  "ConnectionStrings": {
    "PlayTogetherDbConnection": "replace from key vault"
  },

  "Jwt": {
    "SecretKey": "XeZTAT93AkuUbX3P4wnYbBxCCpcZCkHqKxxLXgkz",
    "Expired": 1
  },

  "NLog": {
    "throwConfigExceptions": true,
    "variables": {
      "commonLayout": "${date:format=yyyy-MM-dd HH\\:mm\\:ss.fff} ${level:uppercase=true} ${message}",
      "logDir": "D:\\cn9\\Logs\\PlayTogetherAPI",
      "customFileName": "${level}",
      "shortdate": "${shortdate:universalTime=true}"
    },
    "targets": {
      "logfile": {
        "type": "File",
        "filename": "${logDir}/${customFileName}.${date:format=yyyy-MM-dd}.log",
        "layout": "${commonLayout}",
        "archiveFileName": "${logDir}/${customFileName}.{#}.log",
        "archiveAboveSize": "10485760",
        "archiveNumbering": "Sequence",
        "concurrentWrites": "true"
      },
      "logconsole": {
        "type": "Console",
        "layout": "${commonLayout}"
      }
    },
    "rules": [
      {
        "logger": "*",
        "minLevel": "Trace",
        "writeTo": "logconsole"
      },
      {
        "logger": "*",
        "minLevel": "Trace",
        "writeTo": "logfile"
      }
    ]
  }
}
    */
}