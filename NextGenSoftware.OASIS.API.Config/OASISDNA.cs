﻿
using NextGenSoftware.Holochain.HoloNET.Client.Core;

namespace NextGenSoftware.OASIS.API.DNA
{
    public class OASISDNA
    {
        public OASIS OASIS { get; set; }

        //public string Secret { get; set; }
        //public EmailSettings Email { get; set; }
        //public StorageProviderSettings StorageProviders { get; set; }
    }

    public class OASIS
    {
        public string Secret { get; set; }
        public EmailSettings Email { get; set; }
        public StorageProviderSettings StorageProviders { get; set; }
    }

    public class StorageProviderSettings
    {
      //  public string DefaultProviders { get; set; }
        public string AutoReplicationProviders { get; set; }
        public string AutoFailOverProviders { get; set; }
        public string AutoLoadBalanceProviders { get; set; }
        public HoloOASISProviderSettings HoloOASIS { get; set; }
        public MongoDBOASISProviderSettings MongoDBOASIS { get; set; }
        public EOSIOASISProviderSettings EOSIOOASIS { get; set; }
        public SEEDSOASISProviderSettings SEEDSOASIS { get; set; }
        public ThreeFoldOASISProviderSettings ThreeFoldOASIS { get; set; }
        public EthereumOASISProviderSettings EthereumOASIS { get; set; }
        public SQLLiteDBOASISSettings SQLLiteDBOASIS { get; set; }
        public IPFSOASISSettings IPFSOASIS { get; set; }
        public Neo4jOASISSettings Neo4jOASIS { get; set; }
    }

    public class EmailSettings
    {
        public string EmailFrom { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
    }

    public class ProviderSettingsBase
    {
        public string ConnectionString { get; set; }
    }

    public class HoloOASISProviderSettings : ProviderSettingsBase
    {
        //public string HolochainVersion { get; set; }
        public HolochainVersion HolochainVersion { get; set; }
    }

    public class MongoDBOASISProviderSettings : ProviderSettingsBase
    {
        public string DBName { get; set; }
    }

    public class EOSIOASISProviderSettings : ProviderSettingsBase
    {
    }

    public class SEEDSOASISProviderSettings : ProviderSettingsBase
    {
    }

    public class ThreeFoldOASISProviderSettings : ProviderSettingsBase
    {

    }

    public class EthereumOASISProviderSettings : ProviderSettingsBase
    {

    }

    public class SQLLiteDBOASISSettings : ProviderSettingsBase
    {
    }

    public class IPFSOASISSettings : ProviderSettingsBase
    {
    }

    public class Neo4jOASISSettings : ProviderSettingsBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}