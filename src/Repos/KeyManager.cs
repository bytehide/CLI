using System;
using SecureLocalStorage;
using ShieldCLI.Models;

namespace ShieldCLI.Repos
{
    public class KeyManager
    {
        private SecureLocalStorage.SecureLocalStorage Storage { get; set; }

        private UserConfig UserConfig { get; set; }

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
        public bool HasKey() =>
            !string.IsNullOrEmpty(UserConfig.ApiKey);

        /// <summary>
        /// Checks if <param name="key">key</param> is valid.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsValidKey(string key) => throw new NotImplementedException("TODO: Luis");

        /// <summary>
        /// Checks if current computer user has active and valid key.
        /// </summary>
        /// <returns></returns>
        public bool HasValidKey() => HasKey() && IsValidKey(UserConfig.ApiKey);

        /// <summary>
        /// Gets current user key.
        /// </summary>
        public string Key => UserConfig.ApiKey;

        /// <summary>
        /// Update current computer user dotnetsafer key.
        /// </summary>
        /// <param name="key"></param>
        public void UpdateKey(string key)
        {
            UserConfig.ApiKey = key;
            Storage.Set(_userConfig, UserConfig);
        }
    }
}
