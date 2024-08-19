﻿using System;
using System.Collections.Generic;
using NextGenSoftware.Utilities;
using NextGenSoftware.OASIS.API.Core.Enums;
using NextGenSoftware.OASIS.API.Core.Objects;
using NextGenSoftware.OASIS.API.Core.Interfaces;
using NextGenSoftware.OASIS.API.Core.Interfaces.STAR;
using NextGenSoftware.Holochain.HoloNET.ORM.Interfaces;

namespace NextGenSoftware.OASIS.API.Providers.HoloOASIS
{
    public interface IHcAvatarDetail : IHoloNETAuditEntryBase
    {
        #region IAvatarDetail Properties

        Guid Id { get; set; }
        string Username { get; set; }
        string Email { get; set; }
        int Karma { get; set; } //TODO: This really needs to have a private setter but in the HoloOASIS provider it needs to copy the object along with each property... would prefer another work around if possible?
        int Level { get; set; }
        int XP { get; set; }
        string Model3D { get; set; }
        string UmaJson { get; set; }
        string Portrait { get; set; }
        string DOB { get; set; }
        string Address { get; set; }
        string Town { get; set; }
        string County { get; set; }
        string Country { get; set; }
        string Postcode { get; set; }
        string Landline { get; set; }
        string Mobile { get; set; }
        IList<IAchievement> Achievements { get; set; }
        IAvatarAttributes Attributes { get; set; }
        IAvatarAura Aura { get; set; }
        IAvatarChakras Chakras { get; set; }
        IDictionary<DimensionLevel, Guid> DimensionLevelIds { get; set; }
        public IDictionary<DimensionLevel, IHolon> DimensionLevels { get; set; }
        public ConsoleColor FavouriteColour { get; set; }
        public IList<IGeneKey> GeneKeys { get; set; }
        public IList<IAvatarGift> Gifts { get; set; }
        public IList<HeartRateEntry> HeartRateData { get; set; }
        public IHumanDesign HumanDesign { get; set; }
        public IList<IInventoryItem> Inventory { get; set; }
        public IList<KarmaAkashicRecord> KarmaAkashicRecords { get; set; }
        public IOmiverse Omniverse { get; set; }
        public IAvatarSkills Skills { get; set; }
        public IList<ISpell> Spells { get; set; }
        public ConsoleColor STARCLIColour { get; set; }
        public IAvatarStats Stats { get; set; }
        public IAvatarSuperPowers SuperPowers { get; set; }

        #endregion

        #region IHolonBase Properties

        IList<IHolon> Children { get; set; } //Allows any holon to add any number of custom child holons to it.
        IReadOnlyCollection<IHolon> AllChildren { get; set; } //Readonly collection of all the total children including all the zomes, celestialbodies, celestialspaces, moons, holons, planets, stars etc belong to the holon.
        //Guid CreatedByAvatarId { get; set; }
        //DateTime CreatedDate { get; set; }
        EnumValue<OASISType> CreatedOASISType { get; set; }
        EnumValue<ProviderType> CreatedProviderType { get; set; }
        string CustomKey { get; set; }
        //Guid DeletedByAvatarId { get; set; }
        //DateTime DeletedDate { get; set; }
        string Description { get; set; }
        HolonType HolonType { get; set; }
        EnumValue<ProviderType> InstanceSavedOnProviderType { get; set; }
        bool IsActive { get; set; }
        bool IsChanged { get; set; }
        bool IsNewHolon { get; set; }
        bool IsSaving { get; set; }
        Dictionary<string, object> MetaData { get; set; }
        //Guid ModifiedByAvatarId { get; set; }
        //DateTime ModifiedDate { get; set; }
        string Name { get; set; }
        IHolon Original { get; set; }
        //IHolon ParentHolon { get; set; }
        Guid ParentHolonId { get; set; }
        Guid PreviousVersionId { get; set; }
        Dictionary<ProviderType, string> PreviousVersionProviderUniqueStorageKey { get; set; }
        Dictionary<ProviderType, Dictionary<string, string>> ProviderMetaData { get; set; }
        Dictionary<ProviderType, string> ProviderUniqueStorageKey { get; set; }
        int Version { get; set; }
        Guid VersionId { get; set; }
        string ChildIdListCache { get; set; } //This will store the list of id's for the direct childen of this holon.
        string AllChildIdListCache { get; set; } //This will store the list of id's for the ALL the childen of this holon (including all sub-childen).

        #endregion
    }
}