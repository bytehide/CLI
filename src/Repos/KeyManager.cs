using System;
using SecureLocalStorage;
using ShieldCLI.Models;

namespace ShieldCLI.Repos
{
    internal class KeyManager
    {
        internal SecureLocalStorage.SecureLocalStorage Storage { get; set; }

        internal UserConfig UserConfig { get; set; }

        private readonly string _userConfig = "user_config";

        /// <summary>
        /// Used for manage local storage user properties such a dotnetsafer api key and account setting.
        /// </summary>
        public KeyManager()
        {
            Storage = new SecureLocalStorage.SecureLocalStorage(new CustomLocalStorageConfig(null,"dotnetsafer_shield_cli")
                .WithDefaultKeyBuilder());

            if (Storage.Exists(_userConfig))
                UserConfig = Storage.Get<UserConfig>(_userConfig) ?? new UserConfig();
        }

        /// <summary>
        /// Gets if current computer user has any dotnetsafer key stored.
        /// </summary>
        /// <returns></returns>
        internal bool HasKey() =>
            !string.IsNullOrEmpty(UserConfig.ApiKey);

        /// <summary>
        /// Checks if <param name="key">key</param> is valid.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal bool IsValidKey(string key) => throw new NotImplementedException("TODO: Luis");

        /// <summary>
        /// Checks if current computer user has active and valid key.
        /// </summary>
        /// <returns></returns>
        internal bool HasValidKey() => HasKey() && IsValidKey(UserConfig.ApiKey);

        /// <summary>
        /// Gets current user key.
        /// </summary>
        internal string Key => UserConfig.ApiKey;

        /// <summary>
        /// Update current computer user dotnetsafer key.
        /// </summary>
        /// <param name="key"></param>
        internal void UpdateKey(string key)
        {
            UserConfig.ApiKey = key;
            Storage.Set(_userConfig, UserConfig);
        }
    }
}
