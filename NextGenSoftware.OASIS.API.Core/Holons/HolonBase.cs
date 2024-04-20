﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using NextGenSoftware.OASIS.Common;
using NextGenSoftware.OASIS.API.Core.Enums;
using NextGenSoftware.OASIS.API.Core.Events;
using NextGenSoftware.OASIS.API.Core.Helpers;
using NextGenSoftware.OASIS.API.Core.Interfaces;
using NextGenSoftware.OASIS.API.Core.Managers;

namespace NextGenSoftware.OASIS.API.Core.Holons
{
    public abstract class HolonBase : IHolonBase, INotifyPropertyChanged
    {
        private const string CONST_USERMESSAGE_ID_OR_PROVIDERKEY_NOTSET = "Both Id and ProviderUniqueStorageKey are null, one of these need to be set before calling this method.";
        private string _name;
        private string _description;

        public HolonBase(Guid id)
        {
            Id = id;
        }

        public HolonBase(string providerKey, ProviderType providerType)
        {
            if (providerType == ProviderType.Default)
                providerType = ProviderManager.Instance.CurrentStorageProviderType.Value;

            this.ProviderUniqueStorageKey[providerType] = providerKey;
        }

        //public HolonBase(Dictionary<ProviderType, string> providerKeys)
        //{
        //    ProviderUniqueStorageKey = providerKeys;
        //}

        public HolonBase(HolonType holonType)
        {
            IsNewHolon = true;
            HolonType = holonType;
        }

        public HolonBase()
        {
            IsNewHolon = true;
        }

        public event EventDelegates.Initialized OnInitialized;
        public event EventDelegates.HolonLoaded OnLoaded;
        public event EventDelegates.HolonSaved OnSaved;
        public event EventDelegates.HolonDeleted OnDeleted;
        public event EventDelegates.HolonAdded OnHolonAdded;
        public event EventDelegates.HolonRemoved OnHolonRemoved;
        public event EventDelegates.HolonsLoaded OnChildrenLoaded;
        public event EventDelegates.HolonError OnError;
        public event EventDelegates.HolonsError OnChildrenLoadError;

        public IHolon Original { get; set; }

        public Guid Id { get; set; } //Unique id within the OASIS.
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (value != _name)
                {
                    IsChanged = true;
                    NotifyPropertyChanged("Name");
                }

