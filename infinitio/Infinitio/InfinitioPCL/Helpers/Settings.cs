// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace InfinitioPCL.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class Settings
  {
    private static ISettings AppSettings
    {
      get
      {
        return CrossSettings.Current;
      }
    }

    #region Setting Constants

    private const string SettingsKey = "settings_key";
    private static readonly string SettingsDefault = string.Empty;
	public static readonly string DefaultString = string.Empty;
	public const string UserIDKey = "userID";
	public const string FirstNameKey = "firstName";

    #endregion


    public static string GeneralSettings
    {
      get
      {
        return AppSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
      }
      set
      {
        AppSettings.AddOrUpdateValue<string>(SettingsKey, value);
      }
    }
	
	public static string UserID
		{
			get
			{
				return AppSettings.GetValueOrDefault(UserIDKey, DefaultString);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(UserIDKey, value);
			}
		}
		public static string Name
		{
			get
			{
				return AppSettings.GetValueOrDefault(FirstNameKey, DefaultString);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(FirstNameKey, value);
			}
		}

  }
}