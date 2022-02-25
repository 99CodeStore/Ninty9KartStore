﻿namespace NintyNineKartStore.Utility
{
        public class MailClientConfigurationOption
        {
            public string Host { get; set; }
            public int Port { get; set; }
            public bool UseSsl { get; set; }
            public bool UseTls { get; set; }

            public string FromEmail { get; set; }
            public string Password { get; set; }

            public string DisplayName { get; set; }

        }
    }