                _name = value;
            }
        }
        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                if (value != _description)
                {
                    IsChanged = true;
                    NotifyPropertyChanged("Description");
                }

                _description = value;
            }
        }

        //TODO: Finish converting all properties so are same as above...
        public Dictionary<ProviderType, string> ProviderUniqueStorageKey { get; set; } = new Dictionary<ProviderType, string>(); //Unique key used by each provider (e.g. hashaddress in hc, accountname for Telos, id in MongoDB etc).        
        public Dictionary<ProviderType, Dictionary<string, string>> ProviderMetaData { get; set; } = new Dictionary<ProviderType, Dictionary<string, string>>(); // Key/Value pair meta data can be stored here, which is unique for that provider.
        public Dictionary<string, object> MetaData { get; set; } = new Dictionary<string, object>(); // Key/Value pair meta data can be stored here that applies globally across ALL providers.
        public string CustomKey { get; set; } //A custom key that can be used to load the holon by (other than Id or ProviderKey).
        //public Dictionary<string, string> CustomKeys { get; set; }

        public bool IsNewHolon { get; set; }
        public bool IsChanged { get; set; }
        public bool IsSaving { get; set; }

        //public Dictionary<ProviderType, string> ProviderUniqueStorageKey { get; set; } = new Dictionary<ProviderType, string>(); //Unique key used by each provider (e.g. hashaddress in hc, accountname for Telos, id in MongoDB etc).        
        //public Dictionary<ProviderType, Dictionary<string, string>> ProviderMetaData { get; set; } = new Dictionary<ProviderType, Dictionary<string, string>>(); // Key/Value pair meta data can be stored here, which is unique for that provider.
        //public Dictionary<string, string> MetaData { get; set; } = new Dictionary<string, string>(); // Key/Value pair meta data can be stored here that applies globally across ALL providers.
        public HolonType HolonType { get; set; }

        /*
        public Guid ParentOmniverseId { get; set; } //The Omniverse this Holon belongs to.
        public IOmiverse ParentOmniverse { get; set; } //The Omniverse this Holon belongs to.
        public Guid ParentMultiverseId { get; set; } //The Multiverse this Holon belongs to.
        public IMultiverse ParentMultiverse { get; set; } //The Multiverse this Holon belongs to.
        public Guid ParentUniverseId { get; set; } //The Universe this Holon belongs to.
        public IUniverse ParentUniverse { get; set; } //The Universe this Holon belongs to.
        public Guid ParentDimensionId { get; set; } //The Dimension this Holon belongs to.
        public IDimension ParentDimension { get; set; } //The Dimension this Holon belongs to.
        public DimensionLevel DimensionLevel { get; set; } //The dimension this Holon belongs to (a holon can have a different version of itself in each dimension (asscended/evolved versions of itself).
        public SubDimensionLevel SubDimensionLevel { get; set; } //The sub-dimension/plane this Holon belongs to.
        public Guid ParentGalaxyClusterId { get; set; } //The GalaxyCluster this Holon belongs to.
        public IGalaxyCluster ParentGalaxyCluster { get; set; } //The GalaxyCluster this Holon belongs to.
        public Guid ParentGalaxyId { get; set; } //The Galaxy this Holon belongs to.
        public IGalaxy ParentGalaxy { get; set; } //The Galaxy this Holon belongs to.
        public Guid ParentSolarSystemId { get; set; } //The SolarSystem this Holon belongs to.
        public ISolarSystem ParentSolarSystem { get; set; } //The SolarSystem this Holon belongs to.
        public Guid ParentGreatGrandSuperStarId { get; set; } //The GreatGrandSuperStar this Holon belongs to.
        public IGreatGrandSuperStar ParentGreatGrandSuperStar { get; set; } //The GreatGrandSuperStar this Holon belongs to.
        public Guid ParentGrandSuperStarId { get; set; } //The GrandSuperStar this Holon belongs to.
        public IGrandSuperStar ParentGrandSuperStar { get; set; } //The GrandSuperStar this Holon belongs to.
        public Guid ParentSuperStarId { get; set; } //The SuperStar this Holon belongs to.
        public ISuperStar ParentSuperStar { get; set; } //The SuperStar this Holon belongs to.
        public Guid ParentStarId { get; set; } //The Star this Holon belongs to.
        //public ICelestialBody ParentStar { get; set; } //The Star this Holon belongs to.
        public IStar ParentStar { get; set; } //The Star this Holon belongs to.
        public Guid ParentPlanetId { get; set; } //The Planet this Holon belongs to.
        //public ICelestialBody ParentPlanet { get; set; } //The Planet this Holon belongs to.
        public IPlanet ParentPlanet { get; set; } //The Planet this Holon belongs to.
        public Guid ParentMoonId { get; set; } //The Moon this Holon belongs to.
        //public ICelestialBody ParentMoon { get; set; } //The Moon this Holon belongs to.
        public IMoon ParentMoon { get; set; } //The Moon this Holon belongs to.
        //public Guid ParentCelestialBodyId { get; set; } //The CelestialBody (Planet or Moon (OApp)) this Holon belongs to.
        //public ICelestialBody ParentCelestialBody { get; set; } //The CelestialBody (Planet or Moon (OApp)) this Holon belongs to.
        public Guid ParentZomeId { get; set; } // The zome this holon belongs to. Zomes are like re-usable modules that other OApp's can be composed of. Zomes contain collections of nested holons (data objects). Holons can be infinite depth.
        public IZome ParentZome { get; set; } // The zome this holon belongs to. Zomes are like re-usable modules that other OApp's can be composed of. Zomes contain collections of nested holons (data objects). Holons can be infinite depth.
        public Guid ParentHolonId { get; set; }
        public IHolon ParentHolon { get; set; }
        public IEnumerable<IHolon> Children { get; set; }
        public ObservableCollection<IHolon> ChildrenTest { get; set; }
        */
        public Guid CreatedByAvatarId { get; set; }
        public Avatar CreatedByAvatar { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedByAvatarId { get; set; }
        public Avatar ModifiedByAvatar { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid DeletedByAvatarId { get; set; }
        public Avatar DeletedByAvatar { get; set; }
        public DateTime DeletedDate { get; set; }
        public int Version { get; set; }
        public Guid VersionId { get; set; }
        public Guid PreviousVersionId { get; set; }
        public Dictionary<ProviderType, string> PreviousVersionProviderUniqueStorageKey { get; set; } = new Dictionary<ProviderType, string>();
        public bool IsActive { get; set; }
        public EnumValue<ProviderType> CreatedProviderType { get; set; } // The primary provider that this holon was originally saved with (it can then be auto-replicated to other providers to give maximum redundancy/speed via auto-load balancing etc).
                                                                         //public List<INode> Nodes { get; set; } // List of nodes/fields (int, string, bool, etc) that belong to this Holon (STAR ODK auto-generates these when generating dynamic code from DNA Templates passed in).
                                                                         //  public ObservableCollection<INode> Nodes { get; set; }

        public EnumValue<ProviderType> InstanceSavedOnProviderType { get; set; }

        public EnumValue<OASISType> CreatedOASISType { get; set; }

        public Guid ParentHolonId { get; set; }
        public IHolon ParentHolon { get; set; }
        public IEnumerable<IHolon> Children { get; set; }
        public ObservableCollection<IHolon> ChildrenTest { get; set; }

        /// <summary>
        /// Fired when a property in this class changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /*
       public Holon()
       {
           //TODO: Need to check if these are fired when an item in the collection is changed (not just added/removed).
           if (ChildrenTest != null)
               ChildrenTest.CollectionChanged += Children_CollectionChanged;

           if (Nodes != null)
               Nodes.CollectionChanged += Nodes_CollectionChanged;
       }

       private void Nodes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
       {
           IsChanged = true;

           //TOOD: Not sure if we need this? Because ObservableCollection is supposed to raise PropertyChanged events itself.
           NotifyPropertyChanged("Nodes");
       }

       private void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
       {
           IsChanged = true;

           //TOOD: Not sure if we need this? Because ObservableCollection is supposed to raise PropertyChanged events itself.
           NotifyPropertyChanged("Children");
       }
          */


        /// <summary>
        /// Triggers the property changed event for a specific property.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual bool HasHolonChanged(bool checkChildren = true)
        {
            if (IsChanged)
                return true;

            if (Original != null)
            {
                if (Original.Id != Id)
                    return true;

                if (Original.Name != Name)
                    return true;

                if (Original.Description != Description)
                    return true;

                if (Original.CreatedByAvatar != CreatedByAvatar)
                    return true;

                if (Original.CreatedByAvatarId != CreatedByAvatarId)
                    return true;

                if (Original.CreatedDate != CreatedDate)
                    return true;

                if (Original.ModifiedByAvatar != ModifiedByAvatar)
                    return true;

                if (Original.ModifiedByAvatarId != ModifiedByAvatarId)
                    return true;

                if (Original.ModifiedDate != ModifiedDate)
                    return true;

                if (Original.CreatedProviderType != CreatedProviderType)
                    return true;

                if (Original.DeletedByAvatar != DeletedByAvatar)
                    return true;

                if (Original.DeletedByAvatarId != DeletedByAvatarId)
                    return true;

                if (Original.DeletedDate != DeletedDate)
                    return true;

                if (Original.HolonType != HolonType)
                    return true;

                if (Original.IsActive != IsActive)
                    return true;

                if (Original.CreatedOASISType != CreatedOASISType)
                    return true;

                if (Original.CustomKey != CustomKey)
                    return true;

                if (Original.InstanceSavedOnProviderType != InstanceSavedOnProviderType)
                    return true;

                if (Original.InstanceSavedOnProviderType != InstanceSavedOnProviderType)
                    return true;

                if (Original.PreviousVersionId != PreviousVersionId)
                    return true;

                if (Original.PreviousVersionProviderUniqueStorageKey != PreviousVersionProviderUniqueStorageKey)
                    return true;

                if (Original.ProviderMetaData != ProviderMetaData)
                    return true;

                if (Original.ProviderUniqueStorageKey != ProviderUniqueStorageKey)
                    return true;

                if (Original.Version != Version)
                    return true;

                if (Original.VersionId != VersionId)
                    return true;
            }

            return Id != Guid.Empty;
        }

        public async Task<OASISResult<IHolon>> LoadAsync(bool loadChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true, bool loadChildrenFromProvider = false, int version = 0, ProviderType providerType = ProviderType.Default)
        {
            OASISResult<IHolon> result = new OASISResult<IHolon>();

            try
            {
                if (this.HolonType == HolonType.GreatGrandSuperStar)
                    GetGreatGrandSuperStar(ref result, await HolonManager.Instance.LoadAllHolonsAsync(HolonType.GreatGrandSuperStar, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, providerType));

                else if (this.Id != Guid.Empty)
                    result = await HolonManager.Instance.LoadHolonAsync(this.Id, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, providerType);

                else if (this.ProviderUniqueStorageKey != null && this.ProviderUniqueStorageKey.Count > 0)
                {
                    OASISResult<string> providerKeyResult = GetCurrentProviderKey(providerType);

                    if (!providerKeyResult.IsError && !string.IsNullOrEmpty(providerKeyResult.Result))
                        result = await HolonManager.Instance.LoadHolonAsync(providerKeyResult.Result, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, providerType);
                    else
                        OASISErrorHandling.HandleError(ref result, $"Error occured in HolonBase.LoadAsync. Reason: {providerKeyResult.Message}", providerKeyResult.DetailedMessage);
                }
                else
                {
                    result.IsError = true;
                    result.Message = CONST_USERMESSAGE_ID_OR_PROVIDERKEY_NOTSET;
                }

                if (result != null && !result.IsError && result.Result != null)
                {
                    SetProperties(result.Result);
                    OnLoaded?.Invoke(this, new HolonLoadedEventArgs() { Result = result });
                }
                else
                {
                    OASISErrorHandling.HandleError(ref result, $"Error Occured in HolonBase.LoadAsync Calling HolonManager.LoadHolonAsync. Reason: {result.Message}");
                    OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = result.Exception });
                }

            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, $"Unknown Error Occured in HolonBase.LoadAsync Calling HolonManager.LoadHolonAsync. Reason: {ex}");
                OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public async Task<OASISResult<T>> LoadAsync<T>(bool loadChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true, bool loadChildrenFromProvider = false, int version = 0, ProviderType providerType = ProviderType.Default) where T : IHolon, new()
        {
            OASISResult<T> result = new OASISResult<T>();

            try
            {
                if (this.HolonType == HolonType.GreatGrandSuperStar)
                    GetGreatGrandSuperStar(ref result, await HolonManager.Instance.LoadAllHolonsAsync(HolonType.GreatGrandSuperStar, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, providerType));

                else if (this.Id != Guid.Empty)
                    result = await HolonManager.Instance.LoadHolonAsync<T>(this.Id, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, providerType);

                else if (this.ProviderUniqueStorageKey != null && this.ProviderUniqueStorageKey.Count > 0)
                {
                    OASISResult<string> providerKeyResult = GetCurrentProviderKey(providerType);

                    if (!providerKeyResult.IsError && !string.IsNullOrEmpty(providerKeyResult.Result))
                        result = await HolonManager.Instance.LoadHolonAsync<T>(providerKeyResult.Result, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, providerType);
                    else
                        OASISErrorHandling.HandleError(ref result, $"Error occured in HolonBase.LoadAsync<T>. Reason: {providerKeyResult.Message}", providerKeyResult.DetailedMessage);
                }
                else
                {
                    result.IsError = true;
                    result.Message = CONST_USERMESSAGE_ID_OR_PROVIDERKEY_NOTSET;
                }

                if (result != null && !result.IsError && result.Result != null)
                {
                    SetProperties(result.Result);
                    MapMetaData<T>();
                    OnLoaded?.Invoke(this, new HolonLoadedEventArgs() { Result = OASISResultHelper.CopyResult(result) });
                }
                else
                {
                    OASISErrorHandling.HandleError(ref result, $"Error Occured in HolonBase.LoadAsync<T> Calling HolonManager.LoadHolonAsync<T>. Reason: {result.Message}");
                    OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = result.Exception });
                }
            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, $"Unknown Error Occured in HolonBase.LoadAsync<T> Calling HolonManager.LoadHolonAsync<T>. Reason: {ex}");
                OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public OASISResult<IHolon> Load(bool loadChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true, bool loadChildrenFromProvider = false, int version = 0, ProviderType providerType = ProviderType.Default)
        {
            OASISResult<IHolon> result = new OASISResult<IHolon>();

            try
            {
                if (this.HolonType == HolonType.GreatGrandSuperStar)
                    GetGreatGrandSuperStar(ref result, HolonManager.Instance.LoadAllHolons(HolonType.GreatGrandSuperStar, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, providerType));

                else if (this.Id != Guid.Empty)
                    result = HolonManager.Instance.LoadHolon(this.Id, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, providerType);

                else if (this.ProviderUniqueStorageKey != null && this.ProviderUniqueStorageKey.Count > 0)
                {
                    OASISResult<string> providerKeyResult = GetCurrentProviderKey(providerType);

                    if (!providerKeyResult.IsError && !string.IsNullOrEmpty(providerKeyResult.Result))
                        result = HolonManager.Instance.LoadHolon(providerKeyResult.Result, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, providerType);
                    else
                        OASISErrorHandling.HandleError(ref result, $"Error occured in HolonBase.Load. Reason: {providerKeyResult.Message}", providerKeyResult.DetailedMessage);
                }
                else
                {
                    result.IsError = true;
                    result.Message = CONST_USERMESSAGE_ID_OR_PROVIDERKEY_NOTSET;
                }

                if (result != null && !result.IsError && result.Result != null)
                {
                    SetProperties(result.Result);
                    OnLoaded?.Invoke(this, new HolonLoadedEventArgs() { Result = result });
                }
                else
                {
                    OASISErrorHandling.HandleError(ref result, $"Error Occured in HolonBase.Load Calling HolonManager.LoadHolon. Reason: {result.Message}");
                    OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = result.Exception });
                }

            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, $"Unknown Error Occured in HolonBase.Load Calling HolonManager.LoadHolon. Reason: {ex}");
                OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public OASISResult<T> Load<T>(bool loadChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true, bool loadChildrenFromProvider = false, int version = 0, ProviderType providerType = ProviderType.Default) where T : IHolon, new()
        {
            OASISResult<T> result = new OASISResult<T>();

            try
            {
                if (this.HolonType == HolonType.GreatGrandSuperStar)
                    GetGreatGrandSuperStar(ref result, HolonManager.Instance.LoadAllHolons(HolonType.GreatGrandSuperStar, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, providerType));

                else if (this.Id != Guid.Empty)
                    result = HolonManager.Instance.LoadHolon<T>(this.Id, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, providerType);

                else if (this.ProviderUniqueStorageKey != null && this.ProviderUniqueStorageKey.Count > 0)
                {
                    OASISResult<string> providerKeyResult = GetCurrentProviderKey(providerType);

                    if (!providerKeyResult.IsError && !string.IsNullOrEmpty(providerKeyResult.Result))
                        result = HolonManager.Instance.LoadHolon<T>(providerKeyResult.Result, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, providerType);
                    else
                        OASISErrorHandling.HandleError(ref result, $"Error occured in HolonBase.Load<T>. Reason: {providerKeyResult.Message}", providerKeyResult.DetailedMessage);
                }
                else
                {
                    result.IsError = true;
                    result.Message = CONST_USERMESSAGE_ID_OR_PROVIDERKEY_NOTSET;
                }

                if (result != null && !result.IsError && result.Result != null)
                {
                    SetProperties(result.Result);
                    MapMetaData<T>();
                    OnLoaded?.Invoke(this, new HolonLoadedEventArgs() { Result = OASISResultHelper.CopyResult(result) });
                }
                else
                {
                    OASISErrorHandling.HandleError(ref result, $"Error Occured in HolonBase.Load<T> Calling HolonManager.LoadHolon<T>. Reason: {result.Message}");
                    OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = result.Exception });
                }
            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, $"Unknown Error Occured in HolonBase.Load<T> Calling HolonManager.LoadHolon<T>. Reason: {ex}");
                OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public async Task<OASISResult<IEnumerable<IHolon>>> LoadChildHolonsAsync(HolonType holonType = HolonType.All, bool loadChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true, bool loadChildrenFromProvider = false, int version = 0, ProviderType providerType = ProviderType.Default, bool cache = true)
        {
            OASISResult<IEnumerable<IHolon>> result = new OASISResult<IEnumerable<IHolon>>();

            try
            {
                if (this.Id != Guid.Empty)
                    result = await HolonManager.Instance.LoadHolonsForParentAsync(this.Id, holonType, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, 0, providerType);

                else if (this.ProviderUniqueStorageKey != null && this.ProviderUniqueStorageKey.Count > 0)
                {
                    OASISResult<string> providerKeyResult = GetCurrentProviderKey(providerType);

                    if (!providerKeyResult.IsError && !string.IsNullOrEmpty(providerKeyResult.Result))
                        result = await HolonManager.Instance.LoadHolonsForParentAsync(providerKeyResult.Result, holonType, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, 0, providerType);
                    else
                        OASISErrorHandling.HandleError(ref result, $"Error occured in HolonBase.LoadChildHolonsAsync. Reason: {providerKeyResult.Message}", providerKeyResult.DetailedMessage);
                }
                else
                {
                    result.IsError = true;
                    result.Message = CONST_USERMESSAGE_ID_OR_PROVIDERKEY_NOTSET;
                }

                if (result != null && !result.IsError && result.Result != null)
                {
                    this.Children = result.Result;
                    OnChildrenLoaded?.Invoke(this, new HolonsLoadedEventArgs() { Result = result });
                }
                else
                {
                    OASISErrorHandling.HandleError(ref result, $"Error Occured in HolonBase.LoadChildHolonsAsync Calling HolonManager.LoadHolonsForParentAsync. Reason: {result.Message}");
                    OnChildrenLoadError?.Invoke(this, new HolonsErrorEventArgs() { Reason = result.Message, Exception = result.Exception });
                }

            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, $"Unknown Error Occured in HolonBase.LoadChildHolonsAsync Calling HolonManager.LoadHolonsForParentAsync. Reason: {ex}");
                OnChildrenLoadError?.Invoke(this, new HolonsErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public OASISResult<IEnumerable<IHolon>> LoadChildHolons(HolonType holonType = HolonType.All, bool loadChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true, bool loadChildrenFromProvider = false, int version = 0, ProviderType providerType = ProviderType.Default, bool cache = true)
        {
            OASISResult<IEnumerable<IHolon>> result = new OASISResult<IEnumerable<IHolon>>();

            try
            {
                if (this.Id != Guid.Empty)
                    result = HolonManager.Instance.LoadHolonsForParent(this.Id, holonType, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, 0, providerType);

                else if (this.ProviderUniqueStorageKey != null && this.ProviderUniqueStorageKey.Count > 0)
                {
                    OASISResult<string> providerKeyResult = GetCurrentProviderKey(providerType);

                    if (!providerKeyResult.IsError && !string.IsNullOrEmpty(providerKeyResult.Result))
                        result = HolonManager.Instance.LoadHolonsForParent(providerKeyResult.Result, holonType, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, 0, providerType);
                    else
                        OASISErrorHandling.HandleError(ref result, $"Error occured in HolonBase.LoadChildHolons. Reason: {providerKeyResult.Message}", providerKeyResult.DetailedMessage);
                }
                else
                {
                    result.IsError = true;
                    result.Message = CONST_USERMESSAGE_ID_OR_PROVIDERKEY_NOTSET;
                }

                if (result != null && !result.IsError && result.Result != null)
                {
                    this.Children = result.Result;
                    OnChildrenLoaded?.Invoke(this, new HolonsLoadedEventArgs() { Result = result });
                }
                else
                {
                    OASISErrorHandling.HandleError(ref result, $"Error Occured in HolonBase.LoadChildHolons Calling HolonManager.LoadHolonsForParent. Reason: {result.Message}");
                    OnChildrenLoadError?.Invoke(this, new HolonsErrorEventArgs() { Reason = result.Message, Exception = result.Exception });
                }

            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, $"Unknown Error Occured in HolonBase.LoadChildHolons Calling HolonManager.LoadHolonsForParent. Reason: {ex}");
                OnChildrenLoadError?.Invoke(this, new HolonsErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public async Task<OASISResult<IEnumerable<T>>> LoadChildHolonsAsync<T>(HolonType holonType = HolonType.All, bool loadChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true, bool loadChildrenFromProvider = false, int version = 0, ProviderType providerType = ProviderType.Default, bool cache = true) where T : IHolon, new()
        {
            OASISResult<IEnumerable<T>> result = new OASISResult<IEnumerable<T>>();

            try
            {
                if (this.Id != Guid.Empty)
                    result = await HolonManager.Instance.LoadHolonsForParentAsync<T>(this.Id, holonType, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, 0, providerType);

                else if (this.ProviderUniqueStorageKey != null && this.ProviderUniqueStorageKey.Count > 0)
                {
                    OASISResult<string> providerKeyResult = GetCurrentProviderKey(providerType);

                    if (!providerKeyResult.IsError && !string.IsNullOrEmpty(providerKeyResult.Result))
                        result = await HolonManager.Instance.LoadHolonsForParentAsync<T>(providerKeyResult.Result, holonType, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, 0, providerType);
                    else
                        OASISErrorHandling.HandleError(ref result, $"Error occured in HolonBase.LoadChildHolonsAsync<T>. Reason: {providerKeyResult.Message}", providerKeyResult.DetailedMessage);
                }
                else
                {
                    result.IsError = true;
                    result.Message = CONST_USERMESSAGE_ID_OR_PROVIDERKEY_NOTSET;
                }

                if (result != null && !result.IsError && result.Result != null)
                {
                    this.Children = Mapper.Convert(result.Result);
                    OnChildrenLoaded?.Invoke(this, new HolonsLoadedEventArgs() { Result = OASISResultHelper.CopyResult(result) });
                }
                else
                {
                    OASISErrorHandling.HandleError(ref result, $"Error Occured in HolonBase.LoadChildHolonsAsync<T> Calling HolonManager.LoadHolonsForParentAsync<T>. Reason: {result.Message}");
                    OnChildrenLoadError?.Invoke(this, new HolonsErrorEventArgs() { Reason = result.Message, Exception = result.Exception });
                }

            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, $"Unknown Error Occured in HolonBase.LoadChildHolonsAsync<T> Calling HolonManager.LoadHolonsForParentAsync<T>. Reason: {ex}");
                OnChildrenLoadError?.Invoke(this, new HolonsErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public OASISResult<IEnumerable<T>> LoadChildHolons<T>(HolonType holonType = HolonType.All, bool loadChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true, bool loadChildrenFromProvider = false, int version = 0, ProviderType providerType = ProviderType.Default, bool cache = true) where T : IHolon, new()
        {
            OASISResult<IEnumerable<T>> result = new OASISResult<IEnumerable<T>>();

            try
            {
                if (this.Id != Guid.Empty)
                    result = HolonManager.Instance.LoadHolonsForParent<T>(this.Id, holonType, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, 0, providerType);

                else if (this.ProviderUniqueStorageKey != null && this.ProviderUniqueStorageKey.Count > 0)
                {
                    OASISResult<string> providerKeyResult = GetCurrentProviderKey(providerType);

                    if (!providerKeyResult.IsError && !string.IsNullOrEmpty(providerKeyResult.Result))
                        result = HolonManager.Instance.LoadHolonsForParent<T>(providerKeyResult.Result, holonType, loadChildren, recursive, maxChildDepth, continueOnError, loadChildrenFromProvider, version, 0, providerType);
                    else
                        OASISErrorHandling.HandleError(ref result, $"Error occured in HolonBase.LoadChildHolons<T>. Reason: {providerKeyResult.Message}", providerKeyResult.DetailedMessage);
                }
                else
                {
                    result.IsError = true;
                    result.Message = CONST_USERMESSAGE_ID_OR_PROVIDERKEY_NOTSET;
                }

                if (result != null && !result.IsError && result.Result != null)
                {
                    this.Children = Mapper.Convert(result.Result);
                    OnChildrenLoaded?.Invoke(this, new HolonsLoadedEventArgs() { Result = OASISResultHelper.CopyResult(result) });
                }
                else
                {
                    OASISErrorHandling.HandleError(ref result, $"Error Occured in HolonBase.LoadChildHolons<T> Calling HolonManager.LoadHolonsForParent<T>. Reason: {result.Message}");
                    OnChildrenLoadError?.Invoke(this, new HolonsErrorEventArgs() { Reason = result.Message, Exception = result.Exception });
                }

            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, $"Unknown Error Occured in HolonBase.LoadChildHolons<T> Calling HolonManager.LoadHolonsForParent<T>. Reason: {ex}");
                OnChildrenLoadError?.Invoke(this, new HolonsErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public async Task<OASISResult<IHolon>> SaveAsync(bool saveChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true, bool saveChildrenOnProvider = false, ProviderType providerType = ProviderType.Default)
        {
            OASISResult<IHolon> result = new OASISResult<IHolon>();

            try
            {
                result = await HolonManager.Instance.SaveHolonAsync((IHolon)this, AvatarManager.LoggedInAvatar != null ? AvatarManager.LoggedInAvatar.AvatarId : Guid.Empty, saveChildren, recursive, maxChildDepth, continueOnError, saveChildrenOnProvider, providerType);

                if (result != null && !result.IsError && result.Result != null)
                {
                    SetProperties(result.Result);
                    OnSaved?.Invoke(this, new HolonSavedEventArgs() { Result = result });
                }
                else
                {
                    OASISErrorHandling.HandleError(ref result, $"Error Occured in HolonBase.SaveAsync Calling HolonManager.SaveHolonAsync. Reason: {result.Message}");
                    OnError?.Invoke(this, new HolonErrorEventArgs() { Result = result, Reason = result.Message, Exception = result.Exception });
                }
            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, $"Unknown Error Occured in HolonBase.SaveAsync Calling HolonManager.SaveHolonAsync. Reason: {ex}");
                OnError?.Invoke(this, new HolonErrorEventArgs() { Result = result, Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public async Task<OASISResult<T>> SaveAsync<T>(bool saveChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true, bool saveChildrenOnProvider = false, ProviderType providerType = ProviderType.Default) where T : IHolon, new()
        {
            OASISResult<T> result = new OASISResult<T>();

            try
            {
                result = await HolonManager.Instance.SaveHolonAsync<T>((IHolon)this, AvatarManager.LoggedInAvatar != null ? AvatarManager.LoggedInAvatar.AvatarId : Guid.Empty, saveChildren, recursive, maxChildDepth, continueOnError, saveChildrenOnProvider, providerType);

                if (result != null && !result.IsError && result.Result != null)
                {
                    SetProperties(result.Result);
                    OnSaved?.Invoke(this, new HolonSavedEventArgs() { Result = OASISResultHelper.CopyResult(result) });
                }
                else
                {
                    OASISErrorHandling.HandleError(ref result, $"Error Occured in HolonBase.SaveAsync<T> Calling HolonManager.SaveHolonAsync<T>. Reason: {result.Message}");
                    OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = result.Exception });
                }
            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, $"Unknown Error Occured in HolonBase.SaveAsync<T> Calling HolonManager.SaveHolonAsync<T>. Reason: {ex}");
                OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public OASISResult<IHolon> Save(bool saveChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true, bool saveChildrenOnProvider = false, ProviderType providerType = ProviderType.Default)
        {
            OASISResult<IHolon> result = new OASISResult<IHolon>();

            try
            {
                result = HolonManager.Instance.SaveHolon((IHolon)this, AvatarManager.LoggedInAvatar != null ? AvatarManager.LoggedInAvatar.AvatarId : Guid.Empty, saveChildren, recursive, maxChildDepth, continueOnError, saveChildrenOnProvider, providerType);

                if (result != null && !result.IsError && result.Result != null)
                {
                    SetProperties(result.Result);
                    OnSaved?.Invoke(this, new HolonSavedEventArgs() { Result = result });
                }
                else
                {
                    OASISErrorHandling.HandleError(ref result, $"Error Occured in HolonBase.Save Calling HolonManager.SaveHolon. Reason: {result.Message}");
                    OnError?.Invoke(this, new HolonErrorEventArgs() { Result = result, Reason = result.Message, Exception = result.Exception });
                }
            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, $"Unknown Error Occured in HolonBase.Save Calling HolonManager.SaveHolon. Reason: {ex}");
                OnError?.Invoke(this, new HolonErrorEventArgs() { Result = result, Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public OASISResult<T> Save<T>(bool saveChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true, bool saveChildrenOnProvider = false, ProviderType providerType = ProviderType.Default) where T : IHolon, new()
        {
            OASISResult<T> result = new OASISResult<T>();

            try
            {
                result = HolonManager.Instance.SaveHolon<T>((IHolon)this, AvatarManager.LoggedInAvatar != null ? AvatarManager.LoggedInAvatar.AvatarId : Guid.Empty, saveChildren, recursive, maxChildDepth, continueOnError, saveChildrenOnProvider, providerType);

                if (result != null && !result.IsError && result.Result != null)
                {
                    SetProperties(result.Result);
                    OnSaved?.Invoke(this, new HolonSavedEventArgs() { Result = OASISResultHelper.CopyResult(result) });
                }
                else
                {
                    OASISErrorHandling.HandleError(ref result, $"Error Occured in HolonBase.Save<T> Calling HolonManager.SaveHolon<T>. Reason: {result.Message}");
                    OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = result.Exception });
                }
            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, $"Unknown Error Occured in HolonBase.Save<T> Calling HolonManager.SaveHolon<T>. Reason: {ex}");
                OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public async Task<OASISResult<bool>> DeleteAsync(bool softDelete = true, ProviderType providerType = ProviderType.Default)
        {
            OASISResult<bool> result = new OASISResult<bool>();

            try
            {
                result = await HolonManager.Instance.DeleteHolonAsync(this.Id, softDelete, providerType);

                if (result != null && !result.IsError)
                    OnDeleted?.Invoke(this, new HolonDeletedEventArgs() { Result = result });
                else
                {
                    OASISErrorHandling.HandleError(ref result, string.Concat("Error in HolonBase.DeleteAsync method calling DeleteHolonAsync attempting to delete the holon with ", LoggingHelper.GetHolonInfoForLogging((IHolon)this), ". Error Details: ", result.Message));
                    OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = result.Exception });
                }
            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, $"Unknown Error Occured in HolonBase.DeleteAsync Calling HolonManager.DeleteHolonAsync. Reason: {ex}");
                OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public OASISResult<bool> Delete(bool softDelete = true, ProviderType providerType = ProviderType.Default)
        {
            OASISResult<bool> result = new OASISResult<bool>();

            try
            {
                result = HolonManager.Instance.DeleteHolon(this.Id, softDelete, providerType);

                if (result != null && !result.IsError)
                    OnDeleted?.Invoke(this, new HolonDeletedEventArgs() { Result = result });
                else
                {
                    OASISErrorHandling.HandleError(ref result, string.Concat("Error in HolonBase.Delete method calling DeleteHolon attempting to delete the holon with ", LoggingHelper.GetHolonInfoForLogging((IHolon)this), ". Error Details: ", result.Message));
                    OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = result.Exception });
                }
            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, $"Unknown Error Occured in HolonBase.Delete Calling HolonManager.DeleteHolon. Reason: {ex}");
                OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public async Task<OASISResult<IHolon>> AddHolonAsync(IHolon holon, Guid avatarId, bool saveHolon = true, bool saveChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true, bool saveChildrenOnProvider = false, ProviderType providerType = ProviderType.Default)
        {
            OASISResult<IHolon> result = new OASISResult<IHolon>(holon);

            try
            {
                holon.ParentHolonId = this.Id;
                ((List<IHolon>)this.Children).Add(holon);

                if (saveHolon)
                {
                    result = await HolonManager.Instance.SaveHolonAsync(holon, avatarId, saveChildren, recursive, maxChildDepth, continueOnError, saveChildrenOnProvider, providerType);

                    if (result.IsError)
                    {
                        OASISErrorHandling.HandleError(ref result, string.Concat("Error in HolonBase.AddHolonAsync method calling SaveHolonAsync attempting to save the holon with ", LoggingHelper.GetHolonInfoForLogging(holon), ". Error Details: ", result.Message));
                        OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = result.Exception });
                    }
                    else
                        OnHolonAdded?.Invoke(this, new HolonAddedEventArgs() { Result = OASISResultHelper.CopyResult(result) });
                }
                else
                    OnHolonAdded?.Invoke(this, new HolonAddedEventArgs() { Result = result });
            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, string.Concat("Unknown error occured in HolonBase.AddHolonAsync method attempting to save the holon with ", LoggingHelper.GetHolonInfoForLogging(holon), ". Error Details: ", ex));
                OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public OASISResult<IHolon> AddHolon(IHolon holon, Guid avatarId, bool saveHolon = true, bool saveChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true, bool saveChildrenOnProvider = false, ProviderType providerType = ProviderType.Default)
        {
            OASISResult<IHolon> result = new OASISResult<IHolon>(holon);

            try
            {
                holon.ParentHolonId = this.Id;
                ((List<IHolon>)this.Children).Add(holon);

                if (saveHolon)
                {
                    result = HolonManager.Instance.SaveHolon(holon, avatarId, saveChildren, recursive, maxChildDepth, continueOnError, saveChildrenOnProvider, providerType);

                    if (result.IsError)
                    {
                        OASISErrorHandling.HandleError(ref result, string.Concat("Error in HolonBase.AddHolon method calling SaveHolon attempting to save the holon with ", LoggingHelper.GetHolonInfoForLogging(holon), ". Error Details: ", result.Message));
                        OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = result.Exception });
                    }
                    else
                        OnHolonAdded?.Invoke(this, new HolonAddedEventArgs() { Result = OASISResultHelper.CopyResult(result) });
                }
                else
                    OnHolonAdded?.Invoke(this, new HolonAddedEventArgs() { Result = result });
            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, string.Concat("Unknown error occured in HolonBase.AddHolon method attempting to save the holon with ", LoggingHelper.GetHolonInfoForLogging(holon), ". Error Details: ", ex));
                OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public async Task<OASISResult<T>> AddHolonAsync<T>(T holon, Guid avatarId, bool saveHolon = true, bool saveChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true, bool saveChildrenOnProvider = false, ProviderType providerType = ProviderType.Default) where T : IHolon, new()
        {
            OASISResult<T> result = new OASISResult<T>(holon);

            try
            {
                holon.ParentHolonId = this.Id;
                ((List<IHolon>)this.Children).Add(holon);

                if (saveHolon)
                {
                    result = await HolonManager.Instance.SaveHolonAsync<T>(holon, avatarId, saveChildren, recursive, maxChildDepth, continueOnError, saveChildrenOnProvider, providerType);

                    if (result.IsError)
                    {
                        OASISErrorHandling.HandleError(ref result, string.Concat("Error in HolonBase.AddHolonAsync<T> method calling SaveHolonAsync<T> attempting to save the holon with ", LoggingHelper.GetHolonInfoForLogging(holon), ". Error Details: ", result.Message));
                        OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = result.Exception });
                    }
                    else
                        OnHolonAdded?.Invoke(this, new HolonAddedEventArgs() { Result = OASISResultHelper.CopyResult(result) });
                }
                else
                    OnHolonAdded?.Invoke(this, new HolonAddedEventArgs() { Result = OASISResultHelper.CopyResult(result) });
            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, string.Concat("Unknown error occured in HolonBase.AddHolonAsync<T> method attempting to save the holon with ", LoggingHelper.GetHolonInfoForLogging(holon), ". Error Details: ", ex));
                OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public OASISResult<T> AddHolon<T>(T holon, Guid avatarId, bool saveHolon = true, bool saveChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true, bool saveChildrenOnProvider = false, ProviderType providerType = ProviderType.Default) where T : IHolon, new()
        {
            OASISResult<T> result = new OASISResult<T>(holon);

            try
            {
                holon.ParentHolonId = this.Id;
                ((List<IHolon>)this.Children).Add(holon);

                if (saveHolon)
                {
                    result = HolonManager.Instance.SaveHolon<T>(holon, avatarId, saveChildren, recursive, maxChildDepth, continueOnError, saveChildrenOnProvider, providerType);

                    if (result.IsError)
                    {
                        OASISErrorHandling.HandleError(ref result, string.Concat("Error in HolonBase.AddHolon<T> method calling SaveHolon<T> attempting to save the holon with ", LoggingHelper.GetHolonInfoForLogging(holon), ". Error Details: ", result.Message));
                        OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = result.Exception });
                    }
                    else
                        OnHolonAdded?.Invoke(this, new HolonAddedEventArgs() { Result = OASISResultHelper.CopyResult(result) });
                }
                else
                    OnHolonAdded?.Invoke(this, new HolonAddedEventArgs() { Result = OASISResultHelper.CopyResult(result) });
            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, string.Concat("Unknown error occured in HolonBase.AddHolon<T> method attempting to save the holon with ", LoggingHelper.GetHolonInfoForLogging(holon), ". Error Details: ", ex));
                OnError?.Invoke(this, new HolonErrorEventArgs() { Result = OASISResultHelper.CopyResult(result), Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public async Task<OASISResult<IHolon>> RemoveHolonAsync(IHolon holon, bool deleteHolon = false, bool softDelete = true, ProviderType providerType = ProviderType.Default)
        {
            OASISResult<IHolon> result = new OASISResult<IHolon>(holon);

            try
            {
                holon.ParentHolonId = Guid.Empty;
                ((List<IHolon>)this.Children).Remove(holon);

                if (deleteHolon)
                {
                    OASISResult<bool> deleteHolonResult = await HolonManager.Instance.DeleteHolonAsync(holon.Id, softDelete, providerType);

                    if (deleteHolonResult.IsError)
                    {
                        OASISErrorHandling.HandleError(ref result, string.Concat("Error in HolonBase.RemoveHolonAsync method calling DeleteHolonAsync attempting to delete the holon with ", LoggingHelper.GetHolonInfoForLogging(holon), ". Error Details: ", result.Message));
                        OnError?.Invoke(this, new HolonErrorEventArgs() { Result = result, Reason = result.Message, Exception = result.Exception });
                    }
                    else
                        OnHolonRemoved?.Invoke(this, new HolonRemovedEventArgs() { Result = result });
                }
                else
                    OnHolonRemoved?.Invoke(this, new HolonRemovedEventArgs() { Result = result });
            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, string.Concat("Unknown error occured in HolonBase.RemoveHolonAsync method attempting to remove the holon with ", LoggingHelper.GetHolonInfoForLogging(holon), ". Error Details: ", ex));
                OnError?.Invoke(this, new HolonErrorEventArgs() { Result = result, Reason = result.Message, Exception = ex });
            }

            return result;
        }

        public OASISResult<IHolon> RemoveHolon(IHolon holon, bool deleteHolon = false, bool softDelete = true, ProviderType providerType = ProviderType.Default)
        {
            OASISResult<IHolon> result = new OASISResult<IHolon>(holon);

            try
            {
                holon.ParentHolonId = Guid.Empty;
                ((List<IHolon>)this.Children).Remove(holon);

                if (deleteHolon)
                {
                    OASISResult<bool> deleteHolonResult = HolonManager.Instance.DeleteHolon(holon.Id, softDelete, providerType);

                    if (deleteHolonResult.IsError)
                    {
                        OASISErrorHandling.HandleError(ref result, string.Concat("Error in HolonBase.RemoveHolon method calling DeleteHolon attempting to delete the holon with ", LoggingHelper.GetHolonInfoForLogging(holon), ". Error Details: ", result.Message));
                        OnError?.Invoke(this, new HolonErrorEventArgs() { Result = result, Reason = result.Message, Exception = result.Exception });
                    }
                    else
                        OnHolonRemoved?.Invoke(this, new HolonRemovedEventArgs() { Result = result });
                }
                else
                    OnHolonRemoved?.Invoke(this, new HolonRemovedEventArgs() { Result = result });
            }
            catch (Exception ex)
            {
                OASISErrorHandling.HandleError(ref result, string.Concat("Unknown error occured in HolonBase.RemoveHolon method attempting to remove the holon with ", LoggingHelper.GetHolonInfoForLogging(holon), ". Error Details: ", ex));
                OnError?.Invoke(this, new HolonErrorEventArgs() { Result = result, Reason = result.Message, Exception = ex });
            }

            return result;
        }


        /*
        //https://stackoverflow.com/questions/2363801/what-would-be-the-best-way-to-implement-change-tracking-on-an-object
        //https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.ichangetracking?redirectedfrom=MSDN&view=net-5.0

        protected bool SetProperty<T>(string name, ref T oldValue, T newValue) where T : IComparable<T>
        {
            if (oldValue == null || oldValue.CompareTo(newValue) != 0)
            {
                oldValue = newValue;
               // PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));
               // isDirty = true;
                return true;
            }
            return false;
        }
        // For nullable types
        protected void SetProperty<T>(string name, ref Nullable<T> oldValue, Nullable<T> newValue) where T : struct, IComparable<T>
        {
            if (oldValue.HasValue != newValue.HasValue || (newValue.HasValue && oldValue.Value.CompareTo(newValue.Value) != 0))
            {
                oldValue = newValue;
                //PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));
            }
        }*/

        //private void SetProperties(OASISResult<IHolon> result)
        //{
        //    this.Name = result.Result.Name;
        //    this.Description = result.Result.Description;
        //    this.CreatedByAvatar = result.Result.CreatedByAvatar;
        //    this.CreatedByAvatarId = result.Result.CreatedByAvatarId;
        //    this.CreatedDate = result.Result.CreatedDate;
        //    this.CreatedOASISType = result.Result.CreatedOASISType;
        //    this.CreatedProviderType = result.Result.CreatedProviderType;
        //    this.CustomKey = result.Result.CustomKey;
        //    this.DeletedByAvatar = result.Result.DeletedByAvatar;
        //    this.DeletedByAvatarId = result.Result.DeletedByAvatarId;
        //    this.DeletedDate = result.Result.DeletedDate;
        //    this.HolonType = result.Result.HolonType;
        //    this.Id = result.Result.Id;
        //    this.InstanceSavedOnProviderType = result.Result.InstanceSavedOnProviderType;
        //    this.IsActive = result.Result.IsActive;
        //    this.IsChanged = result.Result.IsChanged;
        //    this.IsNewHolon = result.Result.IsNewHolon;
        //    this.IsSaving = result.Result.IsSaving;
        //    this.MetaData = result.Result.MetaData;
        //    this.ModifiedByAvatar = result.Result.ModifiedByAvatar;
        //    this.ModifiedByAvatarId = result.Result.ModifiedByAvatarId;
        //    this.ModifiedDate = result.Result.ModifiedDate;
        //    this.Original = result.Result.Original;
        //    this.PreviousVersionId = result.Result.PreviousVersionId;
        //    this.PreviousVersionProviderUniqueStorageKey = result.Result.PreviousVersionProviderUniqueStorageKey;
        //    this.ProviderMetaData = result.Result.ProviderMetaData;
        //    this.ProviderUniqueStorageKey = result.Result?.ProviderUniqueStorageKey;
        //    this.Version = result.Result.Version;
        //    this.VersionId = result.Result.VersionId;
        //    //this = Mapper<IHolon, HolonBase>.MapBaseHolonProperties(result.Result);
        //}

        private void SetProperties(IHolon holon)
        {
            this.Name = holon.Name;
            this.Description = holon.Description;
            this.CreatedByAvatar = holon.CreatedByAvatar;
            this.CreatedByAvatarId = holon.CreatedByAvatarId;
            this.CreatedDate = holon.CreatedDate;
            this.CreatedOASISType = holon.CreatedOASISType;
            this.CreatedProviderType = holon.CreatedProviderType;
            this.CustomKey = holon.CustomKey;
            this.DeletedByAvatar = holon.DeletedByAvatar;
            this.DeletedByAvatarId = holon.DeletedByAvatarId;
            this.DeletedDate = holon.DeletedDate;
            this.HolonType = holon.HolonType;
            this.Id = holon.Id;
            this.InstanceSavedOnProviderType = holon.InstanceSavedOnProviderType;
            this.IsActive = holon.IsActive;
            this.IsChanged = holon.IsChanged;
            this.IsNewHolon = holon.IsNewHolon;
            this.IsSaving = holon.IsSaving;
            this.MetaData = holon.MetaData;
            this.ModifiedByAvatar = holon.ModifiedByAvatar;
            this.ModifiedByAvatarId = holon.ModifiedByAvatarId;
            this.ModifiedDate = holon.ModifiedDate;
            this.Original = holon.Original;
            this.PreviousVersionId = holon.PreviousVersionId;
            this.PreviousVersionProviderUniqueStorageKey = holon.PreviousVersionProviderUniqueStorageKey;
            this.ProviderMetaData = holon.ProviderMetaData;
            this.ProviderUniqueStorageKey = holon?.ProviderUniqueStorageKey;
            this.Version = holon.Version;
            this.VersionId = holon.VersionId;
        }

        private void MapMetaData<T>()
        {
            if (this.MetaData != null && this.MetaData.Count > 0)
            {
                foreach (string key in this.MetaData.Keys)
                {
                    PropertyInfo propInfo = typeof(T).GetProperty(key);

                    if (propInfo != null)
                    {
                        if (propInfo.PropertyType == typeof(Guid))
                            propInfo.SetValue(this, new Guid(this.MetaData[key].ToString()));

                        else if (propInfo.PropertyType == typeof(bool))
                            propInfo.SetValue(this, Convert.ToBoolean(this.MetaData[key]));

                        else if (propInfo.PropertyType == typeof(DateTime))
                            propInfo.SetValue(this, Convert.ToDateTime(this.MetaData[key]));

                        else if (propInfo.PropertyType == typeof(int))
                            propInfo.SetValue(this, Convert.ToInt32(this.MetaData[key]));

                        else if (propInfo.PropertyType == typeof(long))
                            propInfo.SetValue(this, Convert.ToInt64(this.MetaData[key]));

                        else if (propInfo.PropertyType == typeof(float))
                            propInfo.SetValue(this, Convert.ToDouble(this.MetaData[key])); //TODO: Check if this is right?! :)

                        else if (propInfo.PropertyType == typeof(double))
                            propInfo.SetValue(this, Convert.ToDouble(this.MetaData[key]));

                        else if (propInfo.PropertyType == typeof(decimal))
                            propInfo.SetValue(this, Convert.ToDecimal(this.MetaData[key]));

                        else if (propInfo.PropertyType == typeof(UInt16))
                            propInfo.SetValue(this, Convert.ToUInt16(this.MetaData[key]));

                        else if (propInfo.PropertyType == typeof(UInt32))
                            propInfo.SetValue(this, Convert.ToUInt32(this.MetaData[key]));

                        else if (propInfo.PropertyType == typeof(UInt64))
                            propInfo.SetValue(this, Convert.ToUInt64(this.MetaData[key]));

                        else if (propInfo.PropertyType == typeof(Single))
                            propInfo.SetValue(this, Convert.ToSingle(this.MetaData[key]));

                        else if (propInfo.PropertyType == typeof(char))
                            propInfo.SetValue(this, Convert.ToChar(this.MetaData[key]));

                        else if (propInfo.PropertyType == typeof(byte))
                            propInfo.SetValue(this, Convert.ToByte(this.MetaData[key]));

                        else if (propInfo.PropertyType == typeof(sbyte))
                            propInfo.SetValue(this, Convert.ToSByte(this.MetaData[key]));

                        else
                            propInfo.SetValue(this, this.MetaData[key]);
                    }

                    //TODO: Add any other missing types...
                }

                //this(IHolonBase) = HolonManager.Instance.MapMetaData<T>((IHolon)this);
            }
        }

        private OASISResult<string> GetCurrentProviderKey(ProviderType providerType = ProviderType.Default)
        {
            OASISResult<string> result = new OASISResult<string>();

            if (providerType == ProviderType.Default || providerType == ProviderType.All || providerType == ProviderType.None)
                providerType = ProviderManager.Instance.CurrentStorageProviderType.Value;

            if (ProviderUniqueStorageKey.ContainsKey(providerType) && !string.IsNullOrEmpty(ProviderUniqueStorageKey[providerType]))
                result.Result = ProviderUniqueStorageKey[providerType];
            else
                OASISErrorHandling.HandleError(ref result, string.Concat("ProviderUniqueStorageKey not found for CurrentStorageProviderType ", Enum.GetName(typeof(ProviderType), providerType)));

            return result;
        }

        private void GetGreatGrandSuperStar(ref OASISResult<IHolon> result, OASISResult<IEnumerable<IHolon>> holonsResult)
        {
            if (!holonsResult.IsError && holonsResult.Result != null)
            {
                List<IHolon> holons = (List<IHolon>)holonsResult.Result;

                if (holons.Count == 1)
                    result.Result = holons[0];
                else
                {
                    result.IsError = true;
                    result.Message = "ERROR, there should only be one GreatGrandSuperStar!";
                }
            }
        }

        private void GetGreatGrandSuperStar<T>(ref OASISResult<T> result, OASISResult<IEnumerable<IHolon>> holonsResult)
        {
            if (!holonsResult.IsError && holonsResult.Result != null)
            {
                List<T> holons = (List<T>)holonsResult.Result;

                if (holons.Count == 1)
                    result.Result = holons[0];
                else
                {
                    result.IsError = true;
                    result.Message = "ERROR, there should only be one GreatGrandSuperStar!";
                }
            }
        }
    }
}
